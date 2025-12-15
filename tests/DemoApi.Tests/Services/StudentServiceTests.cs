using Demo.Api.Data.Entities;
using Demo.Api.Services;
using DemoApi.Tests.Helpers;

namespace DemoApi.Tests.Services;

public class StudentServiceTests
{
    [Fact]
    public async Task GetStudentsAsync_ReturnsAllStudents()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        using var context = factory.CreateContext();
        
        context.Students.AddRange(
            new Student { StudentNo = "S001", Name = "John Doe", Active = true },
            new Student { StudentNo = "S002", Name = "Jane Smith", Active = true }
        );
        await context.SaveChangesAsync();

        var service = new StudentService(context);

        // Act
        var result = await service.GetStudentsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(s => s.StudentNo == "S001");
        result.Should().Contain(s => s.StudentNo == "S002");
    }

    [Fact]
    public async Task GetStudentByIdAsync_WithValidId_ReturnsStudent()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        using var context = factory.CreateContext();
        
        var student = new Student { StudentNo = "S001", Name = "John Doe", Active = true };
        context.Students.Add(student);
        await context.SaveChangesAsync();

        var service = new StudentService(context);

        // Act
        var result = await service.GetStudentByIdAsync(student.StudentId);

        // Assert
        result.Should().NotBeNull();
        result!.StudentNo.Should().Be("S001");
        result.Name.Should().Be("John Doe");
    }
}
