using ArtVerse.Domain.Entities;

namespace ArtVerse.Application.Common.Interfaces;

public interface IAcademicRepository
{
    Task<IReadOnlyList<Class>> GetClassesAsync(CancellationToken ct = default);
    Task<Class?> GetClassByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Subject>> GetSubjectsAsync(CancellationToken ct = default);
    Task<Subject?> GetSubjectByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<StudentEnrollment>> GetEnrollmentsByClassAsync(Guid classId, CancellationToken ct = default);
    Task AddClassAsync(Class classEntity, CancellationToken ct = default);
    Task AddSubjectAsync(Subject subject, CancellationToken ct = default);
    Task AddEnrollmentAsync(StudentEnrollment enrollment, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
