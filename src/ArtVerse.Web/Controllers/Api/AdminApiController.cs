using ArtVerse.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtVerse.Web.Controllers.Api;

[ApiController]
[Route("api/v1/admin")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Principal,Manager")]
public class AdminApiController : ControllerBase
{
    private readonly IAnalyticsRepository _analyticsRepo;

    public AdminApiController(IAnalyticsRepository analyticsRepo) => _analyticsRepo = analyticsRepo;

    [HttpGet("dashboard-stats")]
    public async Task<IActionResult> GetDashboardStats()
    {
        var stats = await _analyticsRepo.GetDashboardStatsAsync();
        return Ok(new { success = true, data = stats });
    }
}
