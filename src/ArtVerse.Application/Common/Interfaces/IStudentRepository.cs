using ArtVerse.Domain.Entities;

namespace ArtVerse.Application.Common.Interfaces;

public interface IStudentRepository
{
    Task<Student?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Student?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<Student?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<Student?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<(IReadOnlyList<Student> Items, int TotalCount)> GetPagedAsync(string? searchTerm, int page, int pageSize, CancellationToken ct = default);
    Task AddAsync(Student student, CancellationToken ct = default);
    void Update(Student student);
    void Delete(Student student);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
