# Quick Start Guide

Get up and running with the DemoApi TypeGen & NSwag starter in minutes!

## Prerequisites

Install the following tools before starting:

1. [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
2. [Node.js](https://nodejs.org/) (v18 or later)
3. A code editor (VS Code, Visual Studio, or Rider)

## Step 1: Install Global Tools

```bash
# Install NSwag CLI
dotnet tool install --global NSwag.ConsoleCore

# Install TypeGen CLI (optional - for advanced usage)
dotnet tool install --global TypeGen.DotNetCli
```

## Step 2: Clone and Setup

```bash
# Clone the repository (if not already done)
git clone <repository-url>
cd DemoApi_TypeGen_NSwag_Starter

# Restore .NET dependencies
cd DemoApi
dotnet restore
cd ..

# Install Node dependencies
cd frontend
npm install
cd ..
```

## Step 3: Generate API Client

Run the automated generation script:

```bash
./generate.sh
```

This script will:
- ✅ Build the .NET API
- ✅ Start the API temporarily
- ✅ Download the OpenAPI specification
- ✅ Generate the TypeScript API client
- ✅ Stop the API

## Step 4: Run the API

In one terminal:

```bash
cd DemoApi
dotnet run
```

The API will be available at:
- **API**: http://localhost:5555
- **Swagger UI**: http://localhost:5555/swagger

## Step 5: Run the Frontend

In another terminal:

```bash
cd frontend
npm start
```

The Angular app will be available at:
- **Frontend**: http://localhost:4200

## Verify Everything Works

1. Open http://localhost:4200 in your browser
2. You should see a list of users loaded from the API
3. Click "Reload Users" to fetch data again

## Project Structure

```
DemoApi_TypeGen_NSwag_Starter/
├── DemoApi/                    # ASP.NET Core Web API
│   ├── Controllers/            # API Controllers
│   ├── Models/                 # C# DTOs with TypeGen attributes
│   └── tgconfig.json          # TypeGen configuration
├── frontend/                   # Angular Application
│   └── src/app/
│       ├── models/            # Generated TypeScript models
│       ├── services/          # Generated API client
│       └── users/             # Example component
├── generate.sh                # Generation script
└── README.md                  # Full documentation
```

## Making Changes

### Add a New DTO

1. Create C# class in `DemoApi/Models/`:

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

2. Add controller in `DemoApi/Controllers/`:

```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetProducts()
    {
        // Implementation
    }
}
```

3. Regenerate TypeScript:

```bash
./generate.sh
```

4. Use in Angular:

```typescript
import { ApiClient } from './services/api-client';

this.apiClient.products_GetProducts().subscribe(products => {
    console.log(products); // Fully typed!
});
```

## API Endpoints

The starter includes these example endpoints:

### Users API
- `GET /api/Users` - Get all users
- `GET /api/Users/{id}` - Get user by ID
- `POST /api/Users` - Create new user
- `DELETE /api/Users/{id}` - Delete user

### WeatherForecast API
- `GET /api/WeatherForecast` - Get weather forecasts
- `GET /api/WeatherForecast/{id}` - Get forecast by ID

## Testing the API

### Using curl

```bash
# Get all users
curl http://localhost:5555/api/Users

# Get single user
curl http://localhost:5555/api/Users/1

# Create user
curl -X POST http://localhost:5555/api/Users \
  -H "Content-Type: application/json" \
  -d '{"name":"Test User","email":"test@example.com","role":1}'
```

### Using Swagger UI

1. Navigate to http://localhost:5555/swagger
2. Try out the endpoints interactively
3. View request/response schemas

## Common Commands

```bash
# Build API
cd DemoApi && dotnet build

# Run API
cd DemoApi && dotnet run

# Build Angular
cd frontend && npm run build

# Run Angular dev server
cd frontend && npm start

# Generate API client
./generate.sh

# Clean everything
cd DemoApi && dotnet clean
cd ../frontend && rm -rf node_modules dist
```

## Troubleshooting

### Port Already in Use

If port 5555 or 4200 is in use:

**For API:**
Edit `DemoApi/Properties/launchSettings.json` and change the port

**For Angular:**
Run with custom port:
```bash
ng serve --port 4201
```

### CORS Errors

If you see CORS errors in browser console:
1. Verify API is running on http://localhost:5555
2. Check `Program.cs` has CORS configured
3. Update `frontend/src/app/app.config.ts` with correct API URL

### Build Errors

**TypeScript errors:**
```bash
cd frontend
rm -rf node_modules package-lock.json
npm install
npm run build
```

**.NET errors:**
```bash
cd DemoApi
dotnet clean
dotnet restore
dotnet build
```

## Next Steps

1. **Read the full documentation**: Check [README.md](README.md) for complete details
2. **Explore the setup guide**: See [SETUP_GUIDE.md](SETUP_GUIDE.md) for advanced configuration
3. **Customize the starter**: Modify the models and controllers for your needs
4. **Add authentication**: Implement JWT or other auth mechanisms
5. **Deploy**: Deploy to Azure, AWS, or your preferred platform

## Additional Resources

- [TypeGen Documentation](https://github.com/jburzynski/TypeGen/wiki)
- [NSwag Documentation](https://github.com/RicoSuter/NSwag/wiki)
- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core)
- [Angular Documentation](https://angular.io/docs)

## Getting Help

If you encounter issues:
1. Check the troubleshooting section above
2. Review [SETUP_GUIDE.md](SETUP_GUIDE.md)
3. Check existing GitHub issues
4. Open a new issue with details

---

**Happy coding! 🚀**
