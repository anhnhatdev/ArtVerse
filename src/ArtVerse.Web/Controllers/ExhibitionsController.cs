using ArtVerse.Application.Exhibitions.Commands;
using ArtVerse.Application.Exhibitions.Queries;
using ArtVerse.Application.Paintings.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArtVerse.Web.Controllers;

public class ExhibitionsController : Controller
{
    private readonly IMediator _mediator;

    public ExhibitionsController(IMediator mediator) => _mediator = mediator;

    // GET: /Exhibitions
    [AllowAnonymous]
    public async Task<IActionResult> Index(string? search, int page = 1)
    {
        const int pageSize = 6;
        var (exhibitions, totalCount) = await _mediator.Send(new GetExhibitionsQuery(search, page, pageSize));

        ViewBag.Search = search;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        ViewBag.TotalCount = totalCount;

        return View(exhibitions);
    }

    // GET: /Exhibitions/Showcase/{id}
    [AllowAnonymous]
    public async Task<IActionResult> Showcase(Guid id)
    {
        var exhibition = await _mediator.Send(new GetExhibitionShowcaseQuery(id));
        if (exhibition == null) return NotFound();
        return View(exhibition);
    }

    // GET: /Exhibitions/Curator/{id}
    [Authorize(Roles = "Admin,Principal,Manager,Staff")]
    public async Task<IActionResult> Curator(Guid id)
    {
        var exhibition = await _mediator.Send(new GetExhibitionShowcaseQuery(id));
        if (exhibition == null) return NotFound();

        var (paintings, _) = await _mediator.Send(new GetGalleryPaintingsQuery(null, null, 1, 100));
        ViewBag.Paintings = new SelectList(paintings, "Id", "Title");

        return View(exhibition);
    }

    // POST: /Exhibitions/CurateArtwork
    [HttpPost]
    [Authorize(Roles = "Admin,Principal,Manager,Staff")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CurateArtwork(Guid exhibitionId, Guid paintingId, int displayOrder)
    {
        try
        {
            await _mediator.Send(new CurateArtworkCommand(exhibitionId, paintingId, displayOrder));
            TempData["Success"] = "Đã tuyển chọn tác phẩm vào danh mục triển lãm thành công!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Curator), new { id = exhibitionId });
    }

    // POST: /Exhibitions/Like/{artworkId} (AJAX supported)
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Like(Guid artworkId, Guid exhibitionId)
    {
        var newLikes = await _mediator.Send(new LikeArtworkCommand(artworkId));
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return Json(new { success = true, likes = newLikes });
        }
        return RedirectToAction(nameof(Showcase), new { id = exhibitionId });
    }
}
