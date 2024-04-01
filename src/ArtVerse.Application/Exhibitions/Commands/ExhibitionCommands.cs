using ArtVerse.Application.Common.Interfaces;
using ArtVerse.Domain.Entities;
using MediatR;

namespace ArtVerse.Application.Exhibitions.Commands;

public record CurateArtworkCommand(Guid ExhibitionId, Guid PaintingId, int DisplayOrder = 0) : IRequest<Guid>;

public class CurateArtworkHandler : IRequestHandler<CurateArtworkCommand, Guid>
{
    private readonly IExhibitionRepository _repo;
    private readonly IPaintingRepository _paintingRepo;

    public CurateArtworkHandler(IExhibitionRepository repo, IPaintingRepository paintingRepo)
    {
        _repo = repo;
        _paintingRepo = paintingRepo;
    }

    public async Task<Guid> Handle(CurateArtworkCommand request, CancellationToken ct)
    {
        var existing = await _repo.GetArtworkAsync(request.ExhibitionId, request.PaintingId, ct);
        if (existing != null) return existing.Id;

        var artwork = ExhibitionArtwork.Create(request.ExhibitionId, request.PaintingId, request.DisplayOrder);
        await _repo.AddArtworkAsync(artwork, ct);

        var painting = await _paintingRepo.GetByIdAsync(request.PaintingId, ct);
        if (painting != null)
        {
            painting.PutOnExhibit();
            _paintingRepo.Update(painting);
        }

        await _repo.SaveChangesAsync(ct);
        return artwork.Id;
    }
}

public record LikeArtworkCommand(Guid ExhibitionArtworkId) : IRequest<int>;

public class LikeArtworkHandler : IRequestHandler<LikeArtworkCommand, int>
{
    private readonly IExhibitionRepository _repo;

    public LikeArtworkHandler(IExhibitionRepository repo) => _repo = repo;

    public async Task<int> Handle(LikeArtworkCommand request, CancellationToken ct)
    {
        var artwork = await _repo.GetExhibitionArtworkByIdAsync(request.ExhibitionArtworkId, ct);
        if (artwork == null) return 0;

        artwork.IncrementLike();
        await _repo.SaveChangesAsync(ct);
        return artwork.LikeCount;
    }
}
