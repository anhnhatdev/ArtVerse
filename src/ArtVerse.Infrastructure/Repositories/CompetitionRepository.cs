using ArtVerse.Application.Common.Interfaces;
using ArtVerse.Domain.Entities;
using ArtVerse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ArtVerse.Infrastructure.Repositories;

public class CompetitionRepository : ICompetitionRepository
{
    private readonly ApplicationDbContext _context;

    public CompetitionRepository(ApplicationDbContext context) => _context = context;

    public async Task<Competition?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Competitions
            .Include(c => c.Criteria)
            .Include(c => c.Entries)
                .ThenInclude(e => e.Painting)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<Competition?> GetWithCriteriaAndEntriesAsync(Guid id, CancellationToken ct = default)
        => await _context.Competitions
            .Include(c => c.Criteria)
            .Include(c => c.Entries)
                .ThenInclude(e => e.Painting)
                    .ThenInclude(p => p.Student)
            .Include(c => c.Entries)
                .ThenInclude(e => e.Scores)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<(IReadOnlyList<Competition> Items, int TotalCount)> GetPagedAsync(
        string? search, int page, int pageSize, CancellationToken ct = default)
    {
        var query = _context.Competitions
            .Include(c => c.Entries)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(c => c.Title.ToLower().Contains(term) ||
                                     (c.Theme != null && c.Theme.ToLower().Contains(term)) ||
                                     c.Code.ToLower().Contains(term));
        }

        var allItems = await query.ToListAsync(ct);
        var totalCount = allItems.Count;
        var items = allItems
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (items, totalCount);
    }

    public async Task<CompetitionEntry?> GetEntryByIdAsync(Guid entryId, CancellationToken ct = default)
        => await _context.CompetitionEntries
            .Include(e => e.Competition)
                .ThenInclude(c => c.Criteria)
            .Include(e => e.Painting)
                .ThenInclude(p => p.Student)
            .Include(e => e.Painting)
                .ThenInclude(p => p.Files)
            .Include(e => e.Scores)
                .ThenInclude(s => s.Judge)
            .FirstOrDefaultAsync(e => e.Id == entryId, ct);

    public async Task<IReadOnlyList<CompetitionEntry>> GetEntriesByCompetitionAsync(Guid competitionId, CancellationToken ct = default)
        => await _context.CompetitionEntries
            .Include(e => e.Painting)
                .ThenInclude(p => p.Files)
            .Include(e => e.Student)
            .Include(e => e.Scores)
            .Where(e => e.CompetitionId == competitionId)
            .ToListAsync(ct);

    public async Task AddAsync(Competition competition, CancellationToken ct = default)
        => await _context.Competitions.AddAsync(competition, ct);

    public async Task AddEntryAsync(CompetitionEntry entry, CancellationToken ct = default)
        => await _context.CompetitionEntries.AddAsync(entry, ct);

    public async Task AddScoreAsync(EntryScore score, CancellationToken ct = default)
        => await _context.EntryScores.AddAsync(score, ct);

    public async Task AddAwardAsync(Award award, CancellationToken ct = default)
        => await _context.Awards.AddAsync(award, ct);

    public void Update(Competition competition) => _context.Competitions.Update(competition);
    public void UpdateEntry(CompetitionEntry entry) => _context.CompetitionEntries.Update(entry);
    public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await _context.SaveChangesAsync(ct);
}
