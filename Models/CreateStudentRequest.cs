using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Api.Models;

[NotMapped]
public class CreateStudentRequest
{
    public string StudentNo { get; set; } = "";
    public string Name { get; set; } = "";
    public bool Active { get; set; }
    public List<AddressDto> Addresses { get; set; } = new();
}
