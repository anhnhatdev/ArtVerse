using ArtVerse.Application.Academic.DTOs;
using ArtVerse.Application.Common.Interfaces;
using MediatR;

namespace ArtVerse.Application.Academic.Queries;

public record GetClassesQuery : IRequest<IReadOnlyList<ClassDto>>;

public class GetClassesHandler : IRequestHandler<GetClassesQuery, IReadOnlyList<ClassDto>>
{
    private readonly IAcademicRepository _repo;

    public GetClassesHandler(IAcademicRepository repo) => _repo = repo;

    public async Task<IReadOnlyList<ClassDto>> Handle(GetClassesQuery request, CancellationToken ct)
    {
        var classes = await _repo.GetClassesAsync(ct);
        return classes.Select(c => new ClassDto(
            c.Id,
            c.Code,
            c.Name,
            c.Year,
            c.Semester,
            c.MaxStudents,
            c.Enrollments.Count,
            c.CreatedAt
        )).ToList();
    }
}

public record GetSubjectsQuery : IRequest<IReadOnlyList<SubjectDto>>;

public class GetSubjectsHandler : IRequestHandler<GetSubjectsQuery, IReadOnlyList<SubjectDto>>
{
    private readonly IAcademicRepository _repo;

    public GetSubjectsHandler(IAcademicRepository repo) => _repo = repo;

    public async Task<IReadOnlyList<SubjectDto>> Handle(GetSubjectsQuery request, CancellationToken ct)
    {
        var subjects = await _repo.GetSubjectsAsync(ct);
        return subjects.Select(s => new SubjectDto(
            s.Id,
            s.Code,
            s.Name,
            s.Description,
            s.CreditHours
        )).ToList();
    }
}

public record GetClassDetailsQuery(Guid ClassId) : IRequest<(ClassDto? ClassInfo, IReadOnlyList<EnrollmentDto> Students)>;

public class GetClassDetailsHandler : IRequestHandler<GetClassDetailsQuery, (ClassDto? ClassInfo, IReadOnlyList<EnrollmentDto> Students)>
{
    private readonly IAcademicRepository _repo;

    public GetClassDetailsHandler(IAcademicRepository repo) => _repo = repo;

    public async Task<(ClassDto? ClassInfo, IReadOnlyList<EnrollmentDto> Students)> Handle(GetClassDetailsQuery request, CancellationToken ct)
    {
        var c = await _repo.GetClassByIdAsync(request.ClassId, ct);
        if (c == null) return (null, new List<EnrollmentDto>());

        var classDto = new ClassDto(c.Id, c.Code, c.Name, c.Year, c.Semester, c.MaxStudents, c.Enrollments.Count, c.CreatedAt);
        var enrollments = c.Enrollments.Select(e => new EnrollmentDto(
            e.Id,
            e.StudentId,
            e.Student?.FullName ?? "Học viên",
            e.Student?.Code ?? string.Empty,
            e.Status,
            e.EnrolledAt
        )).ToList();

        return (classDto, enrollments);
    }
}
