namespace Demo.Api.Data.Entities;

public class Address
{
    public int AddressId { get; set; }
    public int StudentId { get; set; }
    public string Street { get; set; } = "";
    public string City { get; set; } = "";
    public string Province { get; set; } = "";
    public string PostalCode { get; set; } = "";
    public string Country { get; set; } = "";
    
    public Student Student { get; set; } = null!;
}
