using Microsoft.AspNetCore.Authentication.JwtBearer;
using ArtVerse.Application.Common.Interfaces;
using ArtVerse.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArtVerse.Web.Controllers.Api;

[ApiController]
[Route("api/v1/auth")]
public class AuthApiController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenService _jwtService;

    public AuthApiController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }

    public record LoginRequest(string Email, string Password);
    public record QuickLoginRequest(string Role);

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { success = false, message = "Vui lòng cung cấp email và mật khẩu." });

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !user.IsActive)
            return Unauthorized(new { success = false, message = "Tài khoản hoặc mật khẩu không chính xác." });

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            return Unauthorized(new { success = false, message = "Tài khoản hoặc mật khẩu không chính xác." });

        var roles = await _userManager.GetRolesAsync(user);
        var primaryRole = roles.FirstOrDefault() ?? "Student";
        var token = _jwtService.GenerateToken(user.Id, user.Email!, user.FullName, primaryRole);

        return Ok(new
        {
            success = true,
            message = "Đăng nhập thành công!",
            data = new
            {
                token,
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    fullName = user.FullName,
                    role = primaryRole
                }
            }
        });
    }

    [HttpPost("quick-login")]
    [AllowAnonymous]
    public async Task<IActionResult> QuickLogin([FromBody] QuickLoginRequest request)
    {
        string email = request.Role.ToLower() switch
        {
            "admin" => "admin@artverse.com",
            "principal" => "principal@artverse.com",
            "staff" => "staff@artverse.com",
            "student" => "student@artverse.com",
            _ => "admin@artverse.com"
        };

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return NotFound(new { success = false, message = $"Không tìm thấy tài khoản {email}." });

        var roles = await _userManager.GetRolesAsync(user);
        var primaryRole = roles.FirstOrDefault() ?? "Student";
        var token = _jwtService.GenerateToken(user.Id, user.Email!, user.FullName, primaryRole);

        return Ok(new
        {
            success = true,
            message = $"Đã chuyển vai trò sang: {primaryRole.ToUpper()}",
            data = new
            {
                token,
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    fullName = user.FullName,
                    role = primaryRole
                }
            }
        });
    }

    [HttpGet("me")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(new
        {
            success = true,
            data = new
            {
                id = user.Id,
                email = user.Email,
                fullName = user.FullName,
                role = roles.FirstOrDefault() ?? "Student"
            }
        });
    }
}

