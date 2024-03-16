using ArtVerse.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace ArtVerse.Infrastructure.Data;

/// <summary>
/// Tài khoản người dùng đăng nhập — mở rộng từ ASP.NET Core Identity.
/// Đặt ở Infrastructure vì nó phụ thuộc vào thư viện Identity.
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public UserRole Role { get; set; } = UserRole.Student;
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
