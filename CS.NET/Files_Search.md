The simplest approach is to:

1. Enumerate all files recursively.
2. Read each file.
3. Check whether the file contains a match for your regular expression.
4. Return only the files that **do not** contain a match.

Example:

```csharp
using System.Text.RegularExpressions;

string root = @"C:\MyFolder";
Regex regex = new(@"your\s+pattern", RegexOptions.Compiled);

var filesWithoutMatch = Directory
    .EnumerateFiles(root, "*", SearchOption.AllDirectories)
    .Where(file =>
    {
        string text = File.ReadAllText(file);
        return !regex.IsMatch(text);
    });

foreach (var file in filesWithoutMatch)
{
    Console.WriteLine(file);
}
```

### More memory-efficient version

For very large files, avoid reading the entire file into memory:

```csharp
using System.Text.RegularExpressions;

string root = @"C:\MyFolder";
Regex regex = new(@"your\s+pattern", RegexOptions.Compiled);

foreach (var file in Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories))
{
    bool found = false;

    foreach (var line in File.ReadLines(file))
    {
        if (regex.IsMatch(line))
        {
            found = true;
            break;
        }
    }

    if (!found)
        Console.WriteLine(file);
}
```

### Handling binary files or unreadable files

If the directory may contain binaries or files you don't have permission to read:

```csharp
using System.Text.RegularExpressions;

string root = @"C:\MyFolder";
Regex regex = new(@"your\s+pattern", RegexOptions.Compiled);

foreach (var file in Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories))
{
    try
    {
        bool found = File.ReadLines(file).Any(regex.IsMatch);

        if (!found)
            Console.WriteLine(file);
    }
    catch (IOException)
    {
        // Skip file
    }
    catch (UnauthorizedAccessException)
    {
        // Skip file
    }
}
```

### Using the new `EnumerateFiles` overload (.NET 6+)

You can also skip inaccessible directories automatically:

```csharp
var options = new EnumerationOptions
{
    RecurseSubdirectories = true,
    IgnoreInaccessible = true
};

foreach (var file in Directory.EnumerateFiles(root, "*", options))
{
    // ...
}
```

This is generally the preferred approach on modern .NET versions.
