using Demo.Api.Models;

namespace Demo.Api.Services;

public class StudentService : IStudentService
{
    public Task<List<StudentDto>> GetStudentsAsync()
    {
        var students = new List<StudentDto>
        {
            new() { Id = 1, Name = "Alice", Active = true },
            new() { Id = 2, Name = "Bob", Active = false }
        };

        return Task.FromResult(students);
    }
}