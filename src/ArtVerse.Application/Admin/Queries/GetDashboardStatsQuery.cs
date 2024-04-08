using ArtVerse.Application.Admin.DTOs;
using ArtVerse.Application.Common.Interfaces;
using MediatR;

namespace ArtVerse.Application.Admin.Queries;

public record GetDashboardStatsQuery : IRequest<AnalyticsDataDto>;

public class GetDashboardStatsHandler : IRequestHandler<GetDashboardStatsQuery, AnalyticsDataDto>
{
    private readonly IAnalyticsRepository _repo;

    public GetDashboardStatsHandler(IAnalyticsRepository repo) => _repo = repo;

    public async Task<AnalyticsDataDto> Handle(GetDashboardStatsQuery request, CancellationToken ct)
        => await _repo.GetDashboardStatsAsync(ct);
}
