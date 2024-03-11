using ArtVerse.Domain.Entities;
using ArtVerse.Domain.Enums;

namespace ArtVerse.Application.Common.Interfaces;

public interface IPaintingRepository
{
    Task<Painting?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Painting?> GetWithFilesAndStudentAsync(Guid id, CancellationToken ct = default);
    Task<(IReadOnlyList<Painting> Items, int TotalCount)> GetGalleryPagedAsync(
        string? search, ArtTechnique? technique, int page, int pageSize, CancellationToken ct = default);
    Task<IReadOnlyList<Painting>> GetPendingReviewAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Painting>> GetByStudentIdAsync(Guid studentId, CancellationToken ct = default);
    Task AddAsync(Painting painting, CancellationToken ct = default);
    Task AddFileAsync(PaintingFile file, CancellationToken ct = default);
    void Update(Painting painting);
    void Delete(Painting painting);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
