using ArtVerse.Application.Common.Interfaces;
using ArtVerse.Application.Students.DTOs;
using ArtVerse.Domain.Entities;
using MediatR;

namespace ArtVerse.Application.Students.Commands;

public record CreateStudentCommand(CreateStudentDto Dto) : IRequest<Guid>;

public class CreateStudentHandler : IRequestHandler<CreateStudentCommand, Guid>
{
    private readonly IStudentRepository _repo;

    public CreateStudentHandler(IStudentRepository repo) => _repo = repo;

    public async Task<Guid> Handle(CreateStudentCommand request, CancellationToken ct)
    {
        // Tự động sinh mã học viên nếu chưa có
        var studentCount = (await _repo.GetPagedAsync(null, 1, 1, ct)).TotalCount + 1;
        var code = $"SV-{DateTime.Now.Year}-{studentCount:D3}";

        var student = Student.Create(request.Dto.FullName, request.Dto.Email, code);
        student.Update(request.Dto.FullName, request.Dto.Phone, request.Dto.DateOfBirth, request.Dto.Gender);

        await _repo.AddAsync(student, ct);
        await _repo.SaveChangesAsync(ct);

        return student.Id;
    }
}

public record UpdateStudentCommand(UpdateStudentDto Dto) : IRequest<bool>;

public class UpdateStudentHandler : IRequestHandler<UpdateStudentCommand, bool>
{
    private readonly IStudentRepository _repo;

    public UpdateStudentHandler(IStudentRepository repo) => _repo = repo;

    public async Task<bool> Handle(UpdateStudentCommand request, CancellationToken ct)
    {
        var student = await _repo.GetByIdAsync(request.Dto.Id, ct);
        if (student == null) return false;

        student.Update(request.Dto.FullName, request.Dto.Phone, request.Dto.DateOfBirth, request.Dto.Gender);
        _repo.Update(student);
        await _repo.SaveChangesAsync(ct);

        return true;
    }
}

public record DeleteStudentCommand(Guid Id) : IRequest<bool>;

public class DeleteStudentHandler : IRequestHandler<DeleteStudentCommand, bool>
{
    private readonly IStudentRepository _repo;

    public DeleteStudentHandler(IStudentRepository repo) => _repo = repo;

    public async Task<bool> Handle(DeleteStudentCommand request, CancellationToken ct)
    {
        var student = await _repo.GetByIdAsync(request.Id, ct);
        if (student == null) return false;

        _repo.Delete(student);
        await _repo.SaveChangesAsync(ct);

        return true;
    }
}
