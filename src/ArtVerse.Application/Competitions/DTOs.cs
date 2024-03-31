using ArtVerse.Domain.Enums;

namespace ArtVerse.Application.Competitions.DTOs;

public record CompetitionDto(
    Guid Id,
    string Code,
    string Title,
    string? Theme,
    string? Description,
    CompetitionStatus Status,
    DateTimeOffset RegistrationStart,
    DateTimeOffset RegistrationEnd,
    DateTimeOffset SubmissionStart,
    DateTimeOffset SubmissionEnd,
    int EntryCount,
    DateTimeOffset CreatedAt
);

public record ScoringCriteriaDto(
    Guid Id,
    string Name,
    string? Description,
    decimal MaxScore,
    decimal Weight
);

public record CompetitionEntryDto(
    Guid Id,
    Guid CompetitionId,
    Guid PaintingId,
    string PaintingTitle,
    string? PaintingImageUrl,
    string Technique,
    Guid StudentId,
    string StudentName,
    string EntryCode,
    EntryStatus Status,
    decimal AverageScore,
    DateTimeOffset SubmittedAt
);

public record SubmitScoreDto(
    Guid EntryId,
    Guid JudgeId,
    Guid CriteriaId,
    decimal Score,
    string? Comment
);

public record AwardDto(
    Guid Id,
    Guid EntryId,
    string PaintingTitle,
    string StudentName,
    string? ImageUrl,
    string Title,
    int Rank,
    decimal? PrizeAmount
);
