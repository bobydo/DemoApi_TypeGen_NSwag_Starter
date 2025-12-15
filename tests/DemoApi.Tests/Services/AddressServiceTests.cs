using Demo.Api.Data.Entities;
using Demo.Api.Models;
using Demo.Api.Services;
using DemoApi.Tests.Helpers;

namespace DemoApi.Tests.Services;

public class AddressServiceTests
{
    [Fact]
    public async Task GetAddressesAsync_ReturnsAllAddresses()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        using var context = factory.CreateContext();
        
        var student = new Student { StudentNo = "S001", Name = "John Doe", Active = true };
        context.Students.Add(student);
        await context.SaveChangesAsync();

        context.Addresses.AddRange(
            new Address { StudentId = student.StudentId, Street = "123 Main St", City = "Toronto", Province = "ON", PostalCode = "M1M1M1", Country = "Canada" },
            new Address { StudentId = student.StudentId, Street = "456 Oak Ave", City = "Ottawa", Province = "ON", PostalCode = "K1K1K1", Country = "Canada" }
        );
        await context.SaveChangesAsync();

        var service = new AddressService(context);

        // Act
        var result = await service.GetAddressesAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(a => a.Street == "123 Main St");
        result.Should().Contain(a => a.Street == "456 Oak Ave");
    }

    [Fact]
    public async Task GetAddressByIdAsync_WithValidId_ReturnsAddress()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        using var context = factory.CreateContext();
        
        var student = new Student { StudentNo = "S001", Name = "John Doe", Active = true };
        context.Students.Add(student);
        await context.SaveChangesAsync();

        var address = new Address
        {
            StudentId = student.StudentId,
            Street = "123 Main St",
            City = "Toronto",
            Province = "ON",
            PostalCode = "M1M1M1",
            Country = "Canada"
        };
        context.Addresses.Add(address);
        await context.SaveChangesAsync();

        var service = new AddressService(context);

        // Act
        var result = await service.GetAddressByIdAsync(address.AddressId);

        // Assert
        result.Should().NotBeNull();
        result!.Street.Should().Be("123 Main St");
        result.City.Should().Be("Toronto");
    }

    [Fact]
    public async Task GetAddressByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        using var context = factory.CreateContext();
        var service = new AddressService(context);

        // Act
        var result = await service.GetAddressByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAddressesByStudentIdAsync_ReturnsStudentAddresses()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        using var context = factory.CreateContext();
        
        var student1 = new Student { StudentNo = "S001", Name = "John Doe", Active = true };
        var student2 = new Student { StudentNo = "S002", Name = "Jane Smith", Active = true };
        context.Students.AddRange(student1, student2);
        await context.SaveChangesAsync();

        context.Addresses.AddRange(
            new Address { StudentId = student1.StudentId, Street = "123 Main St", City = "Toronto", Province = "ON", PostalCode = "M1M1M1", Country = "Canada" },
            new Address { StudentId = student1.StudentId, Street = "456 Oak Ave", City = "Ottawa", Province = "ON", PostalCode = "K1K1K1", Country = "Canada" },
            new Address { StudentId = student2.StudentId, Street = "789 Pine Rd", City = "Vancouver", Province = "BC", PostalCode = "V1V1V1", Country = "Canada" }
        );
        await context.SaveChangesAsync();

        var service = new AddressService(context);

        // Act
        var result = await service.GetAddressesByStudentIdAsync(student1.StudentId);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(a => a.Street == "123 Main St");
        result.Should().Contain(a => a.Street == "456 Oak Ave");
        result.Should().NotContain(a => a.Street == "789 Pine Rd");
    }

    [Fact]
    public async Task CreateAddressAsync_CreatesAddress()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        using var context = factory.CreateContext();
        
        var student = new Student { StudentNo = "S001", Name = "John Doe", Active = true };
        context.Students.Add(student);
        await context.SaveChangesAsync();

        var service = new AddressService(context);

        var addressDto = new AddressDto
        {
            StudentId = student.StudentId,
            Street = "123 Main St",
            City = "Toronto",
            Province = "ON",
            PostalCode = "M1M1M1",
            Country = "Canada"
        };

        // Act
        var result = await service.CreateAddressAsync(addressDto);

        // Assert
        result.AddressId.Should().BeGreaterThan(0);
        result.Street.Should().Be("123 Main St");
        result.City.Should().Be("Toronto");
        result.StudentId.Should().Be(student.StudentId);

        var savedAddress = await context.Addresses.FindAsync(result.AddressId);
        savedAddress.Should().NotBeNull();
        savedAddress!.Street.Should().Be("123 Main St");
    }

    [Fact]
    public async Task UpdateAddressAsync_WithValidId_UpdatesAddress()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        using var context = factory.CreateContext();
        
        var student = new Student { StudentNo = "S001", Name = "John Doe", Active = true };
        context.Students.Add(student);
        await context.SaveChangesAsync();

        var address = new Address
        {
            StudentId = student.StudentId,
            Street = "123 Main St",
            City = "Toronto",
            Province = "ON",
            PostalCode = "M1M1M1",
            Country = "Canada"
        };
        context.Addresses.Add(address);
        await context.SaveChangesAsync();

        var service = new AddressService(context);

        var updateDto = new AddressDto
        {
            AddressId = address.AddressId,
            StudentId = student.StudentId,
            Street = "999 Updated St",
            City = "Montreal",
            Province = "QC",
            PostalCode = "H1H1H1",
            Country = "Canada"
        };

        // Act
        var result = await service.UpdateAddressAsync(address.AddressId, updateDto);

        // Assert
        result.Should().NotBeNull();
        result!.Street.Should().Be("999 Updated St");
        result.City.Should().Be("Montreal");
        result.Province.Should().Be("QC");

        var updatedAddress = await context.Addresses.FindAsync(address.AddressId);
        updatedAddress!.Street.Should().Be("999 Updated St");
        updatedAddress.City.Should().Be("Montreal");
    }

    [Fact]
    public async Task UpdateAddressAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        using var context = factory.CreateContext();
        var service = new AddressService(context);

        var updateDto = new AddressDto
        {
            AddressId = 999,
            StudentId = 1,
            Street = "999 Updated St",
            City = "Montreal",
            Province = "QC",
            PostalCode = "H1H1H1",
            Country = "Canada"
        };

        // Act
        var result = await service.UpdateAddressAsync(999, updateDto);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAddressAsync_WithValidId_DeletesAddress()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        using var context = factory.CreateContext();
        
        var student = new Student { StudentNo = "S001", Name = "John Doe", Active = true };
        context.Students.Add(student);
        await context.SaveChangesAsync();

        context.Addresses.AddRange(
            new Address { StudentId = student.StudentId, Street = "123 Main St", City = "Toronto", Province = "ON", PostalCode = "M1M1M1", Country = "Canada" },
            new Address { StudentId = student.StudentId, Street = "456 Oak Ave", City = "Ottawa", Province = "ON", PostalCode = "K1K1K1", Country = "Canada" }
        );
        await context.SaveChangesAsync();
        var addressToDelete = context.Addresses.First();

        var service = new AddressService(context);

        // Act
        var result = await service.DeleteAddressAsync(addressToDelete.AddressId);

        // Assert
        result.Should().BeTrue();

        var deletedAddress = await context.Addresses.FindAsync(addressToDelete.AddressId);
        deletedAddress.Should().BeNull();

        var remainingAddresses = context.Addresses.Where(a => a.StudentId == student.StudentId).ToList();
        remainingAddresses.Should().HaveCount(1);
    }

    [Fact]
    public async Task DeleteAddressAsync_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        using var factory = new TestDbContextFactory();
        using var context = factory.CreateContext();
        var service = new AddressService(context);

        // Act
        var result = await service.DeleteAddressAsync(999);

        // Assert
        result.Should().BeFalse();
    }
}
