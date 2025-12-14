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

    /// <summary>
    /// Get all students
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<StudentDto>>> GetStudents()
    {
        return Ok(await _service.GetStudentsAsync());
    }

    /// <summary>
    /// Get a specific student by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetStudent(int id)
    {
        var student = await _service.GetStudentByIdAsync(id);
        if (student == null)
            return NotFound();

        return Ok(student);
    }

    /// <summary>
    /// Create a new student with at least one address
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<StudentDto>> CreateStudent(CreateStudentRequest request)
    {
        // Validate at least one address is provided
        if (request.Addresses == null || request.Addresses.Count == 0)
        {
            return BadRequest("At least one address is required");
        }

        var created = await _service.CreateStudentAsync(request);
        return CreatedAtAction(nameof(GetStudent), new { id = created.StudentId }, created);
    }

    /// <summary>
    /// Update an existing student
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<StudentDto>> UpdateStudent(int id, StudentDto studentDto)
    {
        var updated = await _service.UpdateStudentAsync(id, studentDto);
        if (updated == null)
            return NotFound();

        return Ok(updated);
    }

    /// <summary>
    /// Delete a student
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var deleted = await _service.DeleteStudentAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}