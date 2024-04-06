namespace ArtVerse.Application.Admin.DTOs;

public record DashboardStatsDto(
    int TotalStudents,
    int TotalStaff,
    int TotalArtworks,
    int PendingReviews,
    int TotalCompetitions,
    int TotalExhibitions,
    decimal TotalArtworkValue
);

public record AnalyticsDataDto(
    DashboardStatsDto Stats,
    IReadOnlyList<string> TechniqueLabels,
    IReadOnlyList<int> TechniqueCounts,
    IReadOnlyList<string> MonthlyLabels,
    IReadOnlyList<int> MonthlyCounts
);
