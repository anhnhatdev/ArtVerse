using ArtVerse.Application.Common.Interfaces;
using ArtVerse.Domain.Entities;
using MediatR;

namespace ArtVerse.Application.Academic.Commands;

public record CreateClassCommand(string Code, string Name, int Year, int Semester, int MaxStudents) : IRequest<Guid>;

public class CreateClassHandler : IRequestHandler<CreateClassCommand, Guid>
{
    private readonly IAcademicRepository _repo;

    public CreateClassHandler(IAcademicRepository repo) => _repo = repo;

    public async Task<Guid> Handle(CreateClassCommand request, CancellationToken ct)
    {
        var newClass = new Class
        {
            Code = request.Code.Trim().ToUpper(),
            Name = request.Name.Trim(),
            Year = request.Year,
            Semester = request.Semester,
            MaxStudents = request.MaxStudents
        };

        await _repo.AddClassAsync(newClass, ct);
        await _repo.SaveChangesAsync(ct);
        return newClass.Id;
    }
}

public record CreateSubjectCommand(string Code, string Name, string? Description, int CreditHours) : IRequest<Guid>;

public class CreateSubjectHandler : IRequestHandler<CreateSubjectCommand, Guid>
{
    private readonly IAcademicRepository _repo;

    public CreateSubjectHandler(IAcademicRepository repo) => _repo = repo;

    public async Task<Guid> Handle(CreateSubjectCommand request, CancellationToken ct)
    {
        var subject = new Subject
        {
            Code = request.Code.Trim().ToUpper(),
            Name = request.Name.Trim(),
            Description = request.Description,
            CreditHours = request.CreditHours
        };

        await _repo.AddSubjectAsync(subject, ct);
        await _repo.SaveChangesAsync(ct);
        return subject.Id;
    }
}

public record EnrollStudentCommand(Guid ClassId, Guid StudentId) : IRequest<Guid>;

public class EnrollStudentHandler : IRequestHandler<EnrollStudentCommand, Guid>
{
    private readonly IAcademicRepository _repo;

    public EnrollStudentHandler(IAcademicRepository repo) => _repo = repo;

    public async Task<Guid> Handle(EnrollStudentCommand request, CancellationToken ct)
    {
        var enrollment = new StudentEnrollment
        {
            ClassId = request.ClassId,
            StudentId = request.StudentId,
            Status = "Active"
        };

        await _repo.AddEnrollmentAsync(enrollment, ct);
        await _repo.SaveChangesAsync(ct);
        return enrollment.Id;
    }
}
