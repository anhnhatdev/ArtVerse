using ArtVerse.Application.Academic.Commands;
using ArtVerse.Application.Academic.Queries;
using ArtVerse.Application.Students.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArtVerse.Web.Controllers;

[Authorize(Roles = "Admin,Principal,Manager,Staff")]
public class AcademicController : Controller
{
    private readonly IMediator _mediator;

    public AcademicController(IMediator mediator) => _mediator = mediator;

    // GET: /Academic/Classes
    public async Task<IActionResult> Classes()
    {
        var classes = await _mediator.Send(new GetClassesQuery());
        return View(classes);
    }

    // GET: /Academic/ClassDetails/{id}
    public async Task<IActionResult> ClassDetails(Guid id)
    {
        var (classInfo, students) = await _mediator.Send(new GetClassDetailsQuery(id));
        if (classInfo == null) return NotFound();

        var (allStudents, _) = await _mediator.Send(new GetStudentsQuery(null, 1, 100));
        ViewBag.AvailableStudents = new SelectList(allStudents, "Id", "FullName");

        ViewBag.ClassInfo = classInfo;
        return View(students);
    }

    // POST: /Academic/CreateClass
    [HttpPost]
    [Authorize(Roles = "Admin,Principal,Manager")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateClass(string code, string name, int year, int semester, int maxStudents)
    {
        try
        {
            await _mediator.Send(new CreateClassCommand(code, name, year, semester, maxStudents));
            TempData["Success"] = "Đã khởi tạo lớp học mới thành công!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Classes));
    }

    // POST: /Academic/EnrollStudent
    [HttpPost]
    [Authorize(Roles = "Admin,Principal,Manager")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EnrollStudent(Guid classId, Guid studentId)
    {
        try
        {
            await _mediator.Send(new EnrollStudentCommand(classId, studentId));
            TempData["Success"] = "Đã phân bổ học viên vào lớp học thành công!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(ClassDetails), new { id = classId });
    }

    // GET: /Academic/Subjects
    public async Task<IActionResult> Subjects()
    {
        var subjects = await _mediator.Send(new GetSubjectsQuery());
        return View(subjects);
    }

    // POST: /Academic/CreateSubject
    [HttpPost]
    [Authorize(Roles = "Admin,Principal,Manager")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateSubject(string code, string name, string? description, int creditHours)
    {
        try
        {
            await _mediator.Send(new CreateSubjectCommand(code, name, description, creditHours));
            TempData["Success"] = "Đã thêm môn học mới vào khung chương trình đào tạo!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Subjects));
    }
}
