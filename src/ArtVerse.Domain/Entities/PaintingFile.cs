using ArtVerse.Domain.Common;

namespace ArtVerse.Domain.Entities;

/// <summary>
/// Một file ảnh thuộc về một tác phẩm (Painting có thể có nhiều góc chụp).
/// </summary>
public class PaintingFile : BaseEntity
{
    public Guid PaintingId { get; private set; }
    public Painting? Painting { get; private set; }
    public string FileUrl { get; private set; } = string.Empty;    // URL trên Azure Blob/local
    public string FileName { get; private set; } = string.Empty;
    public long FileSizeBytes { get; private set; }
    public bool IsPrimary { get; private set; } = false;           // Ảnh đại diện chính

    private PaintingFile() { }

    public static PaintingFile Create(Guid paintingId, string fileUrl, string fileName, long fileSizeBytes, bool isPrimary = false)
        => new() { PaintingId = paintingId, FileUrl = fileUrl, FileName = fileName, FileSizeBytes = fileSizeBytes, IsPrimary = isPrimary };
}
