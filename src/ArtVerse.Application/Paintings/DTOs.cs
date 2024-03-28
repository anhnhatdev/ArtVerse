using ArtVerse.Domain.Enums;

namespace ArtVerse.Application.Paintings.DTOs;

public record PaintingDto(
    Guid Id,
    string Code,
    string Title,
    string? Description,
    ArtTechnique Technique,
    int? CreatedYear,
    PaintingStatus Status,
    bool IsForSale,
    decimal? BasePrice,
    Guid StudentId,
    string StudentName,
    string? PrimaryImageUrl,
    DateTimeOffset CreatedAt
);

public record PaintingDetailDto(
    Guid Id,
    string Code,
    string Title,
    string? Description,
    ArtTechnique Technique,
    int? CreatedYear,
    PaintingStatus Status,
    bool IsForSale,
    decimal? BasePrice,
    Guid StudentId,
    string StudentName,
    string StudentEmail,
    string? RejectionReason,
    IReadOnlyList<string> ImageUrls,
    DateTimeOffset CreatedAt
);

public record CreatePaintingDto(
    string Title,
    string? Description,
    ArtTechnique Technique,
    int? CreatedYear,
    bool IsForSale,
    decimal? BasePrice,
    Guid StudentId,
    string? ImageFileName
);
