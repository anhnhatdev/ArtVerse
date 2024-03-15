using ArtVerse.Domain.Common;
using ArtVerse.Domain.Enums;
using ArtVerse.Domain.Exceptions;

namespace ArtVerse.Domain.Entities;

public class Competition : BaseEntity
{
    public string Code { get; private set; } = string.Empty;    // COMP-2026-001
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Theme { get; private set; }                   // Chủ đề cuộc thi
    public CompetitionStatus Status { get; private set; } = CompetitionStatus.Draft;

    // Timeline
    public DateTimeOffset RegistrationStart { get; private set; }
    public DateTimeOffset RegistrationEnd { get; private set; }
    public DateTimeOffset SubmissionStart { get; private set; }
    public DateTimeOffset SubmissionEnd { get; private set; }
    public DateTimeOffset? AnnouncementDate { get; private set; }

    public int? MaxEntries { get; private set; }

    // Navigation Properties
    public ICollection<CompetitionEntry> Entries { get; private set; } = new List<CompetitionEntry>();
    public ICollection<ScoringCriteria> Criteria { get; private set; } = new List<ScoringCriteria>();

    private Competition() { }

    public static Competition Create(string title, string code, string? theme, string? description,
        DateTimeOffset registrationStart, DateTimeOffset registrationEnd,
        DateTimeOffset submissionStart, DateTimeOffset submissionEnd)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Tên cuộc thi không được để trống.");
        if (registrationEnd <= registrationStart)
            throw new DomainException("Ngày kết thúc đăng ký phải sau ngày mở đăng ký.");
        if (submissionEnd <= submissionStart)
            throw new DomainException("Ngày kết thúc nộp bài phải sau ngày mở nộp bài.");

        return new Competition
        {
            Title = title.Trim(), Code = code, Theme = theme, Description = description,
            RegistrationStart = registrationStart, RegistrationEnd = registrationEnd,
            SubmissionStart = submissionStart, SubmissionEnd = submissionEnd,
            Status = CompetitionStatus.Draft
        };
    }

    public void Publish()
    {
        if (Status != CompetitionStatus.Draft)
            throw new DomainException("Chỉ có thể công bố cuộc thi ở trạng thái Draft.");
        if (!Criteria.Any())
            throw new DomainException("Cuộc thi phải có ít nhất 1 tiêu chí chấm điểm trước khi công bố.");

        Status = CompetitionStatus.Open;
        SetUpdated();
    }

    public bool IsRegistrationOpen() =>
        Status == CompetitionStatus.Open &&
        DateTimeOffset.UtcNow >= RegistrationStart &&
        DateTimeOffset.UtcNow <= RegistrationEnd;
}
