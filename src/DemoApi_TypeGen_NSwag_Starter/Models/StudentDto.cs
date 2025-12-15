using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Api.Models;

[NotMapped]
public class StudentDto
{
    public int StudentId { get; set; }
    public string StudentNo { get; set; } = "";
    public string Name { get; set; } = "";
    public bool Active { get; set; }
}