using ArtVerse.Domain.Entities;

namespace ArtVerse.Application.Common.Interfaces;

public interface IExhibitionRepository
{
    Task<Exhibition?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Exhibition?> GetWithArtworksAsync(Guid id, CancellationToken ct = default);
    Task<(IReadOnlyList<Exhibition> Items, int TotalCount)> GetPagedAsync(string? search, int page, int pageSize, CancellationToken ct = default);
    Task AddAsync(Exhibition exhibition, CancellationToken ct = default);
    Task AddArtworkAsync(ExhibitionArtwork artwork, CancellationToken ct = default);
    Task<ExhibitionArtwork?> GetArtworkAsync(Guid exhibitionId, Guid paintingId, CancellationToken ct = default);
    Task<ExhibitionArtwork?> GetExhibitionArtworkByIdAsync(Guid id, CancellationToken ct = default);
    void Update(Exhibition exhibition);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
