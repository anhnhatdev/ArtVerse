using ArtVerse.Application.Students.Commands;
using ArtVerse.Application.Students.DTOs;
using ArtVerse.Application.Students.Queries;
using ArtVerse.Application.Paintings.Queries;
using ArtVerse.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtVerse.Web.Controllers;

public class StudentsController : Controller
{
    private readonly IMediator _mediator;

    public StudentsController(IMediator mediator) => _mediator = mediator;

    // GET: /Students
    [AllowAnonymous]
    public async Task<IActionResult> Index(string? search, int page = 1)
    {
        const int pageSize = 8;
        var (students, totalCount) = await _mediator.Send(new GetStudentsQuery(search, page, pageSize));

        ViewBag.Search = search;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        ViewBag.TotalCount = totalCount;

        return View(students);
    }

    // GET: /Students/Details/{id}
    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid id)
    {
        var student = await _mediator.Send(new GetStudentByIdQuery(id));
        if (student == null) return NotFound();

        var paintings = await _mediator.Send(new GetStudentPaintingsQuery(id));
        ViewBag.Paintings = paintings;

        return View(student);
    }

    // GET: /Students/Create
    [Authorize(Roles = "Admin,Principal,Manager,Staff")]
    public IActionResult Create() => View();

    // POST: /Students/Create
    [HttpPost]
    [Authorize(Roles = "Admin,Principal,Manager,Staff")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string fullName, string email, string? phone, DateOnly? dateOfBirth, Gender? gender)
    {
        try
        {
            var dto = new CreateStudentDto(fullName, email, phone, dateOfBirth, gender);
            var studentId = await _mediator.Send(new CreateStudentCommand(dto));
            TempData["Success"] = "Tạo mới học viên thành công!";
            return RedirectToAction(nameof(Details), new { id = studentId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }
}
