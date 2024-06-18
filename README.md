# 🏛️ ArtVerse - Fine Arts Academy & Digital Exhibition Platform

> ⚠️ **LƯU Ý QUAN TRỌNG & ĐÁNH GIÁ THỰC TẾ (ARCHITECTURAL DISCLAIMER & RETROSPECTIVE)**
> 
> * **Mục đích dự án:** Đây là một đồ án nghiên cứu kiến trúc phần mềm (Academic Proof-of-Concept / Technical Showcase), tập trung giải quyết bài toán mô hình hóa kiến trúc doanh nghiệp: **Clean Architecture 4 lớp, CQRS Pattern với MediatR, Dual-Authentication (Cookie + JWT), REST API và Single Page Application trên Next.js 14 App Router**.
> * **Độ lệch nghiệp vụ so với thực tế (Domain Constraints):**
>   1. *Định lượng hóa nghệ thuật bằng Rubric:* Trong thực tế mỹ thuật chuyên nghiệp, việc chấm điểm tác phẩm mang nặng tính cảm thụ thị giác và triết lý cá nhân; việc lượng hóa thành ma trận Rubric Sliders chỉ phù hợp cho môi trường đào tạo sinh viên (học phần cơ sở, đồ án chuyên ngành) chứ không đại diện cho cách đánh giá của các gallery hay triển lãm nghệ thuật hàn lâm thực tế.
>   2. *Quy trình giám tuyển & Bản quyền:* Dự án mô phỏng luồng kiểm duyệt (Curation Pipeline) ở tầng logic phần mềm; ngoài đời thực, việc thẩm định tranh, cấp chứng thư giám định (Certificate of Authenticity - COA), ký gửi đấu giá và bản quyền tác giả đòi hỏi quy trình pháp lý, bảo hiểm và giám định vật lý phức tạp hơn rất nhiều.
> * **Khuyến cáo sử dụng:** **Không sử dụng trực tiếp mã nguồn này làm sàn thương mại nghệ thuật thương phẩm (Production Marketplace)** mà chưa qua nghiên cứu sâu về nghiệp vụ pháp lý, kiểm định chất liệu và cơ chế đấu giá thực tế.

---

## 📌 1. Giới Thiệu Tổng Quan

**ArtVerse** là nền tảng quản lý đào tạo mỹ thuật, giám tuyển tác phẩm học viên và tổ chức triển lãm trực tuyến. Hệ thống kết hợp giữa **ASP.NET Core 8 Web API** làm lõi xử lý nghiệp vụ phía máy chủ và **Next.js 14 (App Router)** cung cấp giao diện phòng tranh tương tác cao cấp theo phong cách *Obsidian Dark Luxury*.

---

## ✨ 2. Các Tính Năng & Phân Hệ Nổi Bật

### 🎨 Phân Hệ Mỹ Thuật & Thư Viện Tác Phẩm (`/artworks`)
* **Phòng trưng bày đa chất liệu:** Hỗ trợ phân loại và tìm kiếm tức thì theo Sơn dầu, Màu nước, Sơn mài, Lụa, Acrylic, Nghệ thuật số (Digital Art), Than chì, Phấn màu.
* **Soi chi tiết độ nét cao (HD Artwork Zoom):** Xem cận cảnh vệt cọ, bề mặt chất liệu và thuyết minh ý niệm sáng tác của tác giả.
* **Studio nộp tác phẩm trực tuyến (`/studio/upload`):** Sinh viên tải lên tác phẩm với đầy đủ kích thước, năm sáng tác và định giá giao lưu.

### ⚖️ Phân Hệ Giám Tuyển & Kiểm Duyệt (`/curation/review-queue`)
* **Hàng đợi xét duyệt giám tuyển:** Giám tuyển trưởng xem xét hồ sơ tác phẩm mới nộp, thực hiện thao tác **1-Click Phê duyệt / Từ chối** (kèm lý do hoàn thiện).
* **Cấp mã lưu trữ viện:** Tác phẩm sau khi duyệt tự động được cấp mã định danh chuẩn (`AV-2026-XXXX`) để đưa vào thư viện mở hoặc triển lãm.

### 🏆 Phân Hệ Cuộc Thi & Chấm Điểm Rubric (`/competitions`, `/jury/judging-room`)
* **Hội đồng chấm thi Split-Screen:** Màn hình chia đôi chuyên dụng cho Ban Giám khảo: bên trái soi tác phẩm phân giải cao, bên phải trượt điểm theo từng tiêu chí Rubric (Ý tưởng, Kỹ thuật, Bố cục, Cảm xúc).
* **Podium Bảng Vàng Tự Động:** Tính điểm bình quân theo trọng số và vinh danh Top 3 Quán quân - Giải Nhất 🥇, Giải Nhì 🥈, Giải Ba 🥉.

### 🌟 Triển Lãm Số & Không Gian 3D (`/exhibitions`)
* **Không gian triển lãm theo chủ đề:** Quản lý danh mục tranh theo từng sự kiện, lịch trình diễn ra và địa điểm ảo.
* **Tương tác thời gian thực:** Thả tim (Like) tác phẩm, đếm lượt xem và lưu trữ bộ sưu tập yêu thích.

