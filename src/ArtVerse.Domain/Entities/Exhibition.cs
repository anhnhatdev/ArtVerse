using ArtVerse.Domain.Common;
using ArtVerse.Domain.Enums;
using ArtVerse.Domain.Exceptions;

namespace ArtVerse.Domain.Entities;

public class Exhibition : BaseEntity
{
    public string Code { get; private set; } = string.Empty;   // EX-2026-001
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Venue { get; private set; }                 // Địa điểm
    public string? Address { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }
    public string? ThumbnailUrl { get; private set; }
    public decimal? TicketPrice { get; private set; }           // null = miễn phí
    public ExhibitionStatus Status { get; private set; } = ExhibitionStatus.Draft;

    public ICollection<ExhibitionArtwork> Artworks { get; private set; } = new List<ExhibitionArtwork>();

    private Exhibition() { }

    public static Exhibition Create(string title, string code, DateTimeOffset startDate, DateTimeOffset endDate,
        string? venue = null, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Tên triển lãm không được để trống.");
        if (endDate <= startDate)
            throw new DomainException("Ngày kết thúc phải sau ngày bắt đầu.");

        return new Exhibition { Title = title.Trim(), Code = code, StartDate = startDate, EndDate = endDate, Venue = venue, Description = description };
    }

    public void Publish() { Status = ExhibitionStatus.Published; SetUpdated(); }
}
