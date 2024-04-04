using ArtVerse.Domain.Enums;

namespace ArtVerse.Application.Exhibitions.DTOs;

public record ExhibitionDto(
    Guid Id,
    string Code,
    string Title,
    string? Description,
    string? Venue,
    string? Address,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    string? ThumbnailUrl,
    decimal? TicketPrice,
    ExhibitionStatus Status,
    int ArtworkCount,
    DateTimeOffset CreatedAt
);

public record ExhibitionArtworkDto(
    Guid Id,
    Guid ExhibitionId,
    Guid PaintingId,
    string PaintingTitle,
    string? PaintingDescription,
    string Technique,
    string? ImageUrl,
    Guid StudentId,
    string StudentName,
    int DisplayOrder,
    string? SectionName,
    bool IsForSale,
    decimal? AskingPrice,
    bool IsFeatured,
    int ViewCount,
    int LikeCount
);
