using Demo.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[NotMapped]
public class AddressDto
{
    [Key]
    public int AddressId { get; set; }
    [Required]
    public int StudentId { get; set; }
    // Navigation property
    [ForeignKey(nameof(StudentId))]
    public StudentDto Student { get; set; } = null!;
    public string Street { get; set; } = "";
    public string City { get; set; } = "";
    public string Province { get; set; } = "";
    public string PostalCode { get; set; } = "";
    public string Country { get; set; } = "";
}