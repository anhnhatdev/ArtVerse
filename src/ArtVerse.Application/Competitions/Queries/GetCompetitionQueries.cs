using ArtVerse.Application.Common.Interfaces;
using ArtVerse.Application.Competitions.DTOs;
using ArtVerse.Domain.Entities;
using MediatR;

namespace ArtVerse.Application.Competitions.Queries;

public record GetCompetitionsQuery(string? Search = null, int Page = 1, int PageSize = 10)
    : IRequest<(IReadOnlyList<CompetitionDto> Items, int TotalCount)>;

public class GetCompetitionsHandler : IRequestHandler<GetCompetitionsQuery, (IReadOnlyList<CompetitionDto> Items, int TotalCount)>
{
    private readonly ICompetitionRepository _repo;

    public GetCompetitionsHandler(ICompetitionRepository repo) => _repo = repo;

    public async Task<(IReadOnlyList<CompetitionDto> Items, int TotalCount)> Handle(GetCompetitionsQuery request, CancellationToken ct)
    {
        var (items, totalCount) = await _repo.GetPagedAsync(request.Search, request.Page, request.PageSize, ct);
        var dtos = items.Select(c => new CompetitionDto(
            c.Id,
            c.Code,
            c.Title,
            c.Theme,
            c.Description,
            c.Status,
            c.RegistrationStart,
            c.RegistrationEnd,
            c.SubmissionStart,
            c.SubmissionEnd,
            c.Entries.Count,
            c.CreatedAt
        )).ToList();

        return (dtos, totalCount);
    }
}

public record GetCompetitionDetailsQuery(Guid Id) : IRequest<Competition?>;

public class GetCompetitionDetailsHandler : IRequestHandler<GetCompetitionDetailsQuery, Competition?>
{
    private readonly ICompetitionRepository _repo;

    public GetCompetitionDetailsHandler(ICompetitionRepository repo) => _repo = repo;

    public async Task<Competition?> Handle(GetCompetitionDetailsQuery request, CancellationToken ct)
        => await _repo.GetWithCriteriaAndEntriesAsync(request.Id, ct);
}

public record GetCompetitionEntriesQuery(Guid CompetitionId) : IRequest<IReadOnlyList<CompetitionEntryDto>>;

public class GetCompetitionEntriesHandler : IRequestHandler<GetCompetitionEntriesQuery, IReadOnlyList<CompetitionEntryDto>>
{
    private readonly ICompetitionRepository _repo;

    public GetCompetitionEntriesHandler(ICompetitionRepository repo) => _repo = repo;

    public async Task<IReadOnlyList<CompetitionEntryDto>> Handle(GetCompetitionEntriesQuery request, CancellationToken ct)
    {
        var entries = await _repo.GetEntriesByCompetitionAsync(request.CompetitionId, ct);
        return entries.Select(e => new CompetitionEntryDto(
            e.Id,
            e.CompetitionId,
            e.PaintingId,
            e.Painting?.Title ?? "Tác phẩm dự thi",
            e.Painting?.Files.FirstOrDefault()?.FileUrl,
            e.Painting?.Technique.ToString() ?? "Nghệ thuật",
            e.StudentId,
            e.Student?.FullName ?? "Thí sinh",
            e.EntryCode,
            e.Status,
            e.CalculateAverageScore(),
            e.SubmittedAt
        )).ToList();
    }
}
