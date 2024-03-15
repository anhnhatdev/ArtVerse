using ArtVerse.Domain.Common;
using ArtVerse.Domain.Enums;
using ArtVerse.Domain.Exceptions;

namespace ArtVerse.Domain.Entities;

/// <summary>
/// Bài dự thi — liên kết giữa Painting và Competition.
/// Một học viên nộp một tác phẩm vào một cuộc thi.
/// </summary>
public class CompetitionEntry : BaseEntity
{
    public Guid CompetitionId { get; private set; }
    public Competition? Competition { get; private set; }
    public Guid PaintingId { get; private set; }
    public Painting? Painting { get; private set; }
    public Guid StudentId { get; private set; }
    public Student? Student { get; private set; }
    public string EntryCode { get; private set; } = string.Empty;   // VD: COMP-2026-001-S042
    public EntryStatus Status { get; private set; } = EntryStatus.Pending;
    public DateTimeOffset SubmittedAt { get; private set; } = DateTimeOffset.UtcNow;

    public ICollection<EntryScore> Scores { get; private set; } = new List<EntryScore>();

    private CompetitionEntry() { }

    public static CompetitionEntry Create(Guid competitionId, Guid paintingId, Guid studentId, string entryCode)
        => new() { CompetitionId = competitionId, PaintingId = paintingId, StudentId = studentId, EntryCode = entryCode };

    public void Accept() { Status = EntryStatus.Accepted; SetUpdated(); }
    public void Reject() { Status = EntryStatus.Rejected; SetUpdated(); }

    /// <summary>
    /// Tính điểm bình quân từ tất cả giám khảo (đã nhân trọng số).
    /// </summary>
    public decimal CalculateAverageScore()
    {
        if (!Scores.Any()) return 0;
        return Scores.GroupBy(s => s.JudgeId)
            .Select(g => g.Sum(s => s.WeightedScore))
            .Average();
    }
}
