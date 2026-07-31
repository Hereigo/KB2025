In ASP.NET Core (.NET 9), debug and trace information is **not shown to users by default**. If you're seeing detailed exception pages, stack traces, or debugging information in the browser, it's usually because the application is running in the **Development** environment or because exception handling is configured incorrectly.

Here are the recommended ways to prevent debug and trace information from appearing on web pages.

### 1. Use the correct environment

Ensure your production server is running with:

```text
ASPNETCORE_ENVIRONMENT=Production
```

or

```text
DOTNET_ENVIRONMENT=Production
```

In Production, the Developer Exception Page should never be enabled.

---

### 2. Configure exception handling correctly

In `Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.Run();
```

The `UseDeveloperExceptionPage()` middleware displays:

* Stack traces
* Source code snippets
* Request headers
* Query strings
* Loaded assemblies

It should only run in Development.

---

### 3. Create a friendly error page

Instead of exposing exception details:

```csharp
app.UseExceptionHandler("/Error");
```

Example controller:

```csharp
public class ErrorController : Controller
{
    [Route("/Error")]
    public IActionResult Error()
    {
        return View();
    }
}
```

The view might simply say:

> An unexpected error occurred. Please try again later.

---

### 4. Log exceptions instead of displaying them

Example:

```csharp
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>();

        if (exception != null)
        {
            var logger = context.RequestServices
                .GetRequiredService<ILogger<Program>>();

            logger.LogError(exception.Error, "Unhandled exception");
        }

        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An unexpected error occurred.");
    });
});
```

Users see only a generic message while the full details are written to your logs.

---

### 5. Restrict logging verbosity

In `appsettings.Production.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft": "Warning"
    }
  }
}
```

or even:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Error"
    }
  }
}
```

This controls what is written to logs, not what users see.

---

### 6. Don't return exception messages to clients

Avoid code like:

```csharp
catch (Exception ex)
{
    return Results.BadRequest(ex.ToString());    // ❌
}
```

or

```csharp
return Content(ex.StackTrace);                  // ❌
```

Instead:

```csharp
catch (Exception ex)
{
    logger.LogError(ex, "Unexpected error");

    return Results.Problem(
        title: "An unexpected error occurred.",
        statusCode: 500);
}
```

---

### 7. Disable detailed errors

If you're using Kestrel or IIS, don't enable detailed errors in production.

For example:

```json
{
  "DetailedErrors": false
}
```

---

### 8. Avoid exposing sensitive information in custom middleware

Be careful not to return:

* Stack traces
* Inner exceptions
* Connection strings
* SQL queries
* File paths
* Environment variables
* Secrets or API keys

Instead, log those details internally.

---

## Typical production configuration

```csharp
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
```

This configuration ensures that:

* **Development**: Developers see detailed exception pages for debugging.
* **Production**: Users receive a generic error page, while detailed exception information is recorded in the application logs rather than exposed in the browser.

This approach follows the recommended security practice of avoiding information disclosure that could help an attacker understand your application's internals.
