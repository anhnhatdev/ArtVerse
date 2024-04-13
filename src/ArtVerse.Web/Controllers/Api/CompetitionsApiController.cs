using Microsoft.AspNetCore.Authentication.JwtBearer;
using ArtVerse.Application.Competitions.Commands;
using ArtVerse.Application.Competitions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtVerse.Web.Controllers.Api;

[ApiController]
[Route("api/v1/competitions")]
public class CompetitionsApiController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompetitionsApiController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCompetitions([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 12)
    {
        var (items, totalCount) = await _mediator.Send(new GetCompetitionsQuery(search, page, pageSize));
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
    public async Task<IActionResult> GetCompetitionById(Guid id)
    {
        var item = await _mediator.Send(new GetCompetitionDetailsQuery(id));
        if (item == null) return NotFound(new { success = false, message = "Không tìm thấy cuộc thi." });
        return Ok(new { success = true, data = item });
    }

    [HttpGet("{id:guid}/entries")]
    [AllowAnonymous]
    public async Task<IActionResult> GetEntries(Guid id)
    {
        var items = await _mediator.Send(new GetCompetitionEntriesQuery(id));
        return Ok(new { success = true, data = items });
    }

    [HttpGet("{id:guid}/leaderboard")]
    [AllowAnonymous]
    public async Task<IActionResult> GetLeaderboard(Guid id)
    {
        var competition = await _mediator.Send(new GetCompetitionDetailsQuery(id));
        if (competition == null) return NotFound(new { success = false, message = "Không tìm thấy cuộc thi." });

        var entries = await _mediator.Send(new GetCompetitionEntriesQuery(id));
        var rankedEntries = entries.OrderByDescending(e => e.AverageScore).ToList();

        return Ok(new
        {
            success = true,
            data = new
            {
                competition,
                podium = rankedEntries.Take(3).ToList(),
                allEntries = rankedEntries
            }
        });
    }

    [HttpPost("{id:guid}/submit-entry")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> SubmitEntry(Guid id, [FromBody] SubmitEntryApiRequest request)
    {
        var entryId = await _mediator.Send(new SubmitEntryCommand(id, request.PaintingId, request.StudentId));
        return Ok(new { success = true, message = "Nộp bài dự thi thành công!", data = new { entryId } });
    }

    [HttpPost("entries/{entryId:guid}/score")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Principal,Staff")]
    public async Task<IActionResult> ScoreEntry(Guid entryId, [FromBody] ScoreEntryApiRequest request)
    {
        var success = await _mediator.Send(new SubmitScoreCommand(entryId, request.JudgeId, request.CriteriaId, request.Score, request.Comment));
        return Ok(new { success, message = "Đã lưu điểm bài thi thành công!" });
    }

    public record SubmitEntryApiRequest(Guid PaintingId, Guid StudentId);
    public record ScoreEntryApiRequest(Guid JudgeId, Guid CriteriaId, decimal Score, string? Comment);
}

