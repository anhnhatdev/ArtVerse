namespace ArtVerse.Application.Academic.DTOs;

public record ClassDto(
    Guid Id,
    string Code,
    string Name,
    int Year,
    int Semester,
    int MaxStudents,
    int EnrolledCount,
    DateTimeOffset CreatedAt
);

public record SubjectDto(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    int CreditHours
);

public record EnrollmentDto(
    Guid Id,
    Guid StudentId,
    string StudentName,
    string StudentCode,
    string Status,
    DateTimeOffset EnrolledAt
);
