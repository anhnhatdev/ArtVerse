namespace ArtVerse.Domain.Exceptions;

/// <summary>
/// Exception cho các lỗi nghiệp vụ (business rule violations).
/// Ví dụ: "Không thể duyệt tranh chưa được nộp."
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception innerException) : base(message, innerException) { }
}
