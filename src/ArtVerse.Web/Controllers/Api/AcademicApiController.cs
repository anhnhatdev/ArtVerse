using Microsoft.AspNetCore.Authentication.JwtBearer;
using ArtVerse.Application.Academic.Commands;
using ArtVerse.Application.Academic.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtVerse.Web.Controllers.Api;

[ApiController]
[Route("api/v1/academic")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Principal,Manager,Staff")]
public class AcademicApiController : ControllerBase
{
    private readonly IMediator _mediator;

    public AcademicApiController(IMediator mediator) => _mediator = mediator;

    [HttpGet("classes")]
    public async Task<IActionResult> GetClasses()
    {
        var items = await _mediator.Send(new GetClassesQuery());
        return Ok(new { success = true, data = items });
    }

    [HttpGet("classes/{id:guid}")]
    public async Task<IActionResult> GetClassById(Guid id)
    {
        var (classInfo, students) = await _mediator.Send(new GetClassDetailsQuery(id));
        if (classInfo == null) return NotFound(new { success = false, message = "Không tìm thấy lớp học." });
        return Ok(new { success = true, data = new { classInfo, students } });
    }

    [HttpPost("classes")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Principal,Manager")]
    public async Task<IActionResult> CreateClass([FromBody] CreateClassApiRequest request)
    {
        var id = await _mediator.Send(new CreateClassCommand(request.Code, request.Name, request.Year, request.Semester, request.MaxStudents));
        return Ok(new { success = true, message = "Đã tạo lớp học thành công!", data = new { id } });
    }

    [HttpPost("classes/{id:guid}/enroll")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Principal,Manager")]
    public async Task<IActionResult> EnrollStudent(Guid id, [FromBody] EnrollStudentApiRequest request)
    {
        var enrollmentId = await _mediator.Send(new EnrollStudentCommand(id, request.StudentId));
        return Ok(new { success = true, message = "Ghi danh học viên vào lớp thành công!", data = new { enrollmentId } });
    }

    [HttpGet("subjects")]
    public async Task<IActionResult> GetSubjects()
    {
        var items = await _mediator.Send(new GetSubjectsQuery());
        return Ok(new { success = true, data = items });
    }

    [HttpPost("subjects")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Principal,Manager")]
    public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectApiRequest request)
    {
        var id = await _mediator.Send(new CreateSubjectCommand(request.Code, request.Name, request.Description, request.CreditHours));
        return Ok(new { success = true, message = "Đã tạo môn học thành công!", data = new { id } });
    }

    public record CreateClassApiRequest(string Code, string Name, int Year, int Semester, int MaxStudents = 40);
    public record EnrollStudentApiRequest(Guid StudentId);
    public record CreateSubjectApiRequest(string Code, string Name, string? Description, int CreditHours = 3);
}

