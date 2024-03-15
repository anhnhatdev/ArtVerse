using ArtVerse.Domain.Common;
using ArtVerse.Domain.Exceptions;

namespace ArtVerse.Domain.Entities;

public class Staff : BaseEntity
{
    public string Code { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Phone { get; private set; }
    public string? Department { get; private set; }
    public string? Title { get; private set; }
    public string? AvatarUrl { get; private set; }
    public Guid? UserId { get; private set; }

    private Staff() { }

    public static Staff Create(string fullName, string email, string code)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("Tên giảng viên không được để trống.");
        return new Staff { FullName = fullName.Trim(), Email = email.Trim().ToLower(), Code = code };
    }

    public void Update(string fullName, string? phone, string? department, string? title)
    {
        FullName = fullName.Trim(); Phone = phone; Department = department; Title = title;
        SetUpdated();
    }
}
