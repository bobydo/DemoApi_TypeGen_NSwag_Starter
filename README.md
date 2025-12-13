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

dotnet new webapi
dotnet run

dotnet tool install --global TypeGen
typegen generate

dotnet tool install --global NSwag.ConsoleCore
nswag run
