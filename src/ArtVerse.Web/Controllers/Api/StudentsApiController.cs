using ArtVerse.Application.Students.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArtVerse.Web.Controllers.Api;

[ApiController]
[Route("api/v1/students")]
public class StudentsApiController : ControllerBase
{
    private readonly IMediator _mediator;

    public StudentsApiController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetStudents([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 12)
    {
        var (items, totalCount) = await _mediator.Send(new GetStudentsQuery(search, page, pageSize));
        return Ok(new
        {
            success = true,
            data = new
            {
                items,
                totalCount,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            }
        });
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetStudentById(Guid id)
    {
        var student = await _mediator.Send(new GetStudentByIdQuery(id));
        if (student == null) return NotFound(new { success = false, message = "Không tìm thấy hồ sơ học viên." });
        return Ok(new { success = true, data = student });
    }
}
