using TypeGen.Core.TypeAnnotations;

namespace DemoApi.Models;

[ExportTsEnum]
public enum UserRole
{
    Admin,
    User,
    Guest
}
