using Demo.Api.Data.Entities;
using Demo.Api.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Student> Students { get; set; }
    public DbSet<Address> Addresses { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new StudentConfiguration());
        modelBuilder.ApplyConfiguration(new AddressConfiguration());
        
        SeedData(modelBuilder);
    }
    
    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>().HasData(
            new Student { StudentId = 1, StudentNo = "S0001001", Name = "Alice Johnson", Active = true },
            new Student { StudentId = 2, StudentNo = "S0001002", Name = "Bob Smith", Active = true },
            new Student { StudentId = 3, StudentNo = "S0001003", Name = "Charlie Brown", Active = false },
            new Student { StudentId = 4, StudentNo = "S0001004", Name = "Diana Prince", Active = true },
            new Student { StudentId = 5, StudentNo = "S0001005", Name = "Edward Norton", Active = true },
            new Student { StudentId = 6, StudentNo = "S0001006", Name = "Fiona Davis", Active = false },
            new Student { StudentId = 7, StudentNo = "S0001007", Name = "George Wilson", Active = true },
            new Student { StudentId = 8, StudentNo = "S0001008", Name = "Hannah Moore", Active = true },
            new Student { StudentId = 9, StudentNo = "S0001009", Name = "Ivan Taylor", Active = true },
            new Student { StudentId = 10, StudentNo = "S0001010", Name = "Julia Anderson", Active = false }
        );
        
        modelBuilder.Entity<Address>().HasData(
            new Address { AddressId = 1, StudentId = 1, Street = "123 Main St", City = "New York", Province = "NY", PostalCode = "10001", Country = "USA" },
            new Address { AddressId = 2, StudentId = 1, Street = "456 Oak Ave", City = "Boston", Province = "MA", PostalCode = "02101", Country = "USA" },
            new Address { AddressId = 3, StudentId = 2, Street = "789 Pine Rd", City = "Chicago", Province = "IL", PostalCode = "60601", Country = "USA" },
            new Address { AddressId = 4, StudentId = 2, Street = "321 Elm Dr", City = "Seattle", Province = "WA", PostalCode = "98101", Country = "USA" },
            new Address { AddressId = 5, StudentId = 3, Street = "654 Maple Ln", City = "Austin", Province = "TX", PostalCode = "73301", Country = "USA" },
            new Address { AddressId = 6, StudentId = 3, Street = "987 Cedar Blvd", City = "Denver", Province = "CO", PostalCode = "80201", Country = "USA" },
            new Address { AddressId = 7, StudentId = 4, Street = "147 Birch St", City = "Portland", Province = "OR", PostalCode = "97201", Country = "USA" },
            new Address { AddressId = 8, StudentId = 4, Street = "258 Willow Way", City = "Miami", Province = "FL", PostalCode = "33101", Country = "USA" },
            new Address { AddressId = 9, StudentId = 5, Street = "369 Spruce Ct", City = "Phoenix", Province = "AZ", PostalCode = "85001", Country = "USA" },
            new Address { AddressId = 10, StudentId = 5, Street = "741 Ash Pl", City = "Atlanta", Province = "GA", PostalCode = "30301", Country = "USA" },
            new Address { AddressId = 11, StudentId = 6, Street = "852 Cherry Ter", City = "Dallas", Province = "TX", PostalCode = "75201", Country = "USA" },
            new Address { AddressId = 12, StudentId = 6, Street = "963 Walnut Dr", City = "Houston", Province = "TX", PostalCode = "77001", Country = "USA" },
            new Address { AddressId = 13, StudentId = 7, Street = "159 Poplar Ave", City = "Philadelphia", Province = "PA", PostalCode = "19101", Country = "USA" },
            new Address { AddressId = 14, StudentId = 7, Street = "357 Hickory Rd", City = "San Diego", Province = "CA", PostalCode = "92101", Country = "USA" },
            new Address { AddressId = 15, StudentId = 8, Street = "486 Sycamore St", City = "San Francisco", Province = "CA", PostalCode = "94101", Country = "USA" },
            new Address { AddressId = 16, StudentId = 8, Street = "624 Magnolia Ln", City = "Las Vegas", Province = "NV", PostalCode = "89101", Country = "USA" },
            new Address { AddressId = 17, StudentId = 9, Street = "735 Redwood Blvd", City = "Detroit", Province = "MI", PostalCode = "48201", Country = "USA" },
            new Address { AddressId = 18, StudentId = 9, Street = "846 Cypress Way", City = "Minneapolis", Province = "MN", PostalCode = "55401", Country = "USA" },
            new Address { AddressId = 19, StudentId = 10, Street = "957 Dogwood Ct", City = "Tampa", Province = "FL", PostalCode = "33601", Country = "USA" },
            new Address { AddressId = 20, StudentId = 10, Street = "168 Beech Pl", City = "Baltimore", Province = "MD", PostalCode = "21201", Country = "USA" }
        );
    }
}