### 🎓 Quản Lý Học Vụ & Lớp Học (`/academic/classes`)
* **Chương trình đào tạo:** Quản lý môn học, số tín chỉ, niên khóa và phân bổ sinh viên vào các lớp chuyên đề hình họa, sơn dầu, digital concept art.

### ⚡ 1-Click Role Switcher Demo (`/auth/login`)
* Thanh chuyển đổi nhanh 5 vai trò (*Admin, Giám tuyển, Giám khảo, Học viên, Khách vãng lai*) giúp người đánh giá trải nghiệm toàn bộ luồng nghiệp vụ mà không cần nhập lại tài khoản.

---

## 🏗️ 3. Kiến Trúc Kỹ Thuật (Architecture & Tech Stack)

Dự án được xây dựng theo chuẩn **Clean Architecture 4 Lớp kết hợp CQRS Pattern**:

```text
ArtVerse/
├── src/
│   ├── ArtVerse.Domain/          # Core Domain Entities, Value Objects, Enums, Exceptions
│   ├── ArtVerse.Application/     # CQRS Commands/Queries (MediatR), DTOs, AutoMapper, Validation
│   ├── ArtVerse.Infrastructure/  # EF Core 8 (SQLite/SQL Server), Identity, Repositories, JWT Token
│   └── ArtVerse.Web/             # ASP.NET Core 8 Web API + Swagger OpenAPI + Dual-Auth
│
└── frontend/                     # Next.js 14 App Router, TypeScript, TailwindCSS, Zustand, Lucide
```

### 🛠️ Chi Tiết Công Nghệ:
| Lớp | Công nghệ cốt lõi | Vai trò đảm nhiệm |
| :--- | :--- | :--- |
| **Backend Framework** | [.NET 8.0 LTS](https://dotnet.microsoft.com/) / C# 12 | Xử lý nghiệp vụ tập trung, hiệu năng cao |
| **Kiến trúc** | Clean Architecture + CQRS + DDD | Phân tách rành mạch Use Cases và Domain Model |
| **Điều phối tác vụ** | [MediatR 12.x](https://github.com/jbogard/MediatR) | Phân tách xử lý Commands (ghi) và Queries (đọc) |
| **ORM & Database** | EF Core 8.0 / SQLite / SQL Server | Quản lý dữ liệu quan hệ, tự động migrate & seed data |
| **Bảo mật & Auth** | ASP.NET Core Identity & JWT Bearer | Hệ thống phân quyền RBAC đa tầng, Hybrid Cookie/JWT |
| **API Documentation** | [Swagger / OpenAPI v1](https://swagger.io/) | Cung cấp tài liệu API tương tác tại `/swagger` |
| **Frontend Framework** | [Next.js 14](https://nextjs.org/) (App Router) | Client SPA kết xuất nhanh, giao diện nghệ thuật cao cấp |
| **Styling & State** | TailwindCSS + Zustand + Lucide Icons | Quản lý trạng thái đăng nhập và giao diện Obsidian Dark |

---

## 🚀 4. Hướng Dẫn Cài Đặt & Khởi Chạy Hệ Thống

### Yêu Cầu Môi Trường:
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Node.js 18+](https://nodejs.org/) & npm

### Bước 1: Khởi chạy Backend (.NET 8 REST API)
Mở Terminal tại thư mục `C:\final\ArtVerse` và chạy:
```bash
dotnet run --project src/ArtVerse.Web
```
* **API Server:** `http://localhost:5115`
* **Tài liệu Swagger:** `http://localhost:5115/swagger`
*(Hệ thống tự động khởi tạo cơ sở dữ liệu `artverse.db` và nạp sẵn 18+ tác phẩm mẫu, học viên, cuộc thi và triển lãm).*

### Bước 2: Khởi chạy Frontend (Next.js 14)
Mở một cửa sổ Terminal khác tại thư mục `C:\final\ArtVerse\frontend` và chạy:
```bash
npm install
npm run dev
```
* **Giao diện người dùng:** **[http://localhost:3000](http://localhost:3000)**

---

## 🔑 5. Tài Khoản Thử Nghiệm Mặc Định (Demo Accounts)

| Vai trò | Email | Mật khẩu | Chức năng chính |
| :--- | :--- | :--- | :--- |
| **Quản trị viên (Admin)** | `admin@artverse.edu.vn` | `Admin@123` | KPI Dashboard, Quản lý người dùng |
| **Giám tuyển (Curator)** | `curator@artverse.edu.vn` | `Curator@123` | Duyệt tranh, tổ chức triển lãm |
| **Giảng viên / Giám khảo** | `teacher@artverse.edu.vn` | `Teacher@123` | Quản lý lớp, chấm thi Rubric |
| **Học viên xuất sắc** | `student@artverse.edu.vn` | `Student@123` | Upload tranh, nộp bài thi đấu |

---

## 📄 6. Giấy Phép (License)
Dự án được phân phối dưới giấy phép **MIT License**.
