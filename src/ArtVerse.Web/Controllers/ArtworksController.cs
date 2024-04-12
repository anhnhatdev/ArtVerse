using ArtVerse.Application.Common.Interfaces;
using ArtVerse.Application.Paintings.Commands;
using ArtVerse.Application.Paintings.DTOs;
using ArtVerse.Application.Paintings.Queries;
using ArtVerse.Application.Students.Queries;
using ArtVerse.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArtVerse.Web.Controllers;

public class ArtworksController : Controller
{
    private readonly IMediator _mediator;
    private readonly IFileStorageService _fileStorage;

    public ArtworksController(IMediator mediator, IFileStorageService fileStorage)
    {
        _mediator = mediator;
        _fileStorage = fileStorage;
    }

    // GET: /Artworks
    [AllowAnonymous]
    public async Task<IActionResult> Index(string? search, ArtTechnique? technique, int page = 1)
    {
        const int pageSize = 8;
        var (paintings, totalCount) = await _mediator.Send(new GetGalleryPaintingsQuery(search, technique, page, pageSize));

        ViewBag.Search = search;
        ViewBag.Technique = technique;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        ViewBag.TotalCount = totalCount;

        return View(paintings);
    }

    // GET: /Artworks/Details/{id}
    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid id)
    {
        var painting = await _mediator.Send(new GetPaintingDetailsQuery(id));
        if (painting == null) return NotFound();
        return View(painting);
    }

    // GET: /Artworks/Upload
    [Authorize]
    public async Task<IActionResult> Upload()
    {
        var (students, _) = await _mediator.Send(new GetStudentsQuery(null, 1, 100));
        ViewBag.Students = new SelectList(students, "Id", "FullName");
        return View();
    }

    // POST: /Artworks/Upload
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(
        string title, 
        string? description, 
        Guid studentId, 
        ArtTechnique technique, 
        int? createdYear, 
        bool isForSale, 
        decimal? basePrice, 
        IFormFile? imageFile)
    {
        try
        {
            string? savedFileUrl = null;
            long fileSize = 0;
            string? fileName = null;

            if (imageFile != null && imageFile.Length > 0)
            {
                using var stream = imageFile.OpenReadStream();
                savedFileUrl = await _fileStorage.SaveFileAsync(stream, imageFile.FileName, "artworks");
                fileSize = imageFile.Length;
                fileName = imageFile.FileName;
            }

            var dto = new CreatePaintingDto(title, description, technique, createdYear, isForSale, basePrice, studentId, fileName);
            var paintingId = await _mediator.Send(new CreatePaintingCommand(dto, savedFileUrl, fileSize));

            // Nộp duyệt tự động
            await _mediator.Send(new SubmitPaintingCommand(paintingId));

            TempData["Success"] = "Tải lên tác phẩm thành công và đã chuyển tới Hàng đợi duyệt của Hội đồng nghệ thuật!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            var (students, _) = await _mediator.Send(new GetStudentsQuery(null, 1, 100));
            ViewBag.Students = new SelectList(students, "Id", "FullName");
            return View();
        }
    }

    // GET: /Artworks/ReviewQueue
    [Authorize(Roles = "Admin,Principal,Manager,Staff")]
    public async Task<IActionResult> ReviewQueue()
    {
        var items = await _mediator.Send(new GetPendingReviewPaintingsQuery());
        return View(items);
    }

    // POST: /Artworks/Approve/{id}
    [HttpPost]
    [Authorize(Roles = "Admin,Principal,Manager,Staff")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(Guid id)
    {
        try
        {
            await _mediator.Send(new ApprovePaintingCommand(id));
            TempData["Success"] = "Đã phê duyệt tác phẩm thành công!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(ReviewQueue));
    }

    // POST: /Artworks/Reject/{id}
    [HttpPost]
    [Authorize(Roles = "Admin,Principal,Manager,Staff")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(Guid id, string reason)
    {
        try
        {
            await _mediator.Send(new RejectPaintingCommand(id, reason));
            TempData["Success"] = "Đã từ chối tác phẩm và gửi phản hồi lý do.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(ReviewQueue));
    }
}
