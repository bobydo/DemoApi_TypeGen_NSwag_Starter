Install (one-time):
dotnet tool install -g NSwag.ConsoleCore
cd src\DemoApi_TypeGen_NSwag_Starter

NSwag can generate more than just this C# client file.

- C# client: Generates typed HTTP clients (like your StudentApiClient) and optionally DTO classes. You can tweak namespace, class name, JSON library (Newtonsoft vs System.Text.Json), and method styles.
- TypeScript/Angular client: Generates TS clients or Angular services from OpenAPI (you already have this via nswag.json).
- Server stubs: Can generate ASP.NET Core controllers or minimal-API stubs from an OpenAPI spec to bootstrap a server.
- OpenAPI document: Can produce the Swagger/OpenAPI JSON from your ASP.NET Core app (your aspNetCoreToOpenApi config).
- Code style/cleanliness: Supports options to reduce pragma noise, change nullability, use System.Text.Json, skip DTO generation if you have shared models, and customize operation naming.

nswag openapi2csclient /input:http://localhost:5098/swagger/v1/swagger.json \
	/classname:StudentApiClient \
	/namespace:DemoApi_TypeGen_NSwag_Starter.Spec \
	/output:Spec\GatewayClient.cs \
	/JsonLibrary:NewtonsoftJson \
	/GenerateDtoTypes:false \
	/GenerateClientInterfaces:true \
	/GenerateOptionalParameters:true \
	/UseBaseUrl:false \
	/OperationGenerationMode:SingleClientFromOperationId \
	/ClassStyle:Poco \
	/GenerateJsonMethods:false \
	/GenerateDataAnnotations:false


### Explanation:

NSwag is a tool that auto-generates API clients from an OpenAPI (Swagger) specification.
This command generates a C# client so you don’t have to manually write HTTP requests.

nswag openapi2csclient
- Instructs NSwag to generate a C# client from an OpenAPI spec.

input: Spec/openApiGatewaySpec.json
- The OpenAPI (Swagger) file that defines all endpoints, requests, and responses.
- Acts as the API contract.

classname: GeneratedApiGatewayClient
- Name of the generated C# client class.

namespace: ApiGateway
- Namespace where the generated client code lives.

output: Spec/GatewayClient.cs
- Output file containing the generated client code.

What this generates:
- Strongly-typed C# client class
- Async methods for each API endpoint
- Request/response DTOs
- HttpClient logic handled for you
- Error handling based on the OpenAPI spec

Why this is useful:
- Eliminates manual REST code
- Compile-time safety
- Client always matches backend API
- Ideal for gateways, microservices, and integration tests

Notes:
- The config in Spec/nswag.json is intentionally minimal (only non-defaults). Some tools (e.g., NSwag Studio “Save”) will re-expand defaults when exporting; prefer editing the JSON manually to keep it lean.
- Using Newtonsoft.Json requires the package reference in the web project. This repo includes it in src/DemoApi_TypeGen_NSwag_Starter/DemoApi_TypeGen_NSwag_Starter.csproj.
- Where does the Swagger URL come from? The app listens on the URLs defined in src/DemoApi_TypeGen_NSwag_Starter/Properties/launchSettings.json (e.g., http://localhost:5098). Swashbuckle serves the OpenAPI at /swagger/v1/swagger.json, so the full URL is http://localhost:5098/swagger/v1/swagger.json when the app is running. With the project-based generator (aspNetCoreToOpenApi) in nswag.json, you don’t need the app running; NSwag builds the project to produce the spec.

#### Project-based generator vs URL

- The `documentGenerator.aspNetCoreToOpenApi.project` setting in Spec/nswag.json points to the web project. NSwag compiles the project and uses Swashbuckle to produce the same OpenAPI JSON you’d get live at http://localhost:5098/swagger/v1/swagger.json.
- The live URL is defined by the `applicationUrl` values in src/DemoApi_TypeGen_NSwag_Starter/Properties/launchSettings.json and the Swagger endpoint path `/swagger/v1/swagger.json`.
- Benefit: with project-based generation you don’t need to run the app to regenerate clients; with URL-based generation you must have the app running on the specified port.
## Custom Template for Type Annotations

### Problem
NSwag's default Angular template generates arrow function parameters without explicit type annotations when TypeScript can infer the type, leading to inconsistent code style:
```typescript
// Some parameters have explicit types:
.pipe(_observableMergeMap((_responseText: string) => {

// Others rely on inference (no explicit type):
.pipe(_observableMergeMap(_responseText => {
```

### Solution
Created a custom Liquid template that enforces explicit type annotations during generation.

**Files:**
- `Spec/Client.ProcessResponse.ReadBodyStart.liquid` - Custom template that adds `: string` to all `_responseText` parameters
- `Spec/nswag.json` - Updated with `"templateDirectory": "."` to use custom templates

**Configuration in nswag.json:**
```json
"openApiToTypeScriptClient": {
  "template": "Angular",
  "templateDirectory": ".",
  "output": "ClientApp/src/app/services/students-api.service.ts"
}
```

**Result:**
All arrow function parameters now have explicit type annotations:
```typescript
return blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
```

## Referencing ClientApp's node_modules from Spec

### Problem
Want to validate generated TypeScript files in Spec without installing duplicate packages.

### Solution
Configure TypeScript to reference ClientApp's `node_modules` using path mapping.

**Files:**
- `Spec/tsconfig.json` - TypeScript configuration with path mappings

**Configuration in Spec/tsconfig.json:**
```json
{
  "compilerOptions": {
    "baseUrl": "../ClientApp",
    "paths": {
      "rxjs": ["node_modules/rxjs"],
      "rxjs/*": ["node_modules/rxjs/*"],
      "@angular/*": ["node_modules/@angular/*"]
    },
    "noEmit": true,
    "skipLibCheck": true
  },
  "include": [
    "ClientApp/src/**/*.ts"
  ]
}
```

**Validate TypeScript without installing packages in Spec:**
```powershell
# Run from ClientApp directory to use its node_modules
cd src\DemoApi_TypeGen_NSwag_Starter\ClientApp
npx tsc --noEmit --project ../Spec/tsconfig.json
```

**Benefits:**
- ✅ No duplicate `node_modules` installation in Spec
- ✅ Use existing Angular/RxJS packages from ClientApp
- ✅ Validate generated code type-safety
- ✅ Single source of truth for package versions
