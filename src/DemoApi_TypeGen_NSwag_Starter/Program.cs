using Demo.Api.Data;
using Demo.Api.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database
if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

// CORS - Allow Angular app to call the API
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Controllers
builder.Services.AddControllers();

// FluentValidation - automatically validates DTOs (but not DeleteAddressValidator which is used manually)
builder.Services.AddFluentValidationAutoValidation();
// Register validators - only CreateStudentRequestValidator will be auto-validated
builder.Services.AddValidatorsFromAssemblyContaining<Program>(lifetime: ServiceLifetime.Scoped, 
    filter: result => result.ValidatorType != typeof(Demo.Api.Validators.DeleteAddressValidator));

// Manually register DeleteAddressValidator for controller injection (not auto-validation)
builder.Services.AddScoped<Demo.Api.Validators.DeleteAddressValidator>();

// Your services
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IAddressService, AddressService>();

// Swagger (for NSwag / testing)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add OpenAPI document generator for NSwag
builder.Services.AddOpenApiDocument();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // Display Swagger URL in console
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        var addresses = app.Urls;
        foreach (var address in addresses)
        {
            Console.WriteLine($"Swagger UI: {address}/swagger");
        }
    });
}

app.UseCors();

app.UseHttpsRedirection();

// IMPORTANT: map controllers
app.MapControllers();

app.Run();

// Make Program class accessible for testing
public partial class Program { }
