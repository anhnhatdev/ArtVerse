namespace ArtVerse.Domain.Enums;

public enum ExhibitionStatus
{
    Draft = 0,
    Published = 1,   // Đã công bố, chưa diễn ra
    Ongoing = 2,     // Đang diễn ra
    Completed = 3,   // Đã kết thúc
    Cancelled = 4
}
