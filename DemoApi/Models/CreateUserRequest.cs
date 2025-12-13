using TypeGen.Core.TypeAnnotations;

namespace DemoApi.Models;

[ExportTsClass]
public class CreateUserRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}
