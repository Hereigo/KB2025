### appsettings.config

```json
{ 
  "StrongTypedOption": {
    "Field1": "abcd",
    "Field2": "1234"
  },
  {
    "...":"..."
  }
}
```

```csharp
public sealed class StrongTypedOption
{
    public const string StrongTypedOptionSection = "StrongTypedOption";

    [Required]
    public string Field1 { get; init; }

    [Required]
    public int Field2 { get; init; }
}


// Program.cs

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<StrongTypedOption>()
                .BindConfiguration(StrongTypedOption.StrongTypedOptionSection);

WebApplication app = builder.Build();

app.MapGet("settings", (IOptions<StrongTypedOption> options) => {
    return Results.Ok(options.Value);
});

```