using ArtVerse.Domain.Common;

namespace ArtVerse.Domain.Entities;

/// <summary>
/// Tác phẩm được trưng bày trong một triển lãm cụ thể.
/// </summary>
public class ExhibitionArtwork : BaseEntity
{
    public Guid ExhibitionId { get; private set; }
    public Exhibition? Exhibition { get; private set; }
    public Guid PaintingId { get; private set; }
    public Painting? Painting { get; private set; }
    public int DisplayOrder { get; private set; } = 0;
    public string? SectionName { get; private set; }   // Khu vực trưng bày
    public bool IsForSale { get; private set; } = false;
    public decimal? AskingPrice { get; private set; }
    public bool IsFeatured { get; private set; } = false;
    public int ViewCount { get; private set; } = 0;
    public int LikeCount { get; private set; } = 0;

    private ExhibitionArtwork() { }

    public static ExhibitionArtwork Create(Guid exhibitionId, Guid paintingId, int displayOrder = 0)
        => new() { ExhibitionId = exhibitionId, PaintingId = paintingId, DisplayOrder = displayOrder };

    public void IncrementView() => ViewCount++;
    public void IncrementLike() => LikeCount++;
}
