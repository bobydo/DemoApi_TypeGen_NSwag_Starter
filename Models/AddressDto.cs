using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Api.Models;

[NotMapped]
public class AddressDto
{
    public int AddressId { get; set; }
    public int StudentId { get; set; }
    public string Street { get; set; } = "";
    public string City { get; set; } = "";
    public string Province { get; set; } = "";
    public string PostalCode { get; set; } = "";
    public string Country { get; set; } = "";
}