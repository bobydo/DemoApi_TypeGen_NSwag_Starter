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
	/JsonLibrary:SystemTextJson \
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
