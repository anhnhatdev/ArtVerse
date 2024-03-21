using ArtVerse.Domain.Enums;

namespace ArtVerse.Application.Students.DTOs;

public record StudentDto(
    Guid Id,
    string Code,
    string FullName,
    string Email,
    string? Phone,
    DateOnly? DateOfBirth,
    Gender? Gender,
    string? AvatarUrl,
    int ArtworkCount,
    DateTimeOffset CreatedAt
);

public record CreateStudentDto(
    string FullName,
    string Email,
    string? Phone,
    DateOnly? DateOfBirth,
    Gender? Gender
);

public record UpdateStudentDto(
    Guid Id,
    string FullName,
    string? Phone,
    DateOnly? DateOfBirth,
    Gender? Gender
);
