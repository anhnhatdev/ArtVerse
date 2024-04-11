using ArtVerse.Application.Admin.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtVerse.Web.Controllers;

[Authorize(Roles = "Admin,Principal,Manager")]
public class AdminController : Controller
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator) => _mediator = mediator;

    // GET: /Admin/Dashboard
    public async Task<IActionResult> Dashboard()
    {
        var analytics = await _mediator.Send(new GetDashboardStatsQuery());
        return View(analytics);
    }
}
