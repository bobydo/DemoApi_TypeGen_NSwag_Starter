using Demo.Api.Models;
using Demo.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressesController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressesController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    /// <summary>
    /// Get all addresses
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<AddressDto>>> GetAddresses()
    {
        var addresses = await _addressService.GetAddressesAsync();
        return Ok(addresses);
    }

    /// <summary>
    /// Get a specific address by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<AddressDto>> GetAddress(int id)
    {
        var address = await _addressService.GetAddressByIdAsync(id);
        if (address == null)
            return NotFound();

        return Ok(address);
    }

    /// <summary>
    /// Get all addresses for a specific student
    /// </summary>
    [HttpGet("student/{studentId}")]
    public async Task<ActionResult<List<AddressDto>>> GetAddressesByStudent(int studentId)
    {
        var addresses = await _addressService.GetAddressesByStudentIdAsync(studentId);
        return Ok(addresses);
    }

    /// <summary>
    /// Create a new address
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AddressDto>> CreateAddress(AddressDto addressDto)
    {
        var created = await _addressService.CreateAddressAsync(addressDto);
        return CreatedAtAction(nameof(GetAddress), new { id = created.AddressId }, created);
    }

    /// <summary>
    /// Update an existing address
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<AddressDto>> UpdateAddress(int id, AddressDto addressDto)
    {
        var updated = await _addressService.UpdateAddressAsync(id, addressDto);
        if (updated == null)
            return NotFound();

        return Ok(updated);
    }

    /// <summary>
    /// Delete an address
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAddress(int id)
    {
        var deleted = await _addressService.DeleteAddressAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
