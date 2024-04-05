using ArtVerse.Application.Common.Interfaces;
using ArtVerse.Domain.Entities;
using ArtVerse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ArtVerse.Infrastructure.Repositories;

public class AcademicRepository : IAcademicRepository
{
    private readonly ApplicationDbContext _context;

    public AcademicRepository(ApplicationDbContext context) => _context = context;

    public async Task<IReadOnlyList<Class>> GetClassesAsync(CancellationToken ct = default)
    {
        var items = await _context.Classes
            .Include(c => c.Enrollments)
            .ToListAsync(ct);
        return items.OrderByDescending(c => c.CreatedAt).ToList();
    }

    public async Task<Class?> GetClassByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Classes
            .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<IReadOnlyList<Subject>> GetSubjectsAsync(CancellationToken ct = default)
        => await _context.Subjects
            .OrderBy(s => s.Code)
            .ToListAsync(ct);

    public async Task<Subject?> GetSubjectByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Subjects
            .FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<IReadOnlyList<StudentEnrollment>> GetEnrollmentsByClassAsync(Guid classId, CancellationToken ct = default)
        => await _context.StudentEnrollments
            .Include(e => e.Student)
            .Where(e => e.ClassId == classId)
            .ToListAsync(ct);

    public async Task AddClassAsync(Class classEntity, CancellationToken ct = default)
        => await _context.Classes.AddAsync(classEntity, ct);

    public async Task AddSubjectAsync(Subject subject, CancellationToken ct = default)
        => await _context.Subjects.AddAsync(subject, ct);

    public async Task AddEnrollmentAsync(StudentEnrollment enrollment, CancellationToken ct = default)
        => await _context.StudentEnrollments.AddAsync(enrollment, ct);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
}
