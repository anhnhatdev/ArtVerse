using ArtVerse.Application.Common.Interfaces;
using ArtVerse.Application.Students.DTOs;
using MediatR;

namespace ArtVerse.Application.Students.Queries;

public record GetStudentsQuery(string? SearchTerm = null, int Page = 1, int PageSize = 10) 
    : IRequest<(IReadOnlyList<StudentDto> Items, int TotalCount)>;

public class GetStudentsHandler : IRequestHandler<GetStudentsQuery, (IReadOnlyList<StudentDto> Items, int TotalCount)>
{
    private readonly IStudentRepository _repo;

    public GetStudentsHandler(IStudentRepository repo) => _repo = repo;

    public async Task<(IReadOnlyList<StudentDto> Items, int TotalCount)> Handle(GetStudentsQuery request, CancellationToken ct)
    {
        var (students, totalCount) = await _repo.GetPagedAsync(request.SearchTerm, request.Page, request.PageSize, ct);
        var dtos = students.Select(s => new StudentDto(
            s.Id,
            s.Code,
            s.FullName,
            s.Email,
            s.Phone,
            s.DateOfBirth,
            s.Gender,
            s.AvatarUrl,
            s.Paintings.Count,
            s.CreatedAt
        )).ToList();

        return (dtos, totalCount);
    }
}

public record GetStudentByIdQuery(Guid Id) : IRequest<StudentDto?>;

public class GetStudentByIdHandler : IRequestHandler<GetStudentByIdQuery, StudentDto?>
{
    private readonly IStudentRepository _repo;

    public GetStudentByIdHandler(IStudentRepository repo) => _repo = repo;

    public async Task<StudentDto?> Handle(GetStudentByIdQuery request, CancellationToken ct)
    {
        var s = await _repo.GetByIdAsync(request.Id, ct);
        if (s == null) return null;

        return new StudentDto(
            s.Id,
            s.Code,
            s.FullName,
            s.Email,
            s.Phone,
            s.DateOfBirth,
            s.Gender,
            s.AvatarUrl,
            s.Paintings.Count,
            s.CreatedAt
        );
    }
}
