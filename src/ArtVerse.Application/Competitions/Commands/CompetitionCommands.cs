using ArtVerse.Application.Common.Interfaces;
using ArtVerse.Domain.Entities;
using MediatR;

namespace ArtVerse.Application.Competitions.Commands;

public record SubmitEntryCommand(Guid CompetitionId, Guid PaintingId, Guid StudentId) : IRequest<Guid>;

public class SubmitEntryHandler : IRequestHandler<SubmitEntryCommand, Guid>
{
    private readonly ICompetitionRepository _repo;

    public SubmitEntryHandler(ICompetitionRepository repo) => _repo = repo;

    public async Task<Guid> Handle(SubmitEntryCommand request, CancellationToken ct)
    {
        var competition = await _repo.GetByIdAsync(request.CompetitionId, ct);
        if (competition == null) throw new InvalidOperationException("Cuộc thi không tồn tại.");

        var existingEntries = await _repo.GetEntriesByCompetitionAsync(request.CompetitionId, ct);
        var entryNumber = existingEntries.Count + 1;
        var entryCode = $"{competition.Code}-E{entryNumber:D3}";

        var entry = CompetitionEntry.Create(request.CompetitionId, request.PaintingId, request.StudentId, entryCode);
        entry.Accept(); // Auto accept into scoring round

        await _repo.AddEntryAsync(entry, ct);
        await _repo.SaveChangesAsync(ct);

        return entry.Id;
    }
}

public record SubmitScoreCommand(Guid EntryId, Guid JudgeId, Guid CriteriaId, decimal Score, string? Comment) : IRequest<bool>;

public class SubmitScoreHandler : IRequestHandler<SubmitScoreCommand, bool>
{
    private readonly ICompetitionRepository _repo;

    public SubmitScoreHandler(ICompetitionRepository repo) => _repo = repo;

    public async Task<bool> Handle(SubmitScoreCommand request, CancellationToken ct)
    {
        var entry = await _repo.GetEntryByIdAsync(request.EntryId, ct);
        if (entry == null) return false;

        var criteria = entry.Competition?.Criteria.FirstOrDefault(c => c.Id == request.CriteriaId);
        var weight = criteria?.Weight ?? 0.33m;

        var score = EntryScore.Create(request.EntryId, request.JudgeId, request.CriteriaId, request.Score, weight, request.Comment);
        await _repo.AddScoreAsync(score, ct);
        await _repo.SaveChangesAsync(ct);

        return true;
    }
}
