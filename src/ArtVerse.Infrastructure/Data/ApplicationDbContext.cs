using ArtVerse.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArtVerse.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Staff> Staffs => Set<Staff>();
    public DbSet<Painting> Paintings => Set<Painting>();
    public DbSet<PaintingFile> PaintingFiles => Set<PaintingFile>();
    public DbSet<Competition> Competitions => Set<Competition>();
    public DbSet<CompetitionEntry> CompetitionEntries => Set<CompetitionEntry>();
    public DbSet<ScoringCriteria> ScoringCriterias => Set<ScoringCriteria>();
    public DbSet<EntryScore> EntryScores => Set<EntryScore>();
    public DbSet<Award> Awards => Set<Award>();
    public DbSet<Exhibition> Exhibitions => Set<Exhibition>();
    public DbSet<ExhibitionArtwork> ExhibitionArtworks => Set<ExhibitionArtwork>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Class> Classes => Set<Class>();
    public DbSet<StudentEnrollment> StudentEnrollments => Set<StudentEnrollment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ===== Đổi tên bảng Identity cho sạch hơn =====
        builder.Entity<ApplicationUser>().ToTable("Users");
        builder.Entity<IdentityRole<Guid>>().ToTable("Roles");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

        // ===== Global Query Filters (Soft Delete) =====
        // Áp dụng cho cả cha lẫn con để tránh warning EF10622
        builder.Entity<Student>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<StudentEnrollment>().HasQueryFilter(e => !e.IsDeleted);

        builder.Entity<Staff>().HasQueryFilter(e => !e.IsDeleted);

        builder.Entity<Painting>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<PaintingFile>().HasQueryFilter(e => !e.IsDeleted);

        builder.Entity<Competition>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<CompetitionEntry>().HasQueryFilter(e => !e.IsDeleted);

        builder.Entity<Exhibition>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<ExhibitionArtwork>().HasQueryFilter(e => !e.IsDeleted);

        // ===== Decimal Precision (tránh warning SQL Server) =====
        builder.Entity<Painting>()
            .Property(p => p.BasePrice)
            .HasColumnType("decimal(18,2)");

        builder.Entity<Exhibition>()
            .Property(e => e.TicketPrice)
            .HasColumnType("decimal(18,2)");

        builder.Entity<ExhibitionArtwork>()
            .Property(a => a.AskingPrice)
            .HasColumnType("decimal(18,2)");

        builder.Entity<Award>()
            .Property(a => a.PrizeAmount)
            .HasColumnType("decimal(18,2)");

        builder.Entity<EntryScore>()
            .Property(s => s.Score)
            .HasColumnType("decimal(5,2)");

        builder.Entity<EntryScore>()
            .Property(s => s.Weight)
            .HasColumnType("decimal(5,4)");

        builder.Entity<ScoringCriteria>()
            .Property(s => s.MaxScore)
            .HasColumnType("decimal(5,2)");

        builder.Entity<ScoringCriteria>()
            .Property(s => s.Weight)
            .HasColumnType("decimal(5,4)");
    }
}
