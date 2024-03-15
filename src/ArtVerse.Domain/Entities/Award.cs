using ArtVerse.Domain.Common;

namespace ArtVerse.Domain.Entities;

public class Award : BaseEntity
{
    public Guid CompetitionId { get; private set; }
    public Guid EntryId { get; private set; }
    public CompetitionEntry? Entry { get; private set; }
    public string Title { get; private set; } = string.Empty;   // "Giải Nhất", "Giải Khuyến Khích"
    public int Rank { get; private set; }                       // 1, 2, 3, ...
    public decimal? PrizeAmount { get; private set; }
    public string? Description { get; private set; }

    private Award() { }

    public static Award Create(Guid competitionId, Guid entryId, string title, int rank, decimal? prizeAmount = null)
        => new() { CompetitionId = competitionId, EntryId = entryId, Title = title, Rank = rank, PrizeAmount = prizeAmount };
}
