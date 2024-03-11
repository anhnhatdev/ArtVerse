using ArtVerse.Domain.Entities;
using ArtVerse.Domain.Enums;

namespace ArtVerse.Application.Common.Interfaces;

public interface ICompetitionRepository
{
    Task<Competition?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Competition?> GetWithCriteriaAndEntriesAsync(Guid id, CancellationToken ct = default);
    Task<(IReadOnlyList<Competition> Items, int TotalCount)> GetPagedAsync(string? search, int page, int pageSize, CancellationToken ct = default);
    Task<CompetitionEntry?> GetEntryByIdAsync(Guid entryId, CancellationToken ct = default);
    Task<IReadOnlyList<CompetitionEntry>> GetEntriesByCompetitionAsync(Guid competitionId, CancellationToken ct = default);
    Task AddAsync(Competition competition, CancellationToken ct = default);
    Task AddEntryAsync(CompetitionEntry entry, CancellationToken ct = default);
    Task AddScoreAsync(EntryScore score, CancellationToken ct = default);
    Task AddAwardAsync(Award award, CancellationToken ct = default);
    void Update(Competition competition);
    void UpdateEntry(CompetitionEntry entry);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
