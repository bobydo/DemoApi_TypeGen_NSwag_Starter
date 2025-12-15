using Demo.Api.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Tests.Helpers;

public class TestDbContextFactory : IDisposable
{
    private SqliteConnection? _connection;

    public ApplicationDbContext CreateContext(bool includeSeedData = false)
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();

        // Clear seed data if not needed
        if (!includeSeedData)
        {
            context.Students.RemoveRange(context.Students);
            context.Addresses.RemoveRange(context.Addresses);
            context.SaveChanges();
        }

        return context;
    }

    public void Dispose()
    {
        _connection?.Close();
        _connection?.Dispose();
    }
}
