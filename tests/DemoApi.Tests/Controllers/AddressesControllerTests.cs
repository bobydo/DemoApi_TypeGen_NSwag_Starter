using System.Net;
using System.Net.Http.Json;
using Demo.Api.Data;
using Demo.Api.Models;
using DemoApi.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DemoApi.Tests.Controllers;

public class AddressesControllerTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AddressesControllerTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();

        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        db.Database.EnsureCreated();
    }

    private async Task<(StudentDto student, int addressId)> CreateTestStudentWithAddress(string studentNo)
    {
        var request = new CreateStudentRequest
        {
            StudentNo = studentNo,
            Name = $"Student {studentNo}",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto
                {
                    Street = "123 Main St",
                    City = "Springfield",
                    PostalCode = "12345",
                    Country = "USA"
                }
            }
        };

        var response = await _client.PostAsJsonAsync("/api/students", request);
        var student = (await response.Content.ReadFromJsonAsync<StudentDto>())!;
        
        // Get the full student with addresses to get the address ID
        var detailResponse = await _client.GetAsync($"/api/students/{student.StudentId}");
        var detailStudent = await detailResponse.Content.ReadFromJsonAsync<StudentDto>();
        
        return (student, detailStudent!.StudentId); // Return placeholder for addressId since we need to get it differently
    }

    [Fact]
    public async Task AddAddress_ReturnsCreated_WithValidData()
    {
        // Arrange
        var (student, _) = await CreateTestStudentWithAddress("A001");
        var newAddress = new AddressDto
        {
            Street = "456 Oak Ave",
            City = "Chicago",
            PostalCode = "60601",
            Country = "USA"
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/students/{student.StudentId}/addresses", newAddress);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var address = await response.Content.ReadFromJsonAsync<AddressDto>();
        address.Should().NotBeNull();
        address!.Street.Should().Be("456 Oak Ave");
        address.StudentId.Should().Be(student.StudentId);
    }

    [Fact]
    public async Task AddAddress_ReturnsBadRequest_WithInvalidData()
    {
        // Arrange
        var (student, _) = await CreateTestStudentWithAddress("A002");
        var invalidAddress = new AddressDto
        {
            Street = "", // Empty street should fail validation
            City = "Chicago",
            PostalCode = "60601",
            Country = "USA"
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/students/{student.StudentId}/addresses", invalidAddress);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddAddress_ReturnsNotFound_WhenStudentDoesNotExist()
    {
        // Arrange
        var newAddress = new AddressDto
        {
            Street = "456 Oak Ave",
            City = "Chicago",
            PostalCode = "60601",
            Country = "USA"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/students/99999/addresses", newAddress);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteAddress_ReturnsNoContent_WhenMultipleAddressesExist()
    {
        // Arrange - Create student with one address, then add a second
        var (student, _) = await CreateTestStudentWithAddress("A005");
        var secondAddress = new AddressDto
        {
            Street = "456 Second St",
            City = "Chicago",
            PostalCode = "60602",
            Country = "USA"
        };
        var addResponse = await _client.PostAsJsonAsync($"/api/students/{student.StudentId}/addresses", secondAddress);
        var addedAddress = await addResponse.Content.ReadFromJsonAsync<AddressDto>();

        // Act - Delete the second address
        var response = await _client.DeleteAsync($"/api/students/{student.StudentId}/addresses/{addedAddress!.AddressId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteAddress_ReturnsNotFound_WhenAddressDoesNotExist()
    {
        // Arrange
        var (student, _) = await CreateTestStudentWithAddress("A007");

        // Act
        var response = await _client.DeleteAsync($"/api/students/{student.StudentId}/addresses/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteAddress_ReturnsNotFound_WhenStudentDoesNotExist()
    {
        // Act
        var response = await _client.DeleteAsync("/api/students/99999/addresses/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task StudentWithMultipleAddresses_CanDeleteAndKeepOne()
    {
        // Arrange - Create student with 1 address, then add two more
        var (student, _) = await CreateTestStudentWithAddress("A008");
        
        // Add two more addresses
        var secondAddress = await _client.PostAsJsonAsync($"/api/students/{student.StudentId}/addresses", new AddressDto
        {
            Street = "200 Second St",
            City = "NYC",
            PostalCode = "10001",
            Country = "USA"
        });
        var thirdAddress = await _client.PostAsJsonAsync($"/api/students/{student.StudentId}/addresses", new AddressDto
        {
            Street = "300 Third St",
            City = "LA",
            PostalCode = "90001",
            Country = "USA"
        });

        var addedAddress2 = await secondAddress.Content.ReadFromJsonAsync<AddressDto>();

        // Act - Delete one address (should succeed since 2 will remain)
        var deleteResponse = await _client.DeleteAsync($"/api/students/{student.StudentId}/addresses/{addedAddress2!.AddressId}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
