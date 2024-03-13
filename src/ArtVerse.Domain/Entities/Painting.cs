using ArtVerse.Domain.Common;
using ArtVerse.Domain.Enums;
using ArtVerse.Domain.Exceptions;

namespace ArtVerse.Domain.Entities;

/// <summary>
/// Tác phẩm hội họa — Entity cốt lõi của hệ thống.
/// Có vòng đời: Draft → Submitted → Approved/Rejected → OnExhibit → Sold
/// </summary>
public class Painting : BaseEntity
{
    public string Code { get; private set; } = string.Empty;     // AV-2026-0001
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public ArtTechnique Technique { get; private set; }
    public int? CreatedYear { get; private set; }
    public string? Tags { get; private set; }                    // JSON array of tags
    public PaintingStatus Status { get; private set; } = PaintingStatus.Draft;
    public bool IsForSale { get; private set; } = false;
    public decimal? BasePrice { get; private set; }

    // Liên kết đến học viên sở hữu tác phẩm
    public Guid StudentId { get; private set; }
    public Student? Student { get; private set; }

    // Lý do từ chối (khi Staff reject)
    public string? RejectionReason { get; private set; }

    // Danh sách file ảnh của tác phẩm
    public ICollection<PaintingFile> Files { get; private set; } = new List<PaintingFile>();

    private Painting() { }

    /// <summary>
    /// Tạo tác phẩm mới. Luôn bắt đầu ở trạng thái Draft.
    /// </summary>
    public static Painting Create(string title, string code, Guid studentId,
        ArtTechnique technique, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Tên tác phẩm không được để trống.");

        return new Painting
        {
            Title = title.Trim(),
            Code = code,
            StudentId = studentId,
            Technique = technique,
            Description = description,
            Status = PaintingStatus.Draft
        };
    }

    /// <summary>
    /// Học viên nộp tác phẩm để Staff duyệt.
    /// Chỉ có thể nộp khi đang ở Draft.
    /// </summary>
    public void Submit()
    {
        if (Status != PaintingStatus.Draft)
            throw new DomainException($"Không thể nộp tác phẩm ở trạng thái '{Status}'. Chỉ Draft mới nộp được.");

        Status = PaintingStatus.Submitted;
        SetUpdated();
    }

    /// <summary>
    /// Staff duyệt tác phẩm.
    /// </summary>
    public void Approve()
    {
        if (Status != PaintingStatus.Submitted)
            throw new DomainException("Chỉ có thể duyệt tác phẩm đã được nộp (Submitted).");

        Status = PaintingStatus.Approved;
        RejectionReason = null;
        SetUpdated();
    }

    /// <summary>
    /// Staff từ chối tác phẩm, kèm lý do bắt buộc.
    /// </summary>
    public void Reject(string reason)
    {
        if (Status != PaintingStatus.Submitted)
            throw new DomainException("Chỉ có thể từ chối tác phẩm đã được nộp (Submitted).");
        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException("Phải có lý do khi từ chối tác phẩm.");

        Status = PaintingStatus.Rejected;
        RejectionReason = reason.Trim();
        SetUpdated();
    }

    /// <summary>
    /// Đưa tác phẩm vào trưng bày tại triển lãm.
    /// </summary>
    public void PutOnExhibit()
    {
        if (Status != PaintingStatus.Approved)
            throw new DomainException("Chỉ có thể trưng bày tác phẩm đã được duyệt (Approved).");

        Status = PaintingStatus.OnExhibit;
        SetUpdated();
    }

    /// <summary>
    /// Đánh dấu tác phẩm đã được bán.
    /// </summary>
    public void MarkAsSold()
    {
        if (Status != PaintingStatus.OnExhibit && Status != PaintingStatus.Approved)
            throw new DomainException("Tác phẩm phải đang trưng bày hoặc đã duyệt mới có thể bán.");

        Status = PaintingStatus.Sold;
        SetUpdated();
    }

    public void Update(string title, string? description, ArtTechnique technique,
        int? createdYear, bool isForSale, decimal? basePrice)
    {
        if (Status != PaintingStatus.Draft && Status != PaintingStatus.Rejected)
            throw new DomainException("Chỉ có thể sửa tác phẩm ở trạng thái Draft hoặc Rejected.");

        Title = title.Trim();
        Description = description;
        Technique = technique;
        CreatedYear = createdYear;
        IsForSale = isForSale;
        BasePrice = isForSale ? basePrice : null;
        SetUpdated();
    }
}
