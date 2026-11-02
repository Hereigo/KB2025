Here are three ways to set it on Windows. Whichever you pick, SQL Server requires a password of at least 8 characters that uses three of these four: uppercase, lowercase, digits, symbols. If the password is too weak, the container exits right after it starts.

**Option 1: current PowerShell window only (simplest)**
```powershell
$env:MSSQL_SA_PASSWORD = "Your_Str0ng!Pass"
```
```powershell
podman compose up -d
```
You have to set it again in each new terminal. Use single quotes if the password contains `$`.

**Option 2: permanently for your Windows user**
```powershell
[Environment]::SetEnvironmentVariable("MSSQL_SA_PASSWORD", "Your_Str0ng!Pass", "User")
```
Open a new terminal afterwards so it picks up the variable.

**Option 3: a `.env` file next to `compose.yaml`**
```
MSSQL_SA_PASSWORD=Your_Str0ng!Pass
```
`podman compose` reads `.env` from the project folder automatically. The repo's `.gitignore` already has `*.env`, which also covers `.env`, so the file won't be committed. Confirm with `git status` that it doesn't show up.

**Then point the API at the database** with the same password, stored in user secrets rather than appsettings:
```powershell
dotnet user-secrets init --project src/PROJECT_NAMEPlaybook.Api
```
```powershell
dotnet user-secrets set "ConnectionStrings:PROJECT_NAME" "Server=localhost,1433;Database=PROJECT_NAME;User Id=sa;Password=Your_Str0ng!Pass;TrustServerCertificate=True" --project src/PROJECT_NAME.Api # (for example)
```
`TrustServerCertificate=True` is only for the local container, which uses a self-signed certificate. `dotnet user-secrets init` adds a `UserSecretsId` line to the `.csproj`, which is a normal change to commit.

**Check that it's up:**
```powershell
podman compose ps
```
```powershell
podman compose logs sqlserver
```
In the logs, look for "SQL Server is now ready for client connections."

If `podman compose` reports that no compose provider was found, install one (`pip install podman-compose`, or Docker Compose). Podman delegates `compose` to an external tool.