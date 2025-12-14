using Demo.Api.Models;

namespace Demo.Api.Services;

public interface IAddressService
{
    Task<List<AddressDto>> GetAddressesAsync();
    Task<AddressDto?> GetAddressByIdAsync(int id);
    Task<List<AddressDto>> GetAddressesByStudentIdAsync(int studentId);
    Task<AddressDto> CreateAddressAsync(AddressDto addressDto);
    Task<AddressDto?> UpdateAddressAsync(int id, AddressDto addressDto);
    Task<bool> DeleteAddressAsync(int id);
}
