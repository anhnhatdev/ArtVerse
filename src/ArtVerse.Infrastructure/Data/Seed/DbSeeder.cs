using ArtVerse.Domain.Entities;
using ArtVerse.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ArtVerse.Infrastructure.Data.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // 1. Tạo các Roles mặc định
        string[] roles = { "Admin", "Principal", "Manager", "Staff", "Student", "Collector" };
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            }
        }

        // 2. Tạo các tài khoản thử nghiệm cho từng Role
        var usersToSeed = new (string Email, string Name, string Password, UserRole Role, string RoleName)[]
        {
            ("admin@artverse.com", "System Administrator", "Admin@123", UserRole.Admin, "Admin"),
            ("principal@artverse.com", "Prof. Trần Văn Đạo", "Principal@123", UserRole.Principal, "Principal"),
            ("staff@artverse.com", "ThS. Lê Hoàng Yến", "Staff@123", UserRole.Staff, "Staff"),
            ("student@artverse.com", "Nguyễn Minh Châu", "Student@123", UserRole.Student, "Student")
        };

        ApplicationUser? studentUser = null;
        ApplicationUser? staffUser = null;

        foreach (var item in usersToSeed)
        {
            var user = await userManager.FindByEmailAsync(item.Email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = item.Email,
                    Email = item.Email,
                    FullName = item.Name,
                    EmailConfirmed = true,
                    Role = item.Role,
                    IsActive = true
                };
                var result = await userManager.CreateAsync(user, item.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, item.RoleName);
                }
            }

            if (item.Email == "student@artverse.com") studentUser = user;
            if (item.Email == "staff@artverse.com") staffUser = user;
        }

        // 3. Seed Staff (Giám khảo & Giảng viên)
        if (!context.Staffs.Any())
        {
            var st1 = Staff.Create("ThS. Lê Hoàng Yến", "staff@artverse.com", "GV2026-001");
            st1.Update("ThS. Lê Hoàng Yến", "0901234567", "Khoa Hội Họa", "Trưởng bộ môn Sơn dầu");
            context.Staffs.Add(st1);
            await context.SaveChangesAsync();
        }

        var defaultStaff = await context.Staffs.FirstOrDefaultAsync();
        var judgeStaffId = defaultStaff?.Id ?? Guid.NewGuid();

        // 4. Seed Students
        if (!context.Students.Any())
        {
            var s1 = Student.Create("Nguyễn Minh Châu", "student@artverse.com", "SV2026-001");
            if (studentUser != null) s1.LinkAccount(studentUser.Id);

            var s2 = Student.Create("Trần Hoàng Nam", "nam.tran@artverse.edu.vn", "SV2026-002");
            var s3 = Student.Create("Lê Phương Thảo", "thao.le@artverse.edu.vn", "SV2026-003");
            var s4 = Student.Create("Phạm Quốc Anh", "anh.pham@artverse.edu.vn", "SV2026-004");
            var s5 = Student.Create("Vũ Mai Linh", "linh.vu@artverse.edu.vn", "SV2026-005");

            context.Students.AddRange(s1, s2, s3, s4, s5);
            await context.SaveChangesAsync();

            // 5. Seed Paintings with high quality Unsplash Art Photos
            var p1 = Painting.Create("Bình Minh Trên Vịnh Hạ Long", "AV-2026-0001", s1.Id, ArtTechnique.OilPainting, "Tác phẩm khắc họa vẻ đẹp huyền ảo của Vịnh Hạ Long trong sương sớm bằng chất liệu sơn dầu đắp nổi.");
            p1.Update(p1.Title, p1.Description, p1.Technique, 2026, true, 15000000);
            p1.Submit(); p1.Approve();
            p1.Files.Add(PaintingFile.Create(p1.Id, "https://images.unsplash.com/photo-1579783902614-a3fb3927b675?w=800&auto=format&fit=crop", "ha-long-sunrise.jpg", 2048576, true));

            var p2 = Painting.Create("Ký Ức Phố Cổ Hà Nội", "AV-2026-0002", s2.Id, ArtTechnique.Watercolor, "Bức tranh màu nước trong trẻo thể hiện nhịp sống bình yên và nét cổ kính của 36 phố phường.");
            p2.Update(p2.Title, p2.Description, p2.Technique, 2026, true, 8500000);
            p2.Submit(); p2.Approve();
            p2.Files.Add(PaintingFile.Create(p2.Id, "https://images.unsplash.com/photo-1582561424760-0321d75e81fa?w=800&auto=format&fit=crop", "pho-co-hanoi.jpg", 1854200, true));

            var p3 = Painting.Create("Vũ Điệu Ánh Sáng Cyberpunk", "AV-2026-0003", s3.Id, ArtTechnique.Digital, "Nghệ thuật số (Digital Concept Art) lấy cảm hứng từ tương lai viễn tưởng pha trộn nét truyền thống Á Đông.");
            p3.Update(p3.Title, p3.Description, p3.Technique, 2026, true, 12000000);
            p3.Submit(); p3.Approve();
            p3.Files.Add(PaintingFile.Create(p3.Id, "https://images.unsplash.com/photo-1578301978693-85fa9c0320b9?w=800&auto=format&fit=crop", "cyberpunk-dance.png", 3540000, true));

            var p4 = Painting.Create("Mùa Thu Rực Rỡ", "AV-2026-0004", s4.Id, ArtTechnique.OilPainting, "Tranh sơn dầu phong cảnh rừng cây chuyển sắc mùa thu với gam màu ấm áp, giàu cảm xúc.");
            p4.Update(p4.Title, p4.Description, p4.Technique, 2026, true, 9800000);
            p4.Submit(); p4.Approve();
            p4.Files.Add(PaintingFile.Create(p4.Id, "https://images.unsplash.com/photo-1577083552431-6e5fd01aa342?w=800&auto=format&fit=crop", "autumn-colors.jpg", 2150000, true));

            var p5 = Painting.Create("Tĩnh Vật Sen Trắng", "AV-2026-0005", s5.Id, ArtTechnique.Acrylic, "Bức họa tĩnh vật hoa sen thuần khiết thể hiện sự tĩnh lặng và chiều sâu tâm hồn người nghệ sĩ.");
            p5.Update(p5.Title, p5.Description, p5.Technique, 2026, true, 6200000);
            p5.Submit(); p5.Approve();
            p5.Files.Add(PaintingFile.Create(p5.Id, "https://images.unsplash.com/photo-1579783900882-c0d3dad7b119?w=800&auto=format&fit=crop", "lotus-still-life.jpg", 1920000, true));

            var p6 = Painting.Create("Dòng Chảy Thời Gian (Abstract)", "AV-2026-0006", s1.Id, ArtTechnique.OilPainting, "Tác phẩm trừu tượng thể hiện sự chuyển động không ngừng của không gian và thời gian đương đại.");
            p6.Update(p6.Title, p6.Description, p6.Technique, 2026, true, 22000000);
            p6.Submit(); p6.Approve();
            p6.Files.Add(PaintingFile.Create(p6.Id, "https://images.unsplash.com/photo-1541701494587-cb58502866ab?w=800&auto=format&fit=crop", "flow-of-time.jpg", 4120000, true));

            // Tác phẩm đang chờ duyệt
            var p7 = Painting.Create("Hoàng Hôn Trên Sông Hương", "AV-2026-0007", s1.Id, ArtTechnique.Watercolor, "Bức tranh thể hiện chiều sâu không gian sông Hương lúc chiều tà.");
            p7.Submit();
            p7.Files.Add(PaintingFile.Create(p7.Id, "https://images.unsplash.com/photo-1579783901586-d88db74b4fe5?w=800&auto=format&fit=crop", "perfume-river.jpg", 1950000, true));

            context.Paintings.AddRange(p1, p2, p3, p4, p5, p6, p7);
            await context.SaveChangesAsync();

            // 6. Seed Competitions
            var comp1 = Competition.Create(
                title: "ArtVerse National Young Talents 2026",
                code: "COMP-2026-001",
                theme: "Sắc Màu Tương Lai (Colors of Tomorrow)",
                description: "Cuộc thi tìm kiếm tài năng hội họa trẻ xuất sắc nhất năm 2026 trên toàn quốc dành cho học viên viện mỹ thuật.",
                registrationStart: DateTimeOffset.UtcNow.AddDays(-10),
                registrationEnd: DateTimeOffset.UtcNow.AddDays(20),
                submissionStart: DateTimeOffset.UtcNow.AddDays(-5),
                submissionEnd: DateTimeOffset.UtcNow.AddDays(30)
            );

            context.Competitions.Add(comp1);
            await context.SaveChangesAsync();

            // Criteria
            var c1 = ScoringCriteria.Create(comp1.Id, "Ý tưởng & Sáng tạo", 10, 0.35m, "Tính độc đáo và thông điệp của tác phẩm");
            var c2 = ScoringCriteria.Create(comp1.Id, "Kỹ thuật & Bố cục", 10, 0.35m, "Kỹ thuật vẽ, phối màu và cấu trúc bố cục");
            var c3 = ScoringCriteria.Create(comp1.Id, "Cảm xúc & Thẩm mỹ", 10, 0.30m, "Sức hút nghệ thuật và biểu cảm");
            context.ScoringCriterias.AddRange(c1, c2, c3);
            await context.SaveChangesAsync();

            // Entries
            var e1 = CompetitionEntry.Create(comp1.Id, p1.Id, s1.Id, "COMP-2026-001-E01");
            var e2 = CompetitionEntry.Create(comp1.Id, p2.Id, s2.Id, "COMP-2026-001-E02");
            var e3 = CompetitionEntry.Create(comp1.Id, p3.Id, s3.Id, "COMP-2026-001-E03");
            context.CompetitionEntries.AddRange(e1, e2, e3);
            await context.SaveChangesAsync();

            // Scores with real Staff JudgeId
            var sc1 = EntryScore.Create(e1.Id, judgeStaffId, c1.Id, 9.5m, 0.35m, "Ý tưởng bứt phá, đậm chất sử thi");
            var sc2 = EntryScore.Create(e1.Id, judgeStaffId, c2.Id, 9.0m, 0.35m, "Chất sơn dầu phối rất dày và tinh tế");
            var sc3 = EntryScore.Create(e1.Id, judgeStaffId, c3.Id, 9.2m, 0.30m, "Cảm xúc dạt dào");

            var sc4 = EntryScore.Create(e2.Id, judgeStaffId, c1.Id, 8.5m, 0.35m, "Nét vẽ hoài niệm rất đẹp");
            var sc5 = EntryScore.Create(e2.Id, judgeStaffId, c2.Id, 8.8m, 0.35m, "Kỹ thuật màu nước loang điêu luyện");
            var sc6 = EntryScore.Create(e2.Id, judgeStaffId, c3.Id, 8.5m, 0.30m, "Gợi cảm xúc bình yên");

            context.EntryScores.AddRange(sc1, sc2, sc3, sc4, sc5, sc6);

            // 7. Seed Exhibitions
            var ex1 = Exhibition.Create(
                title: "Triển Lãm Ánh Sáng & Nghệ Thuật Đương Đại 2026",
                code: "EX-2026-001",
                startDate: DateTimeOffset.UtcNow.AddDays(-2),
                endDate: DateTimeOffset.UtcNow.AddDays(28),
                venue: "ArtVerse Grand Gallery - Hà Nội",
                description: "Không gian trưng bày đặc biệt quy tụ hơn 20 kiệt tác xuất sắc nhất của các nghệ sĩ trẻ triển vọng."
            );
            ex1.Publish();
            context.Exhibitions.Add(ex1);
            await context.SaveChangesAsync();

            var ea1 = ExhibitionArtwork.Create(ex1.Id, p1.Id, 1);
            ea1.IncrementLike(); ea1.IncrementLike(); ea1.IncrementLike();
            var ea2 = ExhibitionArtwork.Create(ex1.Id, p2.Id, 2);
            ea2.IncrementLike(); ea2.IncrementLike();
            var ea3 = ExhibitionArtwork.Create(ex1.Id, p3.Id, 3);
            ea3.IncrementLike(); ea3.IncrementLike(); ea3.IncrementLike(); ea3.IncrementLike(); ea3.IncrementLike();
            var ea4 = ExhibitionArtwork.Create(ex1.Id, p4.Id, 4);

            context.ExhibitionArtworks.AddRange(ea1, ea2, ea3, ea4);

            // 8. Seed Academic Classes & Subjects
            var sub1 = new Subject { Code = "ART101", Name = "Hình Họa & Phối Cảnh Không Gian", Description = "Nền tảng dựng hình, giải phẫu học cơ thể và phối cảnh đa điểm tụ.", CreditHours = 3 };
            var sub2 = new Subject { Code = "ART201", Name = "Kỹ Thuật Sơn Dầu & Acrylic Chuyên Sâu", Description = "Kỹ thuật pha màu, phủ lớp Glazing và bố cục tranh khổ lớn.", CreditHours = 4 };
            var sub3 = new Subject { Code = "ART301", Name = "Concept Art & Digital Illustration", Description = "Sáng tác kỹ thuật số, thiết kế nhân vật và môi trường thế giới ảo.", CreditHours = 3 };

            context.Subjects.AddRange(sub1, sub2, sub3);
            await context.SaveChangesAsync();

            var cl1 = new Class { Code = "K2026-OIL-01", Name = "Hội Họa Sơn Dầu Chuyên Sâu K1", Year = 2026, Semester = 1, MaxStudents = 25 };
            var cl2 = new Class { Code = "K2026-DIG-01", Name = "Nghệ Thuật Số & Concept Art K1", Year = 2026, Semester = 1, MaxStudents = 30 };
            context.Classes.AddRange(cl1, cl2);
            await context.SaveChangesAsync();

            var en1 = new StudentEnrollment { ClassId = cl1.Id, StudentId = s1.Id, Status = "Active" };
            var en2 = new StudentEnrollment { ClassId = cl1.Id, StudentId = s2.Id, Status = "Active" };
            var en3 = new StudentEnrollment { ClassId = cl2.Id, StudentId = s3.Id, Status = "Active" };
            var en4 = new StudentEnrollment { ClassId = cl2.Id, StudentId = s4.Id, Status = "Active" };
            context.StudentEnrollments.AddRange(en1, en2, en3, en4);

            await context.SaveChangesAsync();
        }
    }
}
