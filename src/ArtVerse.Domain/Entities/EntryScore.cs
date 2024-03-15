using ArtVerse.Domain.Common;

namespace ArtVerse.Domain.Entities;

public class EntryScore : BaseEntity
{
    public Guid EntryId { get; private set; }
    public Guid JudgeId { get; private set; }       // StaffId của giám khảo
    public Staff? Judge { get; private set; }
    public Guid CriteriaId { get; private set; }
    public ScoringCriteria? Criteria { get; private set; }
    public decimal Score { get; private set; }      // Điểm thô
    public decimal Weight { get; private set; }     // Trọng số tại thời điểm chấm
    public decimal WeightedScore => Score * Weight; // Điểm đã nhân trọng số
    public string? Comment { get; private set; }

    private EntryScore() { }

    public static EntryScore Create(Guid entryId, Guid judgeId, Guid criteriaId, decimal score, decimal weight, string? comment)
        => new() { EntryId = entryId, JudgeId = judgeId, CriteriaId = criteriaId, Score = score, Weight = weight, Comment = comment };
}
