If you're using the **.NET 10 SDK**, the simplest way is to use **file-based apps**, which let you build and run a single `.cs` file without creating a project.

Example `Hello.cs`:

```csharp
Console.WriteLine("Hello, .NET 10!");
```

Run it directly from the command line:

```cmd
dotnet run Hello.cs
```

or

```cmd
dotnet run .\Hello.cs
```

That's it—no `.csproj`, no solution, no project creation.

### With command-line arguments

```cmd
dotnet run Hello.cs one two three
```

Access them normally:

```csharp
Console.WriteLine(string.Join(", ", args));
```

### Using additional libraries

You can reference NuGet packages directly in the file:

```csharp
#:package Humanizer@2.14.1

using Humanizer;

Console.WriteLine("12345".Humanize());
```

Then run:

```cmd
dotnet run Program.cs
```

The SDK automatically restores the package.

### Build an executable

You can publish the single-file app without creating a project:

```cmd
dotnet publish Hello.cs -o out
```

or a self-contained executable:

```cmd
dotnet publish Hello.cs -c Release -r win-x64 --self-contained true
```

### If you're using an older .NET SDK

For .NET 8 and earlier, there is **no official single-file-without-project** workflow. Your options are:

* Create a project once:

  ```cmd
  dotnet new console
  dotnet run
  ```
* Use the C# Interactive compiler:

  ```cmd
  csi script.csx
  ```

  (scripts, not regular console apps)
* Use third-party tools such as `dotnet-script`.

For **.NET 10**, however, `dotnet run YourFile.cs` is the intended and simplest way to run a standalone C# source file.
