using Demo.Api.Models;

namespace Demo.Api.Services;

public interface IStudentService
{
    Task<List<StudentDto>> GetStudentsAsync();
    Task<StudentDto?> GetStudentByIdAsync(int id);
    Task<StudentDto> CreateStudentAsync(StudentDto studentDto);
    Task<StudentDto?> UpdateStudentAsync(int id, StudentDto studentDto);
    Task<bool> DeleteStudentAsync(int id);
}