using Demo.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DemoApi.Tests.Helpers;

/// <summary>
/// ✅ Fixed the dual EF Core provider registration issue (via the environment-based check in Program.cs)
/// ✅ Set up WebApplicationFactory with SQLite for testing
/// ✅ Fixed all controller test logic issues (routes, validation, status codes, data setup)
/// ✅ Ensured proper database isolation per test class
/// </summary>
public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureTestServices(services =>
        {
            // 🔥 REMOVE ALL DbContext registrations (critical)
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            services.RemoveAll<ApplicationDbContext>();

            // 🔥 Shared SQLite in-memory connection
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            services.AddSingleton(connection);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(connection);
            });
        });
    }
}
