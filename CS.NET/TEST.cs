// #!/usr/bin/env dotnet run
// Uncomment the line above to run it on Uix or similar environments.

// - if - dotnet --version >= 10.x you can:
// dotnet run TEST.cs

// - To create a Project you can:
// dotnet project convert TEST.cs -o TEST.csproj

#:sdk Microsoft.NET.Sdk.Web
#:package Microsoft.AspNetCore.OpenApi@10.*-*
#:package Humanizer@2.14.1

using Humanizer;

var builder = WebApplication.CreateBuilder();

builder.Services.AddOpenApi();

var app = builder.Build();

var dotNet9Released = DateTimeOffset.Parse("2024-12-03");
var since = DateTimeOffset.Now - dotNet9Released;
var outputString = $"It has been {since.Humanize()} since .NET 9 was released.";

app.MapGet("/", () => outputString);
app.Run();