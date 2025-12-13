#!/bin/bash
set -e

echo "==================================="
echo "Generating API Client and Models"
echo "==================================="

# Step 1: Build the API
echo ""
echo "Step 1: Building DemoApi..."
cd DemoApi
dotnet build -c Release
cd ..

# Step 2: Start the API temporarily
echo ""
echo "Step 2: Starting API server temporarily..."
cd DemoApi
dotnet run --no-build -c Release --urls "http://localhost:5555" &
API_PID=$!
cd ..

# Wait for API to be ready
echo "Waiting for API to start..."
for i in {1..30}; do
    if curl -s http://localhost:5555/swagger/v1/swagger.json > /dev/null 2>&1; then
        echo "API is ready!"
        break
    fi
    if [ $i -eq 30 ]; then
        echo "Error: API failed to start within 30 seconds"
        kill $API_PID 2>/dev/null || true
        exit 1
    fi
    sleep 1
done

# Step 3: Download OpenAPI specification
echo ""
echo "Step 3: Downloading OpenAPI specification..."
curl -s http://localhost:5555/swagger/v1/swagger.json > openapi.json
echo "OpenAPI specification downloaded successfully"

# Step 4: Stop the API
echo ""
echo "Step 4: Stopping API server..."
kill $API_PID
wait $API_PID 2>/dev/null || true

# Step 5: Generate TypeScript API client with NSwag
echo ""
echo "Step 5: Generating TypeScript API client..."
nswag run nswag-from-file.json
echo "API client generated successfully at frontend/src/app/services/api-client.ts"

# Step 6: Note about TypeGen
echo ""
echo "Step 6: TypeScript models (TypeGen)"
echo "TypeScript models are located in frontend/src/app/models/"
echo "These models mirror the C# DTOs and can be regenerated using TypeGen"
echo ""
echo "To use TypeGen CLI (requires .NET SDK matching TypeGen version):"
echo "  cd DemoApi"
echo "  dotnet-typegen generate"
echo ""
echo "Note: TypeGen models are configured in DemoApi/tgconfig.json"

echo ""
echo "==================================="
echo "Generation Complete!"
echo "==================================="
echo ""
echo "Generated files:"
echo "  - frontend/src/app/services/api-client.ts (NSwag API client)"
echo "  - frontend/src/app/models/*.ts (TypeScript models)"
echo ""
