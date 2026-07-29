In an **ASP.NET MVC .NET Framework 4.8** application, you can restrict verbose error output (such as detailed stack traces or yellow screens of death) from being displayed to end users in several ways. Here are the most common and effective approaches:

---

## 1. **Web.config Configuration**

### **Custom Errors Section**
Enable custom errors in your `Web.config` to show user-friendly error pages instead of detailed errors:

```xml
<system.web>
  <customErrors mode="On" defaultRedirect="~/Error">
    <error statusCode="403" redirect="~/Error/Forbidden" />
    <error statusCode="404" redirect="~/Error/NotFound" />
    <error statusCode="500" redirect="~/Error/ServerError" />
  </customErrors>
</system.web>
```
- `mode="On"`: Shows custom error pages to remote users and detailed errors to local users.
- `mode="RemoteOnly"`: Shows custom error pages to remote users, detailed errors to local users.
- `mode="Off"`: Shows detailed errors to everyone (not recommended for production).

### **Compilation Debug Attribute**
Ensure debug is off in production:

```xml
<system.web>
  <compilation debug="false" targetFramework="4.8" />
</system.web>
```

---

## 2. **HTTP Modules and Handlers**

### **Remove or Restrict Trace Output**
Disable trace output in `Web.config`:

```xml
<system.web>
  <trace enabled="false" localOnly="true" />
</system.web>
```

---

## 3. **Global Error Handling**

### **Application_Error in Global.asax**
Handle errors globally and redirect to a custom error page:

```csharp
protected void Application_Error()
{
    Exception exception = Server.GetLastError();
    Server.ClearError(); // Clear the error to prevent the default error page

    Response.Redirect("~/Error/ServerError");
}
```

### **Custom Error Controller**
Create a controller to handle different error types:

```csharp
public class ErrorController : Controller
{
    public ActionResult ServerError()
    {
        return View();
    }
    public ActionResult NotFound()
    {
        return View();
    }
    public ActionResult Forbidden()
    {
        return View();
    }
}
```

---

## 4. **Filter Config (MVC-Specific)**

### **Register Global Error Filter**
In `FilterConfig.cs`:

```csharp
public class FilterConfig
{
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
        filters.Add(new HandleErrorAttribute());
    }
}
```
- This will use the default `Error.cshtml` view in the `Shared` folder.

---

## 5. **IIS Settings (Optional)**

- In IIS, ensure that **Error Pages** are configured to show custom pages and not detailed errors.
- Disable **Detailed Errors** in IIS Manager for your site.

---

## 6. **Environment-Specific Configuration**

Use web.config transforms to ensure these settings are only applied in production:

```xml
<!-- Web.Release.config -->
<system.web>
  <customErrors mode="On" defaultRedirect="~/Error" xdt:Transform="SetAttributes" />
  <compilation debug="false" xdt:Transform="SetAttributes" />
  <trace enabled="false" xdt:Transform="SetAttributes" />
</system.web>
```

---

## **Summary Table**


Error Restriction Methods


| Method                | Location                | Effect                                      |
|-----------------------|-------------------------|---------------------------------------------|
| customErrors          | Web.config              | Shows custom error pages to users           |
| debug="false"         | Web.config              | Disables debug mode (detailed errors)       |
| trace enabled="false" | Web.config              | Disables trace output                        |
| Application_Error     | Global.asax             | Global error handling and redirection       |
| HandleErrorAttribute  | FilterConfig.cs         | MVC-specific error handling                  |
| IIS Error Pages       | IIS Manager             | Configures error pages at the server level  |

---

**Tip:** Always test your error pages in a staging environment to ensure they work as expected and do not leak sensitive information.

---