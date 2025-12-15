using Demo.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DemoApi.Tests.Helpers;

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
