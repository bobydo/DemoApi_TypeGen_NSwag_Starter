using Demo.Api.Data;
using Demo.Api.Data.Entities;
using Demo.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api.Services;

public class StudentService : IStudentService
{
    private readonly ApplicationDbContext _context;

    public StudentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<StudentDto>> GetStudentsAsync()
    {
        var students = await _context.Students
            .AsNoTracking()
            .Select(s => new StudentDto
            {
                StudentId = s.StudentId,
                StudentNo = s.StudentNo,
                Name = s.Name,
                Active = s.Active
            })
            .ToListAsync();

        return students;
    }

    public async Task<StudentDto?> GetStudentByIdAsync(int id)
    {
        var student = await _context.Students
            .AsNoTracking()
            .Where(s => s.StudentId == id)
            .Select(s => new StudentDto
            {
                StudentId = s.StudentId,
                StudentNo = s.StudentNo,
                Name = s.Name,
                Active = s.Active
            })
            .FirstOrDefaultAsync();

        return student;
    }

    public async Task<StudentDto> CreateStudentAsync(CreateStudentRequest request)
    {
        // Create student with addresses in a single transaction
        var student = new Student
        {
            StudentNo = request.StudentNo,
            Name = request.Name,
            Active = request.Active,
            Addresses = request.Addresses.Select(a => new Address
            {
                Street = a.Street,
                City = a.City,
                Province = a.Province,
                PostalCode = a.PostalCode,
                Country = a.Country
            }).ToList()
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return new StudentDto
        {
            StudentId = student.StudentId,
            StudentNo = student.StudentNo,
            Name = student.Name,
            Active = student.Active
        };
    }

    public async Task<StudentDto?> UpdateStudentAsync(int id, StudentDto studentDto)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return null;

        student.StudentNo = studentDto.StudentNo;
        student.Name = studentDto.Name;
        student.Active = studentDto.Active;

        await _context.SaveChangesAsync();

        return new StudentDto
        {
            StudentId = student.StudentId,
            StudentNo = student.StudentNo,
            Name = student.Name,
            Active = student.Active
        };
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return false;

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return true;
    }
}