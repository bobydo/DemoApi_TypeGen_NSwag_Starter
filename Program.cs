using Demo.Api.Services;

var builder = WebApplication.CreateBuilder(args);

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

// Your services
builder.Services.AddScoped<IStudentService, StudentService>();

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
