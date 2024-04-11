using ArtVerse.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArtVerse.Web.Controllers;

public class AuthController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    // GET: /Auth/Login
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    // POST: /Auth/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password, bool rememberMe, string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError("", "Vui lòng nhập đầy đủ Email và Mật khẩu.");
            return View();
        }

        var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            TempData["Success"] = $"Đăng nhập thành công với tài khoản {email}!";
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác.");
        return View();
    }

    // POST: /Auth/QuickLogin?role=Admin
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> QuickLogin(string role, string? returnUrl = null)
    {
        string email = role.ToLower() switch
        {
            "admin" => "admin@artverse.com",
            "principal" => "principal@artverse.com",
            "staff" => "staff@artverse.com",
            "student" => "student@artverse.com",
            _ => "admin@artverse.com"
        };

        var user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            await _signInManager.SignInAsync(user, isPersistent: true);
            TempData["Success"] = $"Đã chuyển nhanh sang vai trò: {role.ToUpper()} ({user.FullName})";
        }
        else
        {
            TempData["Error"] = $"Không tìm thấy tài khoản {email}.";
        }

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    // POST: /Auth/Logout
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        TempData["Success"] = "Bạn đã đăng xuất khỏi hệ thống thành công.";
        return RedirectToAction("Index", "Home");
    }

    // GET: /Auth/AccessDenied
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
