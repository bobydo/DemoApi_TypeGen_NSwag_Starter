# DemoApi_TypeGen_NSwag_Starter

DemoApi/
├─ Models/
│  └─ StudentDto.cs          ← for TypeGen (C# → TS models)
├─ Services/
│  ├─ IStudentService.cs
│  └─ StudentService.cs
├─ Controllers/
│  └─ StudentsController.cs  ← for NSwag (API → TS client)
├─ Program.cs
├─ typegen.json              ← TypeGen config
└─ nswag.json                ← NSwag config

## Setup & Run

```bash
# Restore NuGet packages and dependencies
dotnet restore

# Run the application with hot reload (auto-restart on file changes)
dotnet watch run
```

After the app starts, navigate to: http://localhost:5098/swagger

## Code Generation Tools

### TypeGen (C# Models → TypeScript Interfaces)

> **Note:** TypeGen.DotNetCli currently has compatibility issues with modern .NET versions. The latest available version (2.4.7) requires .NET Core 2.2 which is end-of-life. For TypeScript model generation, consider:
> - Using NSwag's model generation capabilities (included in the API client generation below)
> - Manually creating TypeScript interfaces
> - Using alternative tools like [ts-generator](https://github.com/borisdamevin/ts-generator) or [CSharpToTypeScript](https://github.com/AdrianWilczynski/CSharpToTypeScript)

**If you still want to try TypeGen:**
```bash
# The configuration is in typegen.json, but the tool requires .NET Core 2.2
dotnet tool install --global TypeGen.DotNetCli
dotnet typegen generate
```

### NSwag (API Endpoints → TypeScript Client)

```bash
# Install NSwag globally (generates TypeScript API client from OpenAPI/Swagger)
dotnet tool install --global NSwag.ConsoleCore

# Generate TypeScript API client from your API endpoints (uses nswag.json config)
nswag run
```
