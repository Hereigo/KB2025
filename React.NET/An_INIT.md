
## ✅ A minimal React + .NET Setup Using VS Code

### 1. Create React App (Vite)
```bash
npm create vite@latest myapp.client -- --template react
cd myapp.client
npm install
npm run dev
```
Visit: **http://localhost:5173**

### 2. Create .NET Web API
```bash
dotnet new webapi -n MyApp.Api
cd MyApp.Api
dotnet run
```
Visit the default endpoint:  
`https://localhost:7xxx/WeatherForecast`  
If you see JSON, the API works.

### 3. Enable CORS (allow React to call the API)
In `Program.cs`:
```csharp
var policy = "AllowClient";
builder.Services.AddCors(options =>
{
    options.AddPolicy(policy, p =>
        p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
var app = builder.Build();
app.UseCors(policy);
```

### 4. Add Vite Proxy (optional but recommended)
In `vite.config.js`:
```js
server: {
  proxy: {
    '/api': 'https://localhost:7200'
  }
}
```

### 5. Call the API from React
```js
useEffect(() => {
  fetch('/api/WeatherForecast')
    .then(res => res.json())
    .then(console.log);
}, []);
```

---

## 🆚 Visual Studio vs VS Code (Minimal Setup)

| Feature | Visual Studio Template | Manual Vite + .NET (VS Code) |
|--------|------------------------|-------------------------------|
| Setup speed | ⭐⭐⭐⭐⭐ | ⭐⭐⭐ |
| Control over structure | ⭐⭐ | ⭐⭐⭐⭐⭐ |
| Uses Vite | Yes (new template)   [Microsoft Learn](https://learn.microsoft.com/en-us/visualstudio/javascript/tutorial-asp-net-core-with-react?view=visualstudio) | Yes |
| Best for learning internals | Medium | High |
| Best for enterprise | High | High (with TypeScript) |

---

## 🎯 Recommendation for You
If you want **the absolute minimal setup**, use **VS Code + Vite + Web API**.  
If you want **the easiest setup**, use **Visual Studio’s React + ASP.NET Core template**.

---

## 📌 Follow‑up question
Would you like me to generate a **starter folder structure** or a **sample API + React component** that calls the backend?