using ArtVerse.Application.Common.Interfaces;
using ArtVerse.Application.Paintings.DTOs;
using ArtVerse.Domain.Entities;
using MediatR;

namespace ArtVerse.Application.Paintings.Commands;

public record CreatePaintingCommand(CreatePaintingDto Dto, string? SavedFileUrl, long FileSize = 0) : IRequest<Guid>;

public class CreatePaintingHandler : IRequestHandler<CreatePaintingCommand, Guid>
{
    private readonly IPaintingRepository _repo;

    public CreatePaintingHandler(IPaintingRepository repo) => _repo = repo;

    public async Task<Guid> Handle(CreatePaintingCommand request, CancellationToken ct)
    {
        var count = (await _repo.GetGalleryPagedAsync(null, null, 1, 1, ct)).TotalCount + 1;
        var code = $"AV-{DateTime.Now.Year}-{count:D4}";

        var painting = Painting.Create(
            request.Dto.Title,
            code,
            request.Dto.StudentId,
            request.Dto.Technique,
            request.Dto.Description
        );

        painting.Update(
            request.Dto.Title,
            request.Dto.Description,
            request.Dto.Technique,
            request.Dto.CreatedYear,
            request.Dto.IsForSale,
            request.Dto.BasePrice
        );

        await _repo.AddAsync(painting, ct);

        if (!string.IsNullOrWhiteSpace(request.SavedFileUrl))
        {
            var pFile = PaintingFile.Create(
                painting.Id,
                request.SavedFileUrl,
                request.Dto.ImageFileName ?? "artwork.jpg",
                request.FileSize,
                isPrimary: true
            );
            await _repo.AddFileAsync(pFile, ct);
        }

        await _repo.SaveChangesAsync(ct);
        return painting.Id;
    }
}

public record SubmitPaintingCommand(Guid Id) : IRequest<bool>;

public class SubmitPaintingHandler : IRequestHandler<SubmitPaintingCommand, bool>
{
    private readonly IPaintingRepository _repo;

    public SubmitPaintingHandler(IPaintingRepository repo) => _repo = repo;

    public async Task<bool> Handle(SubmitPaintingCommand request, CancellationToken ct)
    {
        var painting = await _repo.GetByIdAsync(request.Id, ct);
        if (painting == null) return false;

        painting.Submit();
        _repo.Update(painting);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}

public record ApprovePaintingCommand(Guid Id) : IRequest<bool>;

public class ApprovePaintingHandler : IRequestHandler<ApprovePaintingCommand, bool>
{
    private readonly IPaintingRepository _repo;

    public ApprovePaintingHandler(IPaintingRepository repo) => _repo = repo;

    public async Task<bool> Handle(ApprovePaintingCommand request, CancellationToken ct)
    {
        var painting = await _repo.GetByIdAsync(request.Id, ct);
        if (painting == null) return false;

        painting.Approve();
        _repo.Update(painting);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}

public record RejectPaintingCommand(Guid Id, string Reason) : IRequest<bool>;

public class RejectPaintingHandler : IRequestHandler<RejectPaintingCommand, bool>
{
    private readonly IPaintingRepository _repo;

    public RejectPaintingHandler(IPaintingRepository repo) => _repo = repo;

    public async Task<bool> Handle(RejectPaintingCommand request, CancellationToken ct)
    {
        var painting = await _repo.GetByIdAsync(request.Id, ct);
        if (painting == null) return false;

        painting.Reject(request.Reason);
        _repo.Update(painting);
        await _repo.SaveChangesAsync(ct);
        return true;
    }
}
