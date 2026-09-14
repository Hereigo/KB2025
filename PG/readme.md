## PostgreSQL

```cmd
net stop postgresql-x64-16
```

```pgsql
CREATE DATABASE db1
    WITH
    OWNER = test_db_admin
    ENCODING = 'UTF8'
    LOCALE_PROVIDER = 'libc'
    CONNECTION LIMIT = -1
    IS_TEMPLATE = False;
```

```powershell
dotnet tool list -g
# if necessary:
dotnet tool install --global dotnet-ef


dotnet ef migrations add InitialCreate
dotnet ef database update
```

```sql
select * from "Todos" limit 100
```