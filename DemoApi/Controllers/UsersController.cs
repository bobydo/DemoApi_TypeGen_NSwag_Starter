using Microsoft.AspNetCore.Mvc;
using DemoApi.Models;

namespace DemoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private static readonly List<User> Users = new()
    {
        new User
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            CreatedAt = DateTime.UtcNow.AddDays(-30),
            Role = UserRole.Admin
        },
        new User
        {
            Id = 2,
            Name = "Jane Smith",
            Email = "jane@example.com",
            CreatedAt = DateTime.UtcNow.AddDays(-15),
            Role = UserRole.User
        }
    };

    [HttpGet]
    public ActionResult<IEnumerable<User>> GetUsers()
    {
        return Ok(Users);
    }

    [HttpGet("{id}")]
    public ActionResult<User> GetUser(int id)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public ActionResult<User> CreateUser([FromBody] CreateUserRequest request)
    {
        var user = new User
        {
            Id = Users.Max(u => u.Id) + 1,
            Name = request.Name,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow,
            Role = request.Role
        };

        Users.Add(user);
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteUser(int id)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        Users.Remove(user);
        return NoContent();
    }
}
