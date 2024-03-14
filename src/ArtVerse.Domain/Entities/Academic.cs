using ArtVerse.Domain.Common;

namespace ArtVerse.Domain.Entities;

public class Subject : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CreditHours { get; set; } = 3;
}

public class Class : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Semester { get; set; }
    public int MaxStudents { get; set; } = 30;
    public ICollection<StudentEnrollment> Enrollments { get; set; } = new List<StudentEnrollment>();
}

public class StudentEnrollment : BaseEntity
{
    public Guid StudentId { get; set; }
    public Student? Student { get; set; }
    public Guid ClassId { get; set; }
    public Class? Class { get; set; }
    public DateTimeOffset EnrolledAt { get; set; } = DateTimeOffset.UtcNow;
    public string Status { get; set; } = "Active";   // Active, Suspended, Completed
}
