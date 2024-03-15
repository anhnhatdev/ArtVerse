namespace ArtVerse.Domain.Enums;

public enum PaintingStatus
{
    Draft = 0,       // Bản nháp (mới tạo)
    Submitted = 1,   // Đã nộp, chờ duyệt
    Approved = 2,    // Đã được duyệt, công khai
    Rejected = 3,    // Bị từ chối (có lý do)
    OnExhibit = 4,   // Đang trưng bày tại triển lãm
    Sold = 5         // Đã bán
}
