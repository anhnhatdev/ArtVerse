using ArtVerse.Application.Admin.DTOs;

namespace ArtVerse.Application.Common.Interfaces;

public interface IAnalyticsRepository
{
    Task<AnalyticsDataDto> GetDashboardStatsAsync(CancellationToken ct = default);
}
