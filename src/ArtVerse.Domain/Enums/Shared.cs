namespace ArtVerse.Domain.Enums;

public enum Gender { Male, Female, Other }

public enum ArtTechnique
{
    OilPainting, Watercolor, Acrylic, Pastel,
    Charcoal, Pencil, Ink, Digital, Sculpture, Mixed, Other
}

public enum EntryStatus
{
    Pending = 0,     // Chờ xét duyệt
    Accepted = 1,    // Đã được chấp nhận
    Rejected = 2,    // Bị từ chối
    Disqualified = 3 // Bị loại (vi phạm thể lệ)
}
