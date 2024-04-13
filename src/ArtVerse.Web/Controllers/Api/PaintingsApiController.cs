using ArtVerse.Application.Common.Interfaces;
using ArtVerse.Application.Paintings.Commands;
using ArtVerse.Application.Paintings.DTOs;
using ArtVerse.Application.Paintings.Queries;
using ArtVerse.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtVerse.Web.Controllers.Api;

[ApiController]
[Route("api/v1/paintings")]
public class PaintingsApiController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IFileStorageService _fileStorage;

    public PaintingsApiController(IMediator mediator, IFileStorageService fileStorage)
    {
        _mediator = mediator;
        _fileStorage = fileStorage;
    }

    // GET: /api/v1/paintings
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetPaintings(
        [FromQuery] string? search,
        [FromQuery] ArtTechnique? technique,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12)
    {
        var (paintings, totalCount) = await _mediator.Send(new GetGalleryPaintingsQuery(search, technique, page, pageSize));
        return Ok(new
        {
            success = true,
            data = new
            {
                items = paintings,
                totalCount,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            }
        });
    }

    // GET: /api/v1/paintings/{id}
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPaintingById(Guid id)
    {
        var painting = await _mediator.Send(new GetPaintingDetailsQuery(id));
        if (painting == null)
            return NotFound(new { success = false, message = "Không tìm thấy tác phẩm." });

        return Ok(new { success = true, data = painting });
    }

    // POST: /api/v1/paintings/upload
    [HttpPost("upload")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UploadPainting([FromForm] UploadPaintingApiRequest request)
    {
        try
        {
            string? savedFileUrl = null;
            long fileSize = 0;
            string? fileName = null;

            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                using var stream = request.ImageFile.OpenReadStream();
                savedFileUrl = await _fileStorage.SaveFileAsync(stream, request.ImageFile.FileName, "artworks");
                fileSize = request.ImageFile.Length;
                fileName = request.ImageFile.FileName;
            }

            var dto = new CreatePaintingDto(
                request.Title,
                request.Description,
                request.Technique,
                request.CreatedYear,
                request.IsForSale,
                request.BasePrice,
                request.StudentId,
                fileName
            );

            var paintingId = await _mediator.Send(new CreatePaintingCommand(dto, savedFileUrl, fileSize));
            await _mediator.Send(new SubmitPaintingCommand(paintingId));

            return Ok(new
            {
                success = true,
                message = "Tải lên tác phẩm thành công và đã chuyển tới Hàng đợi duyệt!",
                data = new { id = paintingId, fileUrl = savedFileUrl }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // GET: /api/v1/paintings/pending-reviews
    [HttpGet("pending-reviews")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Principal,Manager,Staff")]
    public async Task<IActionResult> GetPendingReviews()
    {
        var items = await _mediator.Send(new GetPendingReviewPaintingsQuery());
        return Ok(new { success = true, data = items });
    }

    // POST: /api/v1/paintings/{id}/approve
    [HttpPost("{id:guid}/approve")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Principal,Manager,Staff")]
    public async Task<IActionResult> ApprovePainting(Guid id)
    {
        var result = await _mediator.Send(new ApprovePaintingCommand(id));
        return Ok(new { success = result, message = "Đã phê duyệt tác phẩm thành công!" });
    }

    // POST: /api/v1/paintings/{id}/reject
    [HttpPost("{id:guid}/reject")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Principal,Manager,Staff")]
    public async Task<IActionResult> RejectPainting(Guid id, [FromBody] RejectRequest request)
    {
        var result = await _mediator.Send(new RejectPaintingCommand(id, request.Reason));
        return Ok(new { success = result, message = "Đã từ chối tác phẩm và phản hồi lý do." });
    }

    public class UploadPaintingApiRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid StudentId { get; set; }
        public ArtTechnique Technique { get; set; }
        public int? CreatedYear { get; set; } = 2026;
        public bool IsForSale { get; set; } = true;
        public decimal? BasePrice { get; set; }
        public IFormFile? ImageFile { get; set; }
    }

    public record RejectRequest(string Reason);
}

