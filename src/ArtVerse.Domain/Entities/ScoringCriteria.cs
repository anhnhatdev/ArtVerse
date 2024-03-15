using ArtVerse.Domain.Common;

namespace ArtVerse.Domain.Entities;

public class ScoringCriteria : BaseEntity
{
    public Guid CompetitionId { get; private set; }
    public string Name { get; private set; } = string.Empty;        // VD: "Sáng tạo"
    public string? Description { get; private set; }
    public decimal MaxScore { get; private set; }                   // VD: 10
    public decimal Weight { get; private set; }                     // VD: 0.30 = 30%

    private ScoringCriteria() { }

    public static ScoringCriteria Create(Guid competitionId, string name, decimal maxScore, decimal weight, string? description = null)
        => new() { CompetitionId = competitionId, Name = name, MaxScore = maxScore, Weight = weight, Description = description };
}
