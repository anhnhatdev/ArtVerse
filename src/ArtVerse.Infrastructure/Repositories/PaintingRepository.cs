using ArtVerse.Application.Common.Interfaces;
using ArtVerse.Domain.Entities;
using ArtVerse.Domain.Enums;
using ArtVerse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ArtVerse.Infrastructure.Repositories;

public class PaintingRepository : IPaintingRepository
{
    private readonly ApplicationDbContext _context;

    public PaintingRepository(ApplicationDbContext context) => _context = context;

    public async Task<Painting?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Paintings
            .Include(p => p.Files)
            .Include(p => p.Student)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Painting?> GetWithFilesAndStudentAsync(Guid id, CancellationToken ct = default)
        => await _context.Paintings
            .Include(p => p.Files)
            .Include(p => p.Student)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<(IReadOnlyList<Painting> Items, int TotalCount)> GetGalleryPagedAsync(
        string? search, ArtTechnique? technique, int page, int pageSize, CancellationToken ct = default)
    {
        var query = _context.Paintings
            .Include(p => p.Files)
            .Include(p => p.Student)
            .Where(p => p.Status == PaintingStatus.Approved || p.Status == PaintingStatus.OnExhibit || p.Status == PaintingStatus.Sold)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(p => p.Title.ToLower().Contains(term) ||
                                     (p.Description != null && p.Description.ToLower().Contains(term)) ||
                                     p.Code.ToLower().Contains(term) ||
                                     (p.Student != null && p.Student.FullName.ToLower().Contains(term)));
        }

        if (technique.HasValue)
        {
            query = query.Where(p => p.Technique == technique.Value);
        }

        var allItems = await query.ToListAsync(ct);
        var totalCount = allItems.Count;
        var items = allItems
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (items, totalCount);
    }

    public async Task<IReadOnlyList<Painting>> GetPendingReviewAsync(CancellationToken ct = default)
    {
        var items = await _context.Paintings
            .Include(p => p.Files)
            .Include(p => p.Student)
            .Where(p => p.Status == PaintingStatus.Submitted)
            .AsNoTracking()
            .ToListAsync(ct);
        return items.OrderByDescending(p => p.CreatedAt).ToList();
    }

    public async Task<IReadOnlyList<Painting>> GetByStudentIdAsync(Guid studentId, CancellationToken ct = default)
    {
        var items = await _context.Paintings
            .Include(p => p.Files)
            .Where(p => p.StudentId == studentId)
            .AsNoTracking()
            .ToListAsync(ct);
        return items.OrderByDescending(p => p.CreatedAt).ToList();
    }

    public async Task AddAsync(Painting painting, CancellationToken ct = default)
        => await _context.Paintings.AddAsync(painting, ct);

    public async Task AddFileAsync(PaintingFile file, CancellationToken ct = default)
        => await _context.PaintingFiles.AddAsync(file, ct);

    public void Update(Painting painting) => _context.Paintings.Update(painting);
    public void Delete(Painting painting) { painting.MarkAsDeleted(); _context.Paintings.Update(painting); }
    public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await _context.SaveChangesAsync(ct);
}
