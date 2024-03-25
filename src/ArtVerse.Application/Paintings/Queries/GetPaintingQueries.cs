using ArtVerse.Application.Common.Interfaces;
using ArtVerse.Application.Paintings.DTOs;
using ArtVerse.Domain.Enums;
using MediatR;

namespace ArtVerse.Application.Paintings.Queries;

public record GetGalleryPaintingsQuery(string? Search, ArtTechnique? Technique, int Page = 1, int PageSize = 12)
    : IRequest<(IReadOnlyList<PaintingDto> Items, int TotalCount)>;

public class GetGalleryPaintingsHandler : IRequestHandler<GetGalleryPaintingsQuery, (IReadOnlyList<PaintingDto> Items, int TotalCount)>
{
    private readonly IPaintingRepository _repo;

    public GetGalleryPaintingsHandler(IPaintingRepository repo) => _repo = repo;

    public async Task<(IReadOnlyList<PaintingDto> Items, int TotalCount)> Handle(GetGalleryPaintingsQuery request, CancellationToken ct)
    {
        var (paintings, totalCount) = await _repo.GetGalleryPagedAsync(request.Search, request.Technique, request.Page, request.PageSize, ct);
        var dtos = paintings.Select(p => new PaintingDto(
            p.Id,
            p.Code,
            p.Title,
            p.Description,
            p.Technique,
            p.CreatedYear,
            p.Status,
            p.IsForSale,
            p.BasePrice,
            p.StudentId,
            p.Student?.FullName ?? "Nghệ sĩ ArtVerse",
            p.Files.FirstOrDefault(f => f.IsPrimary)?.FileUrl ?? p.Files.FirstOrDefault()?.FileUrl,
            p.CreatedAt
        )).ToList();

        return (dtos, totalCount);
    }
}

public record GetPaintingDetailsQuery(Guid Id) : IRequest<PaintingDetailDto?>;

public class GetPaintingDetailsHandler : IRequestHandler<GetPaintingDetailsQuery, PaintingDetailDto?>
{
    private readonly IPaintingRepository _repo;

    public GetPaintingDetailsHandler(IPaintingRepository repo) => _repo = repo;

    public async Task<PaintingDetailDto?> Handle(GetPaintingDetailsQuery request, CancellationToken ct)
    {
        var p = await _repo.GetWithFilesAndStudentAsync(request.Id, ct);
        if (p == null) return null;

        var imageUrls = p.Files.Select(f => f.FileUrl).ToList();
        return new PaintingDetailDto(
            p.Id,
            p.Code,
            p.Title,
            p.Description,
            p.Technique,
            p.CreatedYear,
            p.Status,
            p.IsForSale,
            p.BasePrice,
            p.StudentId,
            p.Student?.FullName ?? "Nghệ sĩ ArtVerse",
            p.Student?.Email ?? string.Empty,
            p.RejectionReason,
            imageUrls,
            p.CreatedAt
        );
    }
}

public record GetPendingReviewPaintingsQuery : IRequest<IReadOnlyList<PaintingDto>>;

public class GetPendingReviewPaintingsHandler : IRequestHandler<GetPendingReviewPaintingsQuery, IReadOnlyList<PaintingDto>>
{
    private readonly IPaintingRepository _repo;

    public GetPendingReviewPaintingsHandler(IPaintingRepository repo) => _repo = repo;

    public async Task<IReadOnlyList<PaintingDto>> Handle(GetPendingReviewPaintingsQuery request, CancellationToken ct)
    {
        var paintings = await _repo.GetPendingReviewAsync(ct);
        return paintings.Select(p => new PaintingDto(
            p.Id,
            p.Code,
            p.Title,
            p.Description,
            p.Technique,
            p.CreatedYear,
            p.Status,
            p.IsForSale,
            p.BasePrice,
            p.StudentId,
            p.Student?.FullName ?? "Chưa rõ",
            p.Files.FirstOrDefault(f => f.IsPrimary)?.FileUrl ?? p.Files.FirstOrDefault()?.FileUrl,
            p.CreatedAt
        )).ToList();
    }
}

public record GetStudentPaintingsQuery(Guid StudentId) : IRequest<IReadOnlyList<PaintingDto>>;

public class GetStudentPaintingsHandler : IRequestHandler<GetStudentPaintingsQuery, IReadOnlyList<PaintingDto>>
{
    private readonly IPaintingRepository _repo;

    public GetStudentPaintingsHandler(IPaintingRepository repo) => _repo = repo;

    public async Task<IReadOnlyList<PaintingDto>> Handle(GetStudentPaintingsQuery request, CancellationToken ct)
    {
        var paintings = await _repo.GetByStudentIdAsync(request.StudentId, ct);
        return paintings.Select(p => new PaintingDto(
            p.Id,
            p.Code,
            p.Title,
            p.Description,
            p.Technique,
            p.CreatedYear,
            p.Status,
            p.IsForSale,
            p.BasePrice,
            p.StudentId,
            p.Student?.FullName ?? "Chưa rõ",
            p.Files.FirstOrDefault(f => f.IsPrimary)?.FileUrl ?? p.Files.FirstOrDefault()?.FileUrl,
            p.CreatedAt
        )).ToList();
    }
}
