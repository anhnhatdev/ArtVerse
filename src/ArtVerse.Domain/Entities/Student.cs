using ArtVerse.Domain.Common;
using ArtVerse.Domain.Enums;
using ArtVerse.Domain.Exceptions;

namespace ArtVerse.Domain.Entities;

/// <summary>
/// Học viên — Entity trung tâm của hệ thống đào tạo.
/// UserId là Guid tham chiếu đến tài khoản login (ApplicationUser ở Infrastructure layer).
/// </summary>
public class Student : BaseEntity
{
    public string Code { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Phone { get; private set; }
    public DateOnly? DateOfBirth { get; private set; }
    public Gender? Gender { get; private set; }
    public string? AvatarUrl { get; private set; }
    public Guid? UserId { get; private set; }  // Link tới tài khoản login

    public ICollection<Painting> Paintings { get; private set; } = new List<Painting>();
    public ICollection<StudentEnrollment> Enrollments { get; private set; } = new List<StudentEnrollment>();

    private Student() { }

    public static Student Create(string fullName, string email, string code)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("Tên học viên không được để trống.");
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email không được để trống.");

        return new Student { FullName = fullName.Trim(), Email = email.Trim().ToLower(), Code = code };
    }

    public void Update(string fullName, string? phone, DateOnly? dateOfBirth, Gender? gender)
    {
        FullName = fullName.Trim();
        Phone = phone;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        SetUpdated();
    }

    public void SetAvatar(string avatarUrl) { AvatarUrl = avatarUrl; SetUpdated(); }
    public void LinkAccount(Guid userId) { UserId = userId; SetUpdated(); }
}
