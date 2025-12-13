# TypeGen and NSwag Setup Guide

## Overview

This project demonstrates the integration of TypeGen and NSwag to maintain type safety between a .NET backend and Angular frontend.

## What is TypeGen?

TypeGen is a tool that generates TypeScript type definitions from C# classes. It ensures that your frontend TypeScript models stay in sync with backend C# DTOs.

### Key Features:
- Automatic TypeScript generation from C# classes
- Support for classes, interfaces, enums
- Customizable naming conventions
- Type mappings between C# and TypeScript

## What is NSwag?

NSwag generates TypeScript HTTP clients from OpenAPI/Swagger specifications. It creates fully-typed Angular services that match your API controllers.

### Key Features:
- Generates Angular HTTP clients
- Full TypeScript type safety
- Supports all HTTP methods
- Includes error handling
- Works with Angular HttpClient

## How They Work Together

```
C# DTOs (with TypeGen attributes)
    ↓
TypeGen generates TypeScript models
    ↓
    models/user.ts, models/weather-forecast.ts, etc.

C# Controllers
    ↓
ASP.NET Core generates OpenAPI spec
    ↓
NSwag generates Angular HTTP client
    ↓
    services/api-client.ts
```

## TypeGen Configuration

### 1. Annotate C# Classes

Add `[ExportTsClass]` attribute to DTOs you want to export:

```csharp
using TypeGen.Core.TypeAnnotations;

namespace DemoApi.Models;

[ExportTsClass]
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserRole Role { get; set; }
}

[ExportTsEnum]
public enum UserRole
{
    Admin,
    User,
    Guest
}
```

### 2. Configure tgconfig.json

```json
{
  "assemblies": [
    "DemoApi.dll"
  ],
  "outputPath": "../frontend/src/app/models",
  "createIndexFile": true,
  "fileNameConversions": ["PascalCaseToDashes"],
  "typeNameConversions": ["PascalCaseToCamelCase"],
  "propertyNameConversions": ["PascalCaseToCamelCase"]
}
```

### 3. Generate TypeScript Models

```bash
cd DemoApi
dotnet build
dotnet-typegen generate
```

## NSwag Configuration

### 1. Configure ASP.NET Core

In `Program.cs`:

```csharp
builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "Demo API";
    config.Version = "v1";
});

app.UseOpenApi();
app.UseSwaggerUi();
```

### 2. Configure nswag-from-file.json

Key settings:
- `template: "Angular"` - Generate Angular client
- `generateDtoTypes: true` - Include model classes
- `operationGenerationMode: "SingleClientFromOperationId"` - Single client class
- `output` - Where to save generated file

### 3. Generate Angular Client

```bash
# Start API
cd DemoApi
dotnet run &

# Download OpenAPI spec
curl -s http://localhost:5555/swagger/v1/swagger.json > openapi.json

# Generate client
nswag run nswag-from-file.json

# Stop API
kill %1
```

Or use the automated script:
```bash
./generate.sh
```

## Using Generated Code

### In Angular Components

```typescript
import { Component, inject, OnInit } from '@angular/core';
import { ApiClient, User } from '../services/api-client';

@Component({...})
export class UsersComponent implements OnInit {
  private apiClient = inject(ApiClient);
  users: User[] = [];

  ngOnInit() {
    this.apiClient.users_GetUsers().subscribe(users => {
      this.users = users; // Fully typed!
    });
  }
}
```

### Type Safety Benefits

```typescript
// ✅ TypeScript catches errors at compile time
user.name = "John";      // OK
user.naem = "John";      // Error: Property 'naem' does not exist

// ✅ IntelliSense provides autocomplete
user.  // Shows: id, name, email, createdAt, role

// ✅ Enum safety
user.role = UserRole.Admin;  // OK
user.role = "admin";         // Error: Type 'string' is not assignable
```

## Development Workflow

### Adding a New DTO

1. Create C# class with TypeGen attribute:
```csharp
[ExportTsClass]
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

2. Add controller using the DTO:
```csharp
[HttpGet]
public ActionResult<IEnumerable<Product>> GetProducts()
{
    return Ok(_products);
}
```

3. Regenerate TypeScript:
```bash
./generate.sh
```

4. Use in Angular:
```typescript
this.apiClient.products_GetProducts().subscribe(products => {
    // products is Product[]
});
```

## Common Issues and Solutions

### TypeGen CLI Version Mismatch

**Problem:** TypeGen CLI requires older .NET version

**Solution 1:** Use compatible .NET version
```bash
dotnet tool uninstall --global TypeGen.DotNetCli
# Install compatible version
```

**Solution 2:** Manually maintain TypeScript models to match C# DTOs

**Solution 3:** Create build task using TypeGen NuGet package directly

### NSwag Method Naming

**Problem:** Methods named like `users_GetUsers()` instead of `getUsers()`

**Cause:** `operationGenerationMode` setting

**Solutions:**
- Use `SingleClientFromOperationId` for single client with prefixed methods
- Use `MultipleClientsFromOperationId` for multiple client classes
- Customize with `operationIdGenerator`

### CORS Issues

**Problem:** Angular can't call API

**Solution:** Enable CORS in `Program.cs`:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

app.UseCors("AllowAngular");
```

### Type Mapping Issues

**Problem:** C# type doesn't map correctly to TypeScript

**Solutions:**
- Use TypeGen custom converters
- Configure type mappings in tgconfig.json
- Use explicit TypeScript types with attributes

## Best Practices

1. **Always annotate DTOs**
   - Use `[ExportTsClass]` on all DTOs
   - Use `[ExportTsEnum]` on enums used in DTOs

2. **Regenerate after changes**
   - Run `./generate.sh` after modifying DTOs or controllers
   - Verify Angular build succeeds

3. **Version control**
   - Commit generated files to track changes
   - Review diffs to catch breaking changes

4. **API versioning**
   - Use API versioning for backwards compatibility
   - Version your OpenAPI spec

5. **Documentation**
   - Add XML comments to controllers
   - Use `[SwaggerOperation]` for better documentation

## Advanced Configuration

### Custom Type Mappings

In tgconfig.json:
```json
{
  "customTypeMappings": {
    "Guid": "string",
    "decimal": "number"
  }
}
```

### TypeGen Attributes

```csharp
[ExportTsClass(OutputDir = "custom/path")]
[TsIgnore]  // Don't export this property
[TsType(TsType.String)]  // Override type
```

### NSwag Customization

```json
{
  "operationIdGenerator": "MultipleClientsFromPathSegments",
  "clientClassAccessModifier": "public",
  "generateOptionalParameters": true
}
```

## Resources

- [TypeGen GitHub](https://github.com/jburzynski/TypeGen)
- [TypeGen Documentation](https://github.com/jburzynski/TypeGen/wiki)
- [NSwag Documentation](https://github.com/RicoSuter/NSwag/wiki)
- [OpenAPI Specification](https://swagger.io/specification/)
- [Angular HttpClient](https://angular.io/guide/http)

## Troubleshooting Commands

```bash
# Check .NET version
dotnet --version

# Check Node version
node --version

# List installed dotnet tools
dotnet tool list --global

# Clean and rebuild
cd DemoApi
dotnet clean
dotnet build

cd ../frontend
rm -rf node_modules dist
npm install
npm run build

# View OpenAPI spec
curl http://localhost:5555/swagger/v1/swagger.json | jq .

# Test API endpoint
curl http://localhost:5555/api/Users
```
