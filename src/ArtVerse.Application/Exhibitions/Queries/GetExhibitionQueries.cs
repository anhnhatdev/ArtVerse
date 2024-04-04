using ArtVerse.Application.Common.Interfaces;
using ArtVerse.Application.Exhibitions.DTOs;
using ArtVerse.Domain.Entities;
using MediatR;

namespace ArtVerse.Application.Exhibitions.Queries;

public record GetExhibitionsQuery(string? Search = null, int Page = 1, int PageSize = 10)
    : IRequest<(IReadOnlyList<ExhibitionDto> Items, int TotalCount)>;

public class GetExhibitionsHandler : IRequestHandler<GetExhibitionsQuery, (IReadOnlyList<ExhibitionDto> Items, int TotalCount)>
{
    private readonly IExhibitionRepository _repo;

    public GetExhibitionsHandler(IExhibitionRepository repo) => _repo = repo;

    public async Task<(IReadOnlyList<ExhibitionDto> Items, int TotalCount)> Handle(GetExhibitionsQuery request, CancellationToken ct)
    {
        var (items, totalCount) = await _repo.GetPagedAsync(request.Search, request.Page, request.PageSize, ct);
        var dtos = items.Select(e => new ExhibitionDto(
            e.Id,
            e.Code,
            e.Title,
            e.Description,
            e.Venue,
            e.Address,
            e.StartDate,
            e.EndDate,
            e.ThumbnailUrl,
            e.TicketPrice,
            e.Status,
            e.Artworks.Count,
            e.CreatedAt
        )).ToList();

        return (dtos, totalCount);
    }
}

public record GetExhibitionShowcaseQuery(Guid ExhibitionId) : IRequest<Exhibition?>;

public class GetExhibitionShowcaseHandler : IRequestHandler<GetExhibitionShowcaseQuery, Exhibition?>
{
    private readonly IExhibitionRepository _repo;

    public GetExhibitionShowcaseHandler(IExhibitionRepository repo) => _repo = repo;

    public async Task<Exhibition?> Handle(GetExhibitionShowcaseQuery request, CancellationToken ct)
        => await _repo.GetWithArtworksAsync(request.ExhibitionId, ct);
}
