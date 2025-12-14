using Demo.Api.Models;

namespace Demo.Api.Services;

public interface IStudentService
{
    Task<List<StudentDto>> GetStudentsAsync();
}