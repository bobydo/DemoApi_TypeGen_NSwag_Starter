using Demo.Api.Data;
using Demo.Api.Data.Entities;
using Demo.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api.Services;

public class AddressService : IAddressService
{
    private readonly ApplicationDbContext _context;

    public AddressService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<AddressDto>> GetAddressesAsync()
    {
        return await _context.Addresses
            .AsNoTracking()
            .Select(a => new AddressDto
            {
                AddressId = a.AddressId,
                StudentId = a.StudentId,
                Street = a.Street,
                City = a.City,
                Province = a.Province,
                PostalCode = a.PostalCode,
                Country = a.Country
            })
            .ToListAsync();
    }

    public async Task<AddressDto?> GetAddressByIdAsync(int id)
    {
        var address = await _context.Addresses
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.AddressId == id);

        if (address == null)
            return null;

        return new AddressDto
        {
            AddressId = address.AddressId,
            StudentId = address.StudentId,
            Street = address.Street,
            City = address.City,
            Province = address.Province,
            PostalCode = address.PostalCode,
            Country = address.Country
        };
    }

    public async Task<List<AddressDto>> GetAddressesByStudentIdAsync(int studentId)
    {
        return await _context.Addresses
            .AsNoTracking()
            .Where(a => a.StudentId == studentId)
            .Select(a => new AddressDto
            {
                AddressId = a.AddressId,
                StudentId = a.StudentId,
                Street = a.Street,
                City = a.City,
                Province = a.Province,
                PostalCode = a.PostalCode,
                Country = a.Country
            })
            .ToListAsync();
    }

    public async Task<AddressDto> CreateAddressAsync(AddressDto addressDto)
    {
        var address = new Address
        {
            StudentId = addressDto.StudentId,
            Street = addressDto.Street,
            City = addressDto.City,
            Province = addressDto.Province,
            PostalCode = addressDto.PostalCode,
            Country = addressDto.Country
        };

        _context.Addresses.Add(address);
        await _context.SaveChangesAsync();

        addressDto.AddressId = address.AddressId;
        return addressDto;
    }

    public async Task<AddressDto?> UpdateAddressAsync(int id, AddressDto addressDto)
    {
        var address = await _context.Addresses.FindAsync(id);
        if (address == null)
            return null;

        address.StudentId = addressDto.StudentId;
        address.Street = addressDto.Street;
        address.City = addressDto.City;
        address.Province = addressDto.Province;
        address.PostalCode = addressDto.PostalCode;
        address.Country = addressDto.Country;

        await _context.SaveChangesAsync();

        addressDto.AddressId = address.AddressId;
        return addressDto;
    }

    public async Task<bool> DeleteAddressAsync(int id)
    {
        var address = await _context.Addresses.FindAsync(id);
        if (address == null)
            return false;

        _context.Addresses.Remove(address);
        await _context.SaveChangesAsync();
        return true;
    }
}
