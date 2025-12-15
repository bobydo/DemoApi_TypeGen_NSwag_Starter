using System.Net;
using System.Net.Http.Json;
using Demo.Api.Data;
using Demo.Api.Models;
using DemoApi.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DemoApi.Tests.Controllers;

public class StudentsControllerTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory _factory;

    public StudentsControllerTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();

        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }

    [Fact]
    public async Task GetAllStudents_ReturnsOkWithEmptyList_WhenNoStudents()
    {
        // Arrange - Clear any existing students from seed data
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Students.RemoveRange(db.Students);
        await db.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("/api/students");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var students = await response.Content.ReadFromJsonAsync<List<StudentDto>>();
        students.Should().NotBeNull();
        students.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateStudent_ReturnsCreated_WithValidData()
    {
        // Arrange
        var request = new CreateStudentRequest
        {
            StudentNo = "S00001",
            Name = "John Doe",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto
                {
                    Street = "123 Main St",
                    City = "Springfield",
                    PostalCode = "12345",
                    Country = "USA",
                    Province = "IL"
                }
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/students", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var student = await response.Content.ReadFromJsonAsync<StudentDto>();
        student.Should().NotBeNull();
        student!.StudentNo.Should().Be("S00001");
        student.Name.Should().Be("John Doe");
    }

    [Fact]
    public async Task CreateStudent_ReturnsBadRequest_WithInvalidStudentNo()
    {
        // Arrange
        var request = new CreateStudentRequest
        {
            StudentNo = "INVALID", // Should start with 'S' followed by 3 digits
            Name = "John Doe",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = "123 Main St", City = "Springfield", PostalCode = "12345", Country = "USA" }
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/students", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateStudent_ReturnsBadRequest_WithNoAddresses()
    {
        // Arrange
        var request = new CreateStudentRequest
        {
            StudentNo = "S001",
            Name = "John Doe",
            Active = true,
            Addresses = new List<AddressDto>() // Empty addresses
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/students", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetStudentById_ReturnsOk_WhenStudentExists()
    {
        // Arrange - Create a student first
        var createRequest = new CreateStudentRequest
        {
            StudentNo = "S00002",
            Name = "Jane Smith",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = "456 Oak Ave", City = "Chicago", PostalCode = "60601", Country = "USA", Province = "IL" }
            }
        };
        var createResponse = await _client.PostAsJsonAsync("/api/students", createRequest);
        var createdStudent = await createResponse.Content.ReadFromJsonAsync<StudentDto>();

        // Act
        var response = await _client.GetAsync($"/api/students/{createdStudent!.StudentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var student = await response.Content.ReadFromJsonAsync<StudentDto>();
        student.Should().NotBeNull();
        student!.StudentId.Should().Be(createdStudent.StudentId);
        student.StudentNo.Should().Be("S00002");
    }

    [Fact]
    public async Task GetStudentById_ReturnsNotFound_WhenStudentDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/api/students/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateStudent_ReturnsNoContent_WithValidData()
    {
        // Arrange - Create a student first
        var createRequest = new CreateStudentRequest
        {
            StudentNo = "S00003",
            Name = "Bob Johnson",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = "789 Pine Rd", City = "Boston", PostalCode = "02101", Country = "USA", Province = "MA" }
            }
        };
        var createResponse = await _client.PostAsJsonAsync("/api/students", createRequest);
        var createdStudent = await createResponse.Content.ReadFromJsonAsync<StudentDto>();

        var updateRequest = new StudentDto
        {
            StudentId = createdStudent!.StudentId,
            StudentNo = "S00003",
            Name = "Bob Johnson Updated",
            Active = true
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/students/{createdStudent.StudentId}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Verify the update
        var getResponse = await _client.GetAsync($"/api/students/{createdStudent.StudentId}");
        var updatedStudent = await getResponse.Content.ReadFromJsonAsync<StudentDto>();
        updatedStudent!.Name.Should().Be("Bob Johnson Updated");
    }

    [Fact]
    public async Task DeleteStudent_ReturnsNoContent_WhenStudentExists()
    {
        // Arrange - Create a student first
        var createRequest = new CreateStudentRequest
        {
            StudentNo = "S00004",
            Name = "Alice Brown",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = "321 Elm St", City = "Seattle", PostalCode = "98101", Country = "USA", Province = "WA" }
            }
        };
        var createResponse = await _client.PostAsJsonAsync("/api/students", createRequest);
        var createdStudent = await createResponse.Content.ReadFromJsonAsync<StudentDto>();

        // Act
        var response = await _client.DeleteAsync($"/api/students/{createdStudent!.StudentId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verify the student is deleted
        var getResponse = await _client.GetAsync($"/api/students/{createdStudent.StudentId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteStudent_ReturnsNotFound_WhenStudentDoesNotExist()
    {
        // Act
        var response = await _client.DeleteAsync("/api/students/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllStudents_ReturnsMultipleStudents_AfterCreation()
    {
        // Arrange - Create multiple students
        var students = new[]
        {
            new CreateStudentRequest
            {
                StudentNo = "S005",
                Name = "Student Five",
                Active = true,
                Addresses = new List<AddressDto>
                {
                    new AddressDto { Street = "100 First St", City = "NYC", PostalCode = "10001", Country = "USA" }
                }
            },
            new CreateStudentRequest
            {
                StudentNo = "S006",
                Name = "Student Six",
                Active = true,
                Addresses = new List<AddressDto>
                {
                    new AddressDto { Street = "200 Second St", City = "LA", PostalCode = "90001", Country = "USA" }
                }
            }
        };

        foreach (var student in students)
        {
            await _client.PostAsJsonAsync("/api/students", student);
        }

        // Act
        var response = await _client.GetAsync("/api/students");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var allStudents = await response.Content.ReadFromJsonAsync<List<StudentDto>>();
        allStudents.Should().NotBeNull();
        allStudents!.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}
