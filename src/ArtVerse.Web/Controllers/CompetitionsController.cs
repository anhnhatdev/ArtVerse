using ArtVerse.Application.Competitions.Commands;
using ArtVerse.Application.Competitions.Queries;
using ArtVerse.Application.Paintings.Queries;
using ArtVerse.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArtVerse.Web.Controllers;

public class CompetitionsController : Controller
{
    private readonly IMediator _mediator;

    public CompetitionsController(IMediator mediator) => _mediator = mediator;

    // GET: /Competitions
    [AllowAnonymous]
    public async Task<IActionResult> Index(string? search, int page = 1)
    {
        const int pageSize = 6;
        var (competitions, totalCount) = await _mediator.Send(new GetCompetitionsQuery(search, page, pageSize));

        ViewBag.Search = search;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        ViewBag.TotalCount = totalCount;

        return View(competitions);
    }

    // GET: /Competitions/Details/{id}
    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid id)
    {
        var competition = await _mediator.Send(new GetCompetitionDetailsQuery(id));
        if (competition == null) return NotFound();

        var entries = await _mediator.Send(new GetCompetitionEntriesQuery(id));
        ViewBag.Entries = entries;

        return View(competition);
    }

    // GET: /Competitions/SubmitEntry/{id}
    [Authorize]
    public async Task<IActionResult> SubmitEntry(Guid id)
    {
        var competition = await _mediator.Send(new GetCompetitionDetailsQuery(id));
        if (competition == null) return NotFound();

        var (paintings, _) = await _mediator.Send(new GetGalleryPaintingsQuery(null, null, 1, 100));
        ViewBag.Paintings = new SelectList(paintings, "Id", "Title");

        return View(competition);
    }

    // POST: /Competitions/SubmitEntry
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitEntry(Guid competitionId, Guid paintingId, Guid studentId)
    {
        try
        {
            await _mediator.Send(new SubmitEntryCommand(competitionId, paintingId, studentId));
            TempData["Success"] = "Nộp bài dự thi thành công! Chúc bạn đạt giải cao.";
            return RedirectToAction(nameof(Details), new { id = competitionId });
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(SubmitEntry), new { id = competitionId });
        }
    }

    // GET: /Competitions/JudgingRoom/{id}
    [Authorize(Roles = "Admin,Principal,Staff")]
    public async Task<IActionResult> JudgingRoom(Guid id)
    {
        var competition = await _mediator.Send(new GetCompetitionDetailsQuery(id));
        if (competition == null) return NotFound();

        var entries = await _mediator.Send(new GetCompetitionEntriesQuery(id));
        ViewBag.Entries = entries;

        return View(competition);
    }

    // POST: /Competitions/ScoreEntry
    [HttpPost]
    [Authorize(Roles = "Admin,Principal,Staff")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ScoreEntry(Guid entryId, Guid judgeId, Guid criteriaId, decimal score, string? comment, Guid competitionId)
    {
        try
        {
            await _mediator.Send(new SubmitScoreCommand(entryId, judgeId, criteriaId, score, comment));
            TempData["Success"] = "Đã lưu điểm đánh giá thành công!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(JudgingRoom), new { id = competitionId });
    }

    // GET: /Competitions/Leaderboard/{id}
    [AllowAnonymous]
    public async Task<IActionResult> Leaderboard(Guid id)
    {
        var competition = await _mediator.Send(new GetCompetitionDetailsQuery(id));
        if (competition == null) return NotFound();

        var entries = await _mediator.Send(new GetCompetitionEntriesQuery(id));
        var rankedEntries = entries.OrderByDescending(e => e.AverageScore).ToList();

        ViewBag.Competition = competition;
        return View(rankedEntries);
    }
}
