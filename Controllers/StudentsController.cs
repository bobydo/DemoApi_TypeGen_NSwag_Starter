using Demo.Api.Models;
using Demo.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _service;

    public StudentsController(IStudentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<StudentDto>>> GetStudents()
    {
        return Ok(await _service.GetStudentsAsync());
    }
}