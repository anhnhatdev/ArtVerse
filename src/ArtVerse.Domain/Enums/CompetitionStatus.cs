namespace ArtVerse.Domain.Enums;

public enum CompetitionStatus
{
    Draft = 0,
    Open = 1,             // Đang mở đăng ký
    SubmissionClosed = 2, // Đã đóng nộp bài
    Judging = 3,          // Đang chấm điểm
    Completed = 4,        // Đã công bố kết quả
    Cancelled = 5
}
