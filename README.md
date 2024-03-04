<div align="center">

# 🎨 ArtVerse
### Next-Generation Enterprise Art Academy & Digital Exhibition Platform

[![.NET 8 LTS](https://img.shields.io/badge/.NET-8.0_LTS-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C# 12](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![Clean Architecture](https://img.shields.io/badge/Architecture-Clean_Architecture-007ACC?style=for-the-badge&logo=visual-studio-code&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)
[![Entity Framework Core 8](https://img.shields.io/badge/ORM-EF_Core_8-5C2D91?style=for-the-badge&logo=nuget&logoColor=white)](https://learn.microsoft.com/en-us/ef/core/)
[![Bootstrap 5](https://img.shields.io/badge/UI-Bootstrap_5-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)](https://getbootstrap.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-success?style=for-the-badge)](LICENSE)

<p align="center">
  <b>ArtVerse</b> là nền tảng quản trị tổng thể dành cho các Học viện Nghệ thuật, Trung tâm Triển lãm và Sàn giao dịch Nghệ thuật số cao cấp. Hệ thống được xây dựng trên nền tảng <b>.NET 8 LTS</b> hiện đại theo kiến trúc <b>Clean Architecture</b>, áp dụng mẫu thiết kế <b>CQRS (Command Query Responsibility Segregation)</b> và mô hình hóa nghiệp vụ theo <b>Domain-Driven Design (DDD)</b>.
</p>

[Tính Năng Nổi Bật](#-tính-năng-nổi-bật) • [Kiến Trúc Hệ Thống](#-kiến-trúc-hệ-thống) • [Công Nghệ Sử Dụng](#-công-nghệ-sử-dụng) • [Hướng Dẫn Cài Đặt](#-hướng-dẫn-cài-đặt) • [Cấu Trúc Dự Án](#-cấu-trúc-thư-mục)

</div>

---

## 🌟 Tính Năng Nổi Bật

### 1. 📊 Trung Tâm Điều Hành & Phân Tích Dữ Liệu (Executive Analytics)
* **Tổng quan KPI thời gian thực**: Theo dõi số lượng học viên, sản lượng tác phẩm, doanh thu niêm yết và các sự kiện đang diễn ra.
* **Trực quan hóa dữ liệu với ApexCharts**:
  * Biểu đồ miền (Area Chart) theo dõi tốc độ tăng trưởng tác phẩm sáng tác theo từng tháng.
  * Biểu đồ Donut phân tích thị phần và phân bố kỹ thuật hội họa (Sơn dầu, Màu nước, Acrylic, Kỹ thuật số, Điêu khắc...).

### 2. 🎨 Quản Lý Vòng Đời Tác Phẩm & Hàng Đợi Phê Duyệt (Artwork Lifecycle)
* **Thư viện tranh trực quan (Card Grid & Modal Preview)**: Tìm kiếm và lọc tác phẩm tức thì theo kỹ thuật, tác giả và trạng thái.
* **Pipeline tải lên & bảo hộ tác phẩm**: Đăng ký thông tin quyền tác giả, thông số kích thước, kỹ thuật và giá niêm yết thương mại.
* **Hàng đợi kiểm duyệt chuyên môn (Review Queue)**: Dành riêng cho Hội đồng nghệ thuật thẩm định, phê duyệt hoặc từ chối tác phẩm kèm lý do chuyên môn.

### 3. 🏆 Động Cơ Quản Lý Cuộc Thi & Phòng Chấm Thi Chuyên Sâu (Competition Engine)
* **Quản trị cuộc thi nhiều vòng**: Thiết lập thể lệ, thời hạn, cơ cấu giải thưởng và hạn mức nộp bài.
* **Phòng chấm thi Split-Screen**: Giao diện chia đôi màn hình độc quyền — một bên soi chi tiết tác phẩm chất lượng cao, một bên nhập điểm theo ma trận Rubric đa tiêu chí có trọng số.
* **Bảng xếp hạng Podium tự động**: Tự động tổng hợp điểm số từ ban giám khảo và vinh danh Top 3 kèm huy chương.

### 4. 🏛️ Triển Lãm Nghệ Thuật Số & Không Gian Trưng Bày (Virtual Exhibition)
* **Không gian triển lãm chuyên đề**: Quản lý sự kiện, địa điểm, lịch trình và danh mục tranh trưng bày phong cách Masonry Gallery.
* **Studio tuyển chọn tác phẩm (Curator Studio)**: Công cụ cho phép giám tuyển lựa chọn và sắp xếp thứ tự ưu tiên các tác phẩm vào sự kiện.
* **Tương tác trực tiếp**: Thả tim (Like) thời gian thực, lưu danh sách yêu thích và hỗ trợ đăng ký sở hữu tác phẩm độc bản.

### 5. 📚 Quản Lý Đào Tạo & Phân Bổ Học Viên (Academic Administration)
* **Quản lý khóa học & lớp chuyên ngành**: Theo dõi niên khóa, học kỳ và kiểm soát sĩ số tối đa của từng lớp.
* **Khung chương trình đào tạo**: Danh mục môn học nghệ thuật, số tín chỉ và chuyên đề học phần.
* **Phân bổ học viên**: Ghi danh học viên vào các lớp học chuyên sâu chỉ với 1 thao tác.

### 6. 👥 Quản Trị Học Viên & Phân Quyền Đa Tầng (User & RBAC Security)
* Quản lý hồ sơ học viên, thông tin liên hệ và danh mục tác phẩm sáng tác cá nhân (Portfolio).
* Hệ thống phân quyền dựa trên vai trò (**Role-Based Access Control - RBAC**): `Admin`, `Principal`, `Manager`, `Staff`, `Student`.

---

## 🏗️ Kiến Trúc Hệ Thống

Dự án tuân thủ nghiêm ngặt nguyên lý **Clean Architecture**, phân tách thành 4 tầng độc lập theo hình đồng tâm nhằm đảm bảo khả năng mở rộng, kiểm thử và bảo trì lâu dài:

```text
                               ┌───────────────────────────┐
                               │       ArtVerse.Web        │ 
                               │   (MVC / Razor / UI)      │
                               └─────────────┬─────────────┘
                                             │ depends on
                               ┌─────────────▼─────────────┐
                               │   ArtVerse.Application    │
                               │   (CQRS / MediatR / DTOs) │
                               └─────────────┬─────────────┘
                                             │ depends on
               ┌─────────────────────────────┼─────────────────────────────┐
               │                                                           │
┌──────────────▼──────────────┐                             ┌──────────────▼──────────────┐
│       ArtVerse.Domain       │ (Core Business Logic)       │   ArtVerse.Infrastructure   │ (Data Access & Security)
│  (Entities, Enums, Rules)   │                             │   (EF Core, Identity, Repos)│
└─────────────────────────────┘                             └─────────────────────────────┘
```

* **`ArtVerse.Domain`**: Chứa toàn bộ các Entity nghiệp vụ thuần túy (`Student`, `Staff`, `Painting`, `Competition`, `Exhibition`, `Class`, `Subject`), Value Objects, Domain Enums và Exceptions. Hoàn toàn không phụ thuộc vào bất kỳ thư viện bên ngoài hay cơ sở dữ liệu nào.
* **`ArtVerse.Application`**: Hiện thực các Use Cases thông qua mô hình **CQRS** sử dụng thư viện **MediatR**, các hợp đồng giao tiếp (Interfaces), DTOs và quy tắc xác thực.
* **`ArtVerse.Infrastructure`**: Hiện thực hóa việc truy xuất dữ liệu với **Entity Framework Core 8**, hỗ trợ cơ chế đa hệ quản trị CSDL (**SQLite** / **SQL Server**), quản trị định danh người dùng qua **ASP.NET Core Identity** và triển khai các Repository.
* **`ArtVerse.Web`**: Tầng giao diện người dùng ASP.NET Core MVC 8, kết hợp Razor Pages, Bootstrap 5, ApexCharts và cấu hình Dependency Injection trung tâm.

---

## 🛠️ Công Nghệ Sử Dụng

| Hạng mục | Công nghệ / Thư viện | Mục đích sử dụng |
| :--- | :--- | :--- |
| **Framework Cốt Lõi** | [.NET 8.0 LTS](https://dotnet.microsoft.com/) / C# 12 | Nền tảng thực thi backend hiệu năng cao |
| **Kiến Trúc** | Clean Architecture + CQRS + DDD | Chuẩn hóa thiết kế phần mềm doanh nghiệp |
| **Điều Phối Tác Vụ** | [MediatR 12.3](https://github.com/jbogard/MediatR) | Phân tách Command / Query trong CQRS |
| **Truy Xuất Dữ Liệu** | [Entity Framework Core 8.0](https://learn.microsoft.com/en-us/ef/core/) | ORM hiện đại, Linq Provider |
| **Cơ Sở Dữ Liệu** | SQLite (Zero-config) / Microsoft SQL Server | Lưu trữ dữ liệu hệ thống |
| **Bảo Mật & Xác Thực** | ASP.NET Core Identity 8.0 | Quản lý tài khoản, mã hóa mật khẩu & RBAC |
| **Giao Diện Người Dùng** | Razor Pages + Bootstrap 5 + Bootstrap Icons | Giao diện Responsive hiện đại, trực quan |
| **Biểu Đồ & Thống Kê** | [ApexCharts JS](https://apexcharts.com/) | Trực quan hóa dữ liệu Dashboard tương tác cao |

---

## 🚀 Hướng Dẫn Cài Đặt & Khởi Chạy

### 1. Yêu Cầu Tiền Đề
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (hoặc mới hơn).
* Trình duyệt web hiện đại (Chrome, Edge, Firefox, Safari).

### 2. Khởi Chạy Nhanh (Zero-Configuration)
Mở Terminal tại thư mục gốc của dự án (`C:\final\ArtVerse`) và thực thi lệnh:

```bash
dotnet run --project src/ArtVerse.Web
```

👉 Mở trình duyệt và truy cập: **[http://localhost:5115](http://localhost:5115)**

> 💡 **Khởi tạo tự động**: Khi ứng dụng khởi chạy lần đầu, cơ sở dữ liệu `artverse.db` và toàn bộ dữ liệu mẫu (Học viên, Tác phẩm, Cuộc thi, Triển lãm, Lớp học) sẽ **tự động được khởi tạo hoàn toàn**.

### 3. Tài Khoản Quản Trị Mặc Định
* **Email:** `admin@artverse.com`
* **Mật khẩu:** `Admin@123`

---

## 📁 Cấu Trúc Thư Mục

```text
ArtVerse/
├── README.md                           # Tài liệu tổng quan dự án
├── ArtVerse.sln                        # Visual Studio Solution File
└── src/
    ├── ArtVerse.Domain/                # Tầng Domain (Nghiệp vụ cốt lõi)
    │   ├── Common/ (BaseEntity.cs)
    │   ├── Entities/ (Student, Staff, Painting, Competition, Exhibition, Academic...)
    │   ├── Enums/ (PaintingStatus, CompetitionStatus, ExhibitionStatus, UserRole...)
    │   └── Exceptions/ (DomainException.cs)
    │
    ├── ArtVerse.Application/           # Tầng Application (CQRS & Use Cases)
    │   ├── Common/Interfaces/ (Repository Contracts)
    │   ├── Students/ (CQRS Commands, Queries, DTOs)
    │   ├── Paintings/ (CQRS Commands, Queries, DTOs)
    │   ├── Competitions/ (CQRS Commands, Queries, DTOs)
    │   ├── Exhibitions/ (CQRS Commands, Queries, DTOs)
    │   ├── Academic/ (CQRS Commands, Queries, DTOs)
    │   └── Admin/ (CQRS Queries, Analytics DTOs)
    │
    ├── ArtVerse.Infrastructure/        # Tầng Infrastructure (Database & Identity)
    │   ├── Data/ (ApplicationDbContext, ApplicationUser)
    │   ├── Data/Seed/ (DbSeeder.cs - Khởi tạo dữ liệu mẫu ban đầu)
    │   └── Repositories/ (Student, Painting, Competition, Exhibition, Academic, Analytics)
    │
    └── ArtVerse.Web/                   # Tầng Web (MVC & Presentation)
        ├── Controllers/ (Home, Students, Artworks, Competitions, Exhibitions, Academic, Admin)
        ├── Views/ (Razor Views giao diện người dùng)
        ├── wwwroot/ (Static assets, CSS, JS, Uploads hình ảnh)
        ├── Program.cs (Cấu hình DI, Authentication, Middleware Pipeline)
        └── appsettings.json (Cấu hình kết nối cơ sở dữ liệu)
```

---

## 🛡️ Tiêu Chuẩn Bảo Mật & Vận Hành

* **Chống giả mạo yêu cầu (Anti-CSRF)**: Toàn bộ form thao tác dữ liệu đều được bảo vệ bởi thẻ `[ValidateAntiForgeryToken]`.
* **Phân quyền truy cập đa tầng (Role-Based Authorization)**: Kiểm soát nghiêm ngặt các vùng dữ liệu nhạy cảm theo chính sách `AdminOnly`, `ManagerOrAbove`, `StaffOrAbove`, `StudentOnly`.
* **Toàn vẹn dữ liệu (Soft Delete & Auditing)**: Kế thừa `BaseEntity` hỗ trợ tự động ghi nhận thời gian khởi tạo (`CreatedAt`), cập nhật (`UpdatedAt`) và cơ chế xóa mềm (`IsDeleted`).

---

## 📜 Giấy Phép (License)

Dự án được phân phối dưới giấy phép **MIT License**.
