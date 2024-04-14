using ArtVerse.Application.Common.Interfaces;
using ArtVerse.Application.Students.Queries;
using ArtVerse.Infrastructure.Data;
using ArtVerse.Infrastructure.Data.Seed;
using ArtVerse.Infrastructure.Repositories;
using ArtVerse.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ===================================================
// 1. CẤU HÌNH DATABASE (Linh hoạt SQLite hoặc SQL Server)
// ===================================================
var dbProvider = builder.Configuration.GetValue<string>("DatabaseProvider") ?? "Sqlite";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (dbProvider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
    {
        var sqlConn = builder.Configuration.GetConnectionString("SqlServerConnection")
            ?? throw new InvalidOperationException("Connection string 'SqlServerConnection' not found.");
        options.UseSqlServer(sqlConn);
    }
    else
    {
        var sqliteConn = builder.Configuration.GetConnectionString("SqliteConnection")
            ?? "Data Source=artverse.db";
        options.UseSqlite(sqliteConn, b => b.MigrationsAssembly("ArtVerse.Infrastructure"));
    }
});

// ===================================================
// 2. CẤU HÌNH IDENTITY (User, Role)
// ===================================================
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.LogoutPath = "/Auth/Logout";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// ===================================================
// 3. DUAL AUTHENTICATION (Cookie for MVC + JWT for REST API)
// ===================================================
var jwtSecret = builder.Configuration["JwtSettings:Secret"] ?? "ArtVerse_Super_Secret_Key_For_JWT_Authentication_2026_Enterprise_Platform!";
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"] ?? "ArtVerseApi";
var jwtAudience = builder.Configuration["JwtSettings:Audience"] ?? "ArtVerseClient";

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "JWT_OR_COOKIE";
    options.DefaultChallengeScheme = "JWT_OR_COOKIE";
})
.AddPolicyScheme("JWT_OR_COOKIE", "JWT or Cookie", options =>
{
    options.ForwardDefaultSelector = context =>
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return JwtBearerDefaults.AuthenticationScheme;

        if (context.Request.Path.StartsWithSegments("/api"))
            return JwtBearerDefaults.AuthenticationScheme;

        return IdentityConstants.ApplicationScheme;
    };
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        RoleClaimType = ClaimTypes.Role,
        NameClaimType = ClaimTypes.Name,
        ClockSkew = TimeSpan.Zero
    };
});

// ===================================================
// 4. CẤU HÌNH CORS CHO FRONTEND NEXT.JS / REACT
// ===================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllDev", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3001", "http://localhost:5173", "http://127.0.0.1:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ===================================================
// 5. ĐĂNG KÝ DEPENDENCY INJECTION (Repositories & Services)
// ===================================================
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IPaintingRepository, PaintingRepository>();
builder.Services.AddScoped<ICompetitionRepository, CompetitionRepository>();
builder.Services.AddScoped<IExhibitionRepository, ExhibitionRepository>();
builder.Services.AddScoped<IAcademicRepository, AcademicRepository>();
builder.Services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// ===================================================
// 6. CẤU HÌNH MEDIATOR (CQRS)
// ===================================================
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetStudentsQuery).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

// ===================================================
// 7. CẤU HÌNH MVC + CONTROLLERS + SWAGGER OPENAPI
// ===================================================
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ArtVerse Enterprise REST API",
        Version = "v1",
        Description = "Nền tảng Quản lý Học viện Nghệ thuật, Thư viện Tranh & Triển lãm Số (Clean Architecture .NET 8)"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Nhập mã JWT Token theo định dạng: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ===================================================
// 8. CẤU HÌNH AUTHORIZATION POLICIES (RBAC)
// ===================================================
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ManagerOrAbove", policy => policy.RequireRole("Admin", "Principal", "Manager"));
    options.AddPolicy("StaffOrAbove", policy => policy.RequireRole("Admin", "Principal", "Manager", "Staff"));
    options.AddPolicy("StudentOnly", policy => policy.RequireRole("Student"));
});

var app = builder.Build();

// Tự động khởi tạo Database và Seed dữ liệu ban đầu
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();
        await DbSeeder.SeedAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Lỗi trong quá trình khởi tạo Database.");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ArtVerse Enterprise API v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowAllDev");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
