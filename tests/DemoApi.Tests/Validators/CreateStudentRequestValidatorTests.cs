using Demo.Api.Models;
using Demo.Api.Services;
using Demo.Api.Validators;
using DemoApi.Tests.Helpers;
using FluentValidation.TestHelper;

namespace DemoApi.Tests.Validators;

public class CreateStudentRequestValidatorTests
{
    private readonly CreateStudentRequestValidator _validator;

    public CreateStudentRequestValidatorTests()
    {
        _validator = new CreateStudentRequestValidator();
    }

    #region Validator Unit Tests

    [Fact]
    public void Should_HaveError_When_StudentNo_IsEmpty()
    {
        // Arrange
        var model = new CreateStudentRequest
        {
            StudentNo = "",
            Name = "John Doe",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = "123 Main St", City = "Toronto", Province = "ON", PostalCode = "M5H2N2", Country = "Canada" }
            }
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StudentNo)
            .WithErrorMessage("Student number is required");
    }

    [Fact]
    public void Should_HaveError_When_StudentNo_ExceedsMaxLength()
    {
        // Arrange
        var model = new CreateStudentRequest
        {
            StudentNo = "S12345678", // 9 characters - exceeds 8
            Name = "John Doe",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = "123 Main St", City = "Toronto", Province = "ON", PostalCode = "M5H2N2", Country = "Canada" }
            }
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StudentNo)
            .WithErrorMessage("Student number cannot exceed 8 characters");
    }

    [Fact]
    public void Should_HaveError_When_Name_IsEmpty()
    {
        // Arrange
        var model = new CreateStudentRequest
        {
            StudentNo = "S001",
            Name = "",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = "123 Main St", City = "Toronto", Province = "ON", PostalCode = "M5H2N2", Country = "Canada" }
            }
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name is required");
    }

    [Fact]
    public void Should_HaveError_When_Name_ExceedsMaxLength()
    {
        // Arrange
        var model = new CreateStudentRequest
        {
            StudentNo = "S001",
            Name = new string('A', 101), // 101 characters - exceeds 100
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = "123 Main St", City = "Toronto", Province = "ON", PostalCode = "M5H2N2", Country = "Canada" }
            }
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name cannot exceed 100 characters");
    }

    [Fact]
    public void Should_HaveError_When_Addresses_IsNull()
    {
        // Arrange
        var model = new CreateStudentRequest
        {
            StudentNo = "S001",
            Name = "John Doe",
            Active = true,
            Addresses = null!
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Addresses)
            .WithErrorMessage("Addresses are required");
    }

    [Fact]
    public void Should_HaveError_When_Addresses_IsEmpty()
    {
        // Arrange
        var model = new CreateStudentRequest
        {
            StudentNo = "S001",
            Name = "John Doe",
            Active = true,
            Addresses = new List<AddressDto>()
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Addresses)
            .WithErrorMessage("At least one address is required");
    }

    [Fact]
    public void Should_HaveError_When_Address_Street_IsEmpty()
    {
        // Arrange
        var model = new CreateStudentRequest
        {
            StudentNo = "S001",
            Name = "John Doe",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = "", City = "Toronto", Province = "ON", PostalCode = "M5H2N2", Country = "Canada" }
            }
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor("Addresses[0].Street")
            .WithErrorMessage("Street is required");
    }

    [Fact]
    public void Should_HaveError_When_Address_Street_ExceedsMaxLength()
    {
        // Arrange
        var model = new CreateStudentRequest
        {
            StudentNo = "S001",
            Name = "John Doe",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = new string('A', 51), City = "Toronto", Province = "ON", PostalCode = "M5H2N2", Country = "Canada" }
            }
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor("Addresses[0].Street")
            .WithErrorMessage("Street cannot exceed 50 characters");
    }

    [Fact]
    public void Should_HaveError_When_Address_City_IsEmpty()
    {
        // Arrange
        var model = new CreateStudentRequest
        {
            StudentNo = "S001",
            Name = "John Doe",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = "123 Main St", City = "", Province = "ON", PostalCode = "M5H2N2", Country = "Canada" }
            }
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor("Addresses[0].City")
            .WithErrorMessage("City is required");
    }

    [Fact]
    public void Should_HaveError_When_Address_PostalCode_ExceedsMaxLength()
    {
        // Arrange
        var model = new CreateStudentRequest
        {
            StudentNo = "S001",
            Name = "John Doe",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = "123 Main St", City = "Toronto", Province = "ON", PostalCode = "M5H2N2AB", Country = "Canada" }
            }
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor("Addresses[0].PostalCode")
            .WithErrorMessage("Postal code cannot exceed 7 characters");
    }

    [Fact]
    public void Should_NotHaveError_When_Request_IsValid()
    {
        // Arrange
        var model = new CreateStudentRequest
        {
            StudentNo = "S001",
            Name = "John Doe",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = "123 Main St", City = "Toronto", Province = "ON", PostalCode = "M5H2N2", Country = "Canada" }
            }
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_NotHaveError_When_StudentNo_IsAtMaxLength()
    {
        // Arrange
        var model = new CreateStudentRequest
        {
            StudentNo = "S1234567", // Exactly 8 characters
            Name = "John Doe",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = "123 Main St", City = "Toronto", Province = "ON", PostalCode = "M5H2N2", Country = "Canada" }
            }
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.StudentNo);
    }

    [Fact]
    public void Should_ValidateMultipleAddresses()
    {
        // Arrange
        var model = new CreateStudentRequest
        {
            StudentNo = "S001",
            Name = "John Doe",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = "123 Main St", City = "Toronto", Province = "ON", PostalCode = "M5H2N2", Country = "Canada" },
                new AddressDto { Street = "456 Oak Ave", City = "Vancouver", Province = "BC", PostalCode = "V6B1A1", Country = "Canada" }
            }
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_HaveError_When_SecondAddress_IsInvalid()
    {
        // Arrange
        var model = new CreateStudentRequest
        {
            StudentNo = "S001",
            Name = "John Doe",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = "123 Main St", City = "Toronto", Province = "ON", PostalCode = "M5H2N2", Country = "Canada" },
                new AddressDto { Street = "", City = "Vancouver", Province = "BC", PostalCode = "V6B1A1", Country = "Canada" }
            }
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor("Addresses[1].Street")
            .WithErrorMessage("Street is required");
    }

    #endregion

    #region Integration Tests with Service

    [Fact]
    public async Task CreateStudent_WithValidData_ShouldSucceed()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        using var context = factory.CreateContext();
        var service = new StudentService(context);

        var request = new CreateStudentRequest
        {
            StudentNo = "S001",
            Name = "John Doe",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = "123 Main St", City = "Toronto", Province = "ON", PostalCode = "M5H2N2", Country = "Canada" }
            }
        };

        // Act - Validate first
        var validationResult = await _validator.ValidateAsync(request);
        
        // Assert validation
        validationResult.IsValid.Should().BeTrue();

        // Act - Create
        var result = await service.CreateStudentAsync(request);

        // Assert creation
        result.Should().NotBeNull();
        result.StudentNo.Should().Be("S001");
        result.Name.Should().Be("John Doe");
        result.Active.Should().BeTrue();
        
        // Verify in database
        var studentInDb = await context.Students.FindAsync(result.StudentId);
        studentInDb.Should().NotBeNull();
        studentInDb!.Addresses.Should().HaveCount(1);
    }

    [Fact]
    public async Task CreateStudent_WithInvalidData_ShouldFailValidation()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        using var context = factory.CreateContext();
        var service = new StudentService(context);

        var request = new CreateStudentRequest
        {
            StudentNo = "", // Invalid - empty
            Name = "John Doe",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = "123 Main St", City = "Toronto", Province = "ON", PostalCode = "M5H2N2", Country = "Canada" }
            }
        };

        // Act
        var validationResult = await _validator.ValidateAsync(request);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => e.PropertyName == "StudentNo");
        
        // Should not proceed with creation when validation fails
    }

    [Fact]
    public async Task CreateStudent_WithMaxLengthFields_ShouldSucceed()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        using var context = factory.CreateContext();
        var service = new StudentService(context);

        var request = new CreateStudentRequest
        {
            StudentNo = "S1234567", // Max 8 characters
            Name = new string('A', 100), // Max 100 characters
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto 
                { 
                    Street = new string('B', 50), // Max 50
                    City = new string('C', 40), // Max 40
                    Province = new string('D', 30), // Max 30
                    PostalCode = "M5H2N2A", // Max 7
                    Country = new string('E', 30) // Max 30
                }
            }
        };

        // Act
        var validationResult = await _validator.ValidateAsync(request);
        
        // Assert validation
        validationResult.IsValid.Should().BeTrue();

        // Act - Create
        var result = await service.CreateStudentAsync(request);

        // Assert creation
        result.Should().NotBeNull();
        result.StudentNo.Should().Be("S1234567");
    }

    [Fact]
    public async Task CreateAndDeleteStudent_ShouldWork()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        using var context = factory.CreateContext();
        var service = new StudentService(context);

        var request = new CreateStudentRequest
        {
            StudentNo = "S002",
            Name = "Jane Smith",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = "456 Oak Ave", City = "Vancouver", Province = "BC", PostalCode = "V6B1A1", Country = "Canada" }
            }
        };

        // Act - Validate and Create
        var validationResult = await _validator.ValidateAsync(request);
        validationResult.IsValid.Should().BeTrue();

        var created = await service.CreateStudentAsync(request);
        created.Should().NotBeNull();

        // Act - Delete
        var deleteResult = await service.DeleteStudentAsync(created.StudentId);

        // Assert
        deleteResult.Should().BeTrue();
        
        var studentInDb = await context.Students.FindAsync(created.StudentId);
        studentInDb.Should().BeNull();
    }

    [Fact]
    public async Task CreateAndUpdateStudent_ShouldWork()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        using var context = factory.CreateContext();
        var service = new StudentService(context);

        var createRequest = new CreateStudentRequest
        {
            StudentNo = "S003",
            Name = "Bob Wilson",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = "789 Pine Rd", City = "Calgary", Province = "AB", PostalCode = "T2P1J9", Country = "Canada" }
            }
        };

        // Act - Create
        var validationResult = await _validator.ValidateAsync(createRequest);
        validationResult.IsValid.Should().BeTrue();

        var created = await service.CreateStudentAsync(createRequest);

        // Act - Update
        var updateDto = new StudentDto
        {
            StudentId = created.StudentId,
            StudentNo = "S003A",
            Name = "Robert Wilson",
            Active = false
        };

        var updated = await service.UpdateStudentAsync(created.StudentId, updateDto);

        // Assert
        updated.Should().NotBeNull();
        updated!.StudentNo.Should().Be("S003A");
        updated.Name.Should().Be("Robert Wilson");
        updated.Active.Should().BeFalse();
    }

    [Fact]
    public async Task CreateStudent_WithEmptyAddressList_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateStudentRequest
        {
            StudentNo = "S004",
            Name = "Test User",
            Active = true,
            Addresses = new List<AddressDto>() // Empty list
        };

        // Act
        var validationResult = await _validator.ValidateAsync(request);

        // Assert
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().Contain(e => 
            e.PropertyName == "Addresses" && 
            e.ErrorMessage == "At least one address is required");
    }

    [Fact]
    public async Task CreateStudent_WithMultipleAddresses_ShouldSucceed()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        using var context = factory.CreateContext();
        var service = new StudentService(context);

        var request = new CreateStudentRequest
        {
            StudentNo = "S005",
            Name = "Multi Address Student",
            Active = true,
            Addresses = new List<AddressDto>
            {
                new AddressDto { Street = "123 Main St", City = "Toronto", Province = "ON", PostalCode = "M5H2N2", Country = "Canada" },
                new AddressDto { Street = "456 Oak Ave", City = "Vancouver", Province = "BC", PostalCode = "V6B1A1", Country = "Canada" },
                new AddressDto { Street = "789 Pine Rd", City = "Calgary", Province = "AB", PostalCode = "T2P1J9", Country = "Canada" }
            }
        };

        // Act
        var validationResult = await _validator.ValidateAsync(request);
        validationResult.IsValid.Should().BeTrue();

        var result = await service.CreateStudentAsync(request);

        // Assert
        result.Should().NotBeNull();
        
        var studentInDb = await context.Students.FindAsync(result.StudentId);
        studentInDb.Should().NotBeNull();
        studentInDb!.Addresses.Should().HaveCount(3);
    }

    #endregion
}
