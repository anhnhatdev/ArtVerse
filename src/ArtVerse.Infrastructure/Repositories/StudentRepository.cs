using ArtVerse.Application.Common.Interfaces;
using ArtVerse.Domain.Entities;
using ArtVerse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ArtVerse.Infrastructure.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly ApplicationDbContext _context;

    public StudentRepository(ApplicationDbContext context) => _context = context;

    public async Task<Student?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _context.Students
            .Include(s => s.Paintings)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<Student?> GetByCodeAsync(string code, CancellationToken ct = default)
        => await _context.Students
            .Include(s => s.Paintings)
            .FirstOrDefaultAsync(s => s.Code.ToLower() == code.Trim().ToLower(), ct);

    public async Task<Student?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await _context.Students
            .Include(s => s.Paintings)
            .FirstOrDefaultAsync(s => s.Email.ToLower() == email.Trim().ToLower(), ct);

    public async Task<Student?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await _context.Students
            .Include(s => s.Paintings)
            .FirstOrDefaultAsync(s => s.UserId == userId, ct);

    public async Task<(IReadOnlyList<Student> Items, int TotalCount)> GetPagedAsync(
        string? searchTerm, int page, int pageSize, CancellationToken ct = default)
    {
        var query = _context.Students
            .Include(s => s.Paintings)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim().ToLower();
            query = query.Where(s => s.FullName.ToLower().Contains(term) ||
                                     s.Code.ToLower().Contains(term) ||
                                     s.Email.ToLower().Contains(term));
        }

        var allItems = await query.ToListAsync(ct);
        var totalCount = allItems.Count;
        var items = allItems
            .OrderByDescending(s => s.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (items, totalCount);
    }

    public async Task AddAsync(Student student, CancellationToken ct = default)
        => await _context.Students.AddAsync(student, ct);

    public void Update(Student student) => _context.Students.Update(student);
    public void Delete(Student student) { student.MarkAsDeleted(); _context.Students.Update(student); }
    public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await _context.SaveChangesAsync(ct);
}
