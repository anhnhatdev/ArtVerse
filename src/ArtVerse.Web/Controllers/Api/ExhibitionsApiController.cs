using Microsoft.AspNetCore.Authentication.JwtBearer;
using ArtVerse.Application.Exhibitions.Commands;
using ArtVerse.Application.Exhibitions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtVerse.Web.Controllers.Api;

[ApiController]
[Route("api/v1/exhibitions")]
public class ExhibitionsApiController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExhibitionsApiController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetExhibitions([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 12)
    {
        var (items, totalCount) = await _mediator.Send(new GetExhibitionsQuery(search, page, pageSize));
        return Ok(new
        {
            success = true,
            data = new
            {
                items,
                totalCount,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            }
        });
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetExhibitionById(Guid id)
    {
        var exhibition = await _mediator.Send(new GetExhibitionShowcaseQuery(id));
        if (exhibition == null) return NotFound(new { success = false, message = "Không tìm thấy triển lãm." });
        return Ok(new { success = true, data = exhibition });
    }

    [HttpPost("{id:guid}/curate")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Principal,Manager,Staff")]
    public async Task<IActionResult> CurateArtwork(Guid id, [FromBody] CurateArtworkApiRequest request)
    {
        var curatedId = await _mediator.Send(new CurateArtworkCommand(id, request.PaintingId, request.DisplayOrder));
        return Ok(new { success = true, message = "Đã tuyển chọn tác phẩm vào triển lãm!", data = new { curatedId } });
    }

    [HttpPost("artworks/{artworkId:guid}/like")]
    [AllowAnonymous]
    public async Task<IActionResult> LikeArtwork(Guid artworkId)
    {
        var likes = await _mediator.Send(new LikeArtworkCommand(artworkId));
        return Ok(new { success = true, likes });
    }

    public record CurateArtworkApiRequest(Guid PaintingId, int DisplayOrder = 0);
}

