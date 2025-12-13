# DemoApi_TypeGen_NSwag_Starter

A comprehensive starter project demonstrating how to use **TypeGen** to keep frontend models in sync with C# DTOs, and **NSwag** to generate typed Angular API clients from ASP.NET Core controllers.

## 🚀 Quick Links

- **[Quick Start Guide](QUICKSTART.md)** - Get started in 5 minutes
- **[Setup Guide](SETUP_GUIDE.md)** - Detailed configuration and troubleshooting
- **[VS Code Workspace](DemoApi_TypeGen_NSwag_Starter.code-workspace)** - Open the project in VS Code

## Overview

This project showcases a complete workflow for maintaining type safety between a .NET backend and an Angular frontend:

- **TypeGen**: Automatically generates TypeScript models from C# DTOs with annotations
- **NSwag**: Generates a fully-typed Angular HTTP client from OpenAPI/Swagger specifications
- **ASP.NET Core Web API**: Sample backend with controllers and DTOs
- **Angular**: Sample frontend consuming the generated client

## Project Structure

```
.
├── DemoApi/                        # ASP.NET Core Web API
│   ├── Controllers/                # API Controllers
│   │   ├── UsersController.cs
│   │   └── WeatherForecastController.cs
│   ├── Models/                     # DTOs with TypeGen attributes
│   │   ├── User.cs
│   │   ├── UserRole.cs
│   │   ├── CreateUserRequest.cs
│   │   └── WeatherForecast.cs
│   ├── tgconfig.json              # TypeGen configuration
│   └── Program.cs                  # App configuration with NSwag
├── frontend/                       # Angular application
│   └── src/app/
│       ├── models/                 # Generated TypeScript models (TypeGen)
│       │   ├── user.ts
│       │   ├── weather-forecast.ts
│       │   └── create-user-request.ts
│       └── services/               # Generated API client (NSwag)
│           └── api-client.ts
├── nswag.json                     # NSwag configuration (AspNetCore)
├── nswag-from-file.json           # NSwag configuration (from OpenAPI file)
└── generate.sh                     # Script to generate API client and models
```

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [Node.js](https://nodejs.org/) (v18 or later)
- [NSwag CLI](https://github.com/RicoSuter/NSwag/wiki/CommandLine)
- [TypeGen CLI](https://github.com/jburzynski/TypeGen) (optional)

## Getting Started

### 1. Install Required Tools

```bash
# Install NSwag CLI globally
dotnet tool install --global NSwag.ConsoleCore

# Install TypeGen CLI globally (optional)
dotnet tool install --global TypeGen.DotNetCli
```

### 2. Build and Run the API

```bash
cd DemoApi
dotnet build
dotnet run
```

The API will be available at `http://localhost:5555` with Swagger UI at `http://localhost:5555/swagger`.

### 3. Generate API Client and Models

Use the provided script to automatically generate the TypeScript client and models:

```bash
./generate.sh
```

This script will:
1. Build the DemoApi project
2. Start the API temporarily
3. Download the OpenAPI specification
4. Generate the TypeScript API client using NSwag
5. Stop the API

### 4. Run the Angular Frontend

```bash
cd frontend
npm install
npm start
```

The frontend will be available at `http://localhost:4200`.

## Key Features

### TypeGen Integration

TypeGen generates TypeScript models from C# DTOs annotated with `[ExportTsClass]` and `[ExportTsEnum]` attributes.

**C# DTO Example:**

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

**Generated TypeScript:**

```typescript
export class User {
    id!: number;
    name!: string;
    email!: string;
    createdAt!: Date;
    role!: UserRole;
}

export enum UserRole {
    Admin = 0,
    User = 1,
    Guest = 2
}
```

Configuration is in `DemoApi/tgconfig.json`:
- Output path: `../frontend/src/app/models`
- Naming conventions: PascalCase to camelCase
- Generates index file for easy imports

### NSwag Integration

NSwag generates a fully-typed Angular HTTP client from your ASP.NET Core controllers.

**Configuration in Program.cs:**

```csharp
builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "Demo API";
    config.Version = "v1";
});
```

**Generated Client Usage:**

```typescript
import { ApiClient } from './services/api-client';

@Component({...})
export class UsersComponent {
    constructor(private apiClient: ApiClient) {}
    
    loadUsers() {
        this.apiClient.getUsers().subscribe(users => {
            console.log(users); // Fully typed!
        });
    }
}
```

## API Endpoints

### Users API

- `GET /api/Users` - Get all users
- `GET /api/Users/{id}` - Get user by ID
- `POST /api/Users` - Create new user
- `DELETE /api/Users/{id}` - Delete user

### WeatherForecast API

- `GET /api/WeatherForecast` - Get weather forecasts
- `GET /api/WeatherForecast/{id}` - Get forecast by ID

## Development Workflow

### Adding a New DTO

1. Create C# DTO with TypeGen attributes:

```csharp
using TypeGen.Core.TypeAnnotations;

[ExportTsClass]
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

2. Add controller methods using the DTO

3. Regenerate API client:

```bash
./generate.sh
```

### Manual Generation

If you prefer manual steps:

```bash
# Build API
cd DemoApi
dotnet build

# Generate TypeScript models (requires TypeGen CLI)
dotnet-typegen generate

# Start API
dotnet run &

# Generate API client
cd ..
curl -s http://localhost:5555/swagger/v1/swagger.json > openapi.json
nswag run nswag-from-file.json

# Stop API
kill %1
```

## Configuration Files

### TypeGen Configuration (tgconfig.json)

- `assemblies`: List of assemblies to scan for types
- `outputPath`: Where to generate TypeScript files
- `fileNameConversions`: Naming convention for files (e.g., PascalCase to dash-case)
- `typeNameConversions`: Naming convention for types
- `propertyNameConversions`: Naming convention for properties
- `csNullableTranslation`: How to handle C# nullable types

### NSwag Configuration (nswag-from-file.json)

- `documentGenerator.fromDocument.url`: Path to OpenAPI specification
- `codeGenerators.openApiToTypeScriptClient`: TypeScript client generation settings
  - `template`: "Angular" for Angular HttpClient
  - `generateDtoTypes`: true to include model classes
  - `operationGenerationMode`: How to organize client methods
  - `output`: Where to save the generated client

## Benefits

1. **Type Safety**: Full type checking from backend to frontend
2. **No Manual Sync**: Models and API clients update automatically
3. **IntelliSense**: Complete autocomplete in your IDE
4. **Refactoring**: Rename properties in C# and catch errors in TypeScript
5. **Documentation**: OpenAPI spec provides API documentation
6. **Less Boilerplate**: No need to write HTTP client code manually

## Troubleshooting

### TypeGen CLI Version Issues

If you encounter version compatibility issues with TypeGen CLI:
- The models can also be manually maintained to match C# DTOs
- Alternatively, use a .NET version compatible with the TypeGen CLI version
- Consider using the NuGet package directly in a custom build task

### NSwag Runtime Mismatch

Ensure the NSwag CLI version matches the NuGet package version in your project:

```bash
dotnet tool list --global | grep NSwag
```

Check `DemoApi.csproj` for the `NSwag.AspNetCore` package version.

### API Not Starting

Check if port 5555 is already in use:

```bash
lsof -i :5555
```

Change the port in `generate.sh` and `nswag-from-file.json` if needed.

## References

- [TypeGen Documentation](https://github.com/jburzynski/TypeGen/wiki)
- [NSwag Documentation](https://github.com/RicoSuter/NSwag/wiki)
- [OpenAPI Specification](https://swagger.io/specification/)
- [Angular HttpClient](https://angular.io/guide/http)

## License

This project is a starter template and is free to use for any purpose.