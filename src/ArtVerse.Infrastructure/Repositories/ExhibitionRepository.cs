using ArtVerse.Application.Common.Interfaces;
using ArtVerse.Domain.Entities;
using ArtVerse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ArtVerse.Infrastructure.Repositories;

public class ExhibitionRepository : IExhibitionRepository
{
    private readonly ApplicationDbContext _context;

    public ExhibitionRepository(ApplicationDbContext context) => _context = context;

    public async Task<Exhibition?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Exhibitions
            .Include(e => e.Artworks)
                .ThenInclude(ea => ea.Painting)
                    .ThenInclude(p => p.Student)
            .FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task<Exhibition?> GetWithArtworksAsync(Guid id, CancellationToken ct = default)
        => await _context.Exhibitions
            .Include(e => e.Artworks.OrderBy(a => a.DisplayOrder))
                .ThenInclude(ea => ea.Painting)
                    .ThenInclude(p => p.Student)
            .Include(e => e.Artworks)
                .ThenInclude(ea => ea.Painting)
                    .ThenInclude(p => p.Files)
            .FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task<(IReadOnlyList<Exhibition> Items, int TotalCount)> GetPagedAsync(
        string? search, int page, int pageSize, CancellationToken ct = default)
    {
        var query = _context.Exhibitions
            .Include(e => e.Artworks)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(e => e.Title.ToLower().Contains(term) ||
                                     (e.Description != null && e.Description.ToLower().Contains(term)) ||
                                     e.Code.ToLower().Contains(term));
        }

        var allItems = await query.ToListAsync(ct);
        var totalCount = allItems.Count;
        var items = allItems
            .OrderByDescending(e => e.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (items, totalCount);
    }

    public async Task AddAsync(Exhibition exhibition, CancellationToken ct = default)
        => await _context.Exhibitions.AddAsync(exhibition, ct);

    public async Task AddArtworkAsync(ExhibitionArtwork artwork, CancellationToken ct = default)
        => await _context.ExhibitionArtworks.AddAsync(artwork, ct);

    public async Task<ExhibitionArtwork?> GetArtworkAsync(Guid exhibitionId, Guid paintingId, CancellationToken ct = default)
        => await _context.ExhibitionArtworks
            .FirstOrDefaultAsync(ea => ea.ExhibitionId == exhibitionId && ea.PaintingId == paintingId, ct);

    public async Task<ExhibitionArtwork?> GetExhibitionArtworkByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.ExhibitionArtworks
            .FirstOrDefaultAsync(ea => ea.Id == id, ct);

    public void Update(Exhibition exhibition) => _context.Exhibitions.Update(exhibition);
    public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await _context.SaveChangesAsync(ct);
}
