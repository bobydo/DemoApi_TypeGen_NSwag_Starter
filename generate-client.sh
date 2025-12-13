#!/bin/bash

# Build the API
echo "Building DemoApi..."
cd DemoApi
dotnet build -c Release

# Run the API in the background
echo "Starting API server..."
dotnet run --no-build -c Release &
API_PID=$!

# Wait for API to start
echo "Waiting for API to start..."
sleep 10

# Download the OpenAPI spec
echo "Downloading OpenAPI specification..."
curl -o ../openapi.json http://localhost:5000/swagger/v1/swagger.json

# Stop the API
echo "Stopping API server..."
kill $API_PID

cd ..

# Generate TypeScript client using NSwag
echo "Generating TypeScript API client..."
cat > nswag-runtime.json << 'EOF'
{
  "runtime": "Net100",
  "documentGenerator": {
    "fromDocument": {
      "json": "$(cat openapi.json)"
    }
  },
  "codeGenerators": {
    "openApiToTypeScriptClient": {
      "className": "ApiClient",
      "moduleName": "",
      "namespace": "",
      "typeScriptVersion": 5.3,
      "template": "Angular",
      "promiseType": "Promise",
      "httpClass": "HttpClient",
      "withCredentials": false,
      "useSingletonProvider": true,
      "injectionTokenType": "InjectionToken",
      "rxJsVersion": 7.0,
      "dateTimeType": "Date",
      "nullValue": "Undefined",
      "generateClientClasses": true,
      "generateClientInterfaces": false,
      "generateOptionalParameters": true,
      "exportTypes": true,
      "wrapDtoExceptions": true,
      "exceptionClass": "ApiException",
      "clientBaseClass": null,
      "wrapResponses": false,
      "wrapResponseMethods": [],
      "generateResponseClasses": true,
      "responseClass": "SwaggerResponse",
      "protectedMethods": [],
      "configurationClass": null,
      "useTransformOptionsMethod": false,
      "useTransformResultMethod": false,
      "generateDtoTypes": false,
      "operationGenerationMode": "MultipleClientsFromOperationId",
      "markOptionalProperties": true,
      "generateCloneMethod": false,
      "typeStyle": "Class",
      "enumStyle": "Enum",
      "useLeafType": false,
      "classTypes": [],
      "extendedClasses": [],
      "extensionCode": null,
      "generateDefaultValues": true,
      "excludedTypeNames": [],
      "excludedParameterNames": [],
      "handleReferences": false,
      "generateTypeCheckMethods": false,
      "generateConstructorInterface": true,
      "convertConstructorInterfaceData": false,
      "importRequiredTypes": true,
      "useGetBaseUrlMethod": false,
      "baseUrlTokenName": "API_BASE_URL",
      "queryNullValue": "",
      "useAbortSignal": false,
      "inlineNamedDictionaries": false,
      "inlineNamedAny": false,
      "includeHttpContext": false,
      "templateDirectory": null,
      "typeNameGeneratorType": null,
      "propertyNameGeneratorType": null,
      "enumNameGeneratorType": null,
      "serviceHost": null,
      "serviceSchemes": null,
      "output": "frontend/src/app/services/api-client.ts",
      "newLineBehavior": "Auto"
    }
  }
}
EOF

nswag run nswag-runtime.json

echo "Done!"
