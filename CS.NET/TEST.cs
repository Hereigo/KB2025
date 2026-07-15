// #!/usr/bin/env dotnet run
// Uncomment the line above to run it on Uix or similar environments.

// - if - dotnet --version >= 10.x you can:
// dotnet run TEST.cs

// - To create a Project you can:
// dotnet project convert TEST.cs -o TEST.csproj

Console.WriteLine("\r\n Hello World! \r\n");

class Program
{
    static async Task Main()
    {
        string baseDocs = @"\Service\Files\DocumentLibrary";
        string baseLogs = @"\Service\Logs\Requests";

        Console.WriteLine("=== DocumentLibrary Files ===");
        await foreach (var file in GetDocumentLibraryFilesAsync(baseDocs))
            Console.WriteLine(file);

        Console.WriteLine("=== Request Logs ===");
        await foreach (var file in GetRequestLogFilesAsync(baseLogs))
            Console.WriteLine(file);
    }

    static async IAsyncEnumerable<string> GetDocumentLibraryFilesAsync(string rootPath)
    {
        if (!Directory.Exists(rootPath))
            yield break;

        var validExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { ".jpg", ".jpeg", ".png", ".pdf" };

        // Regex for folder pattern: YYYY-MM-DD\GUID
        var folderPattern = new Regex(@"^\d{4}-\d{2}-\d{2}$");
        var guidPattern = new Regex(@"^[0-9a-fA-F]{32}$");

        foreach (var dateDir in Directory.EnumerateDirectories(rootPath))
        {
            if (!folderPattern.IsMatch(Path.GetFileName(dateDir)))
                continue;

            foreach (var guidDir in Directory.EnumerateDirectories(dateDir))
            {
                if (!guidPattern.IsMatch(Path.GetFileName(guidDir)))
                    continue;

                foreach (var file in Directory.EnumerateFiles(guidDir))
                {
                    if (validExtensions.Contains(Path.GetExtension(file)))
                    {
                        await Task.Yield();
                        yield return Path.GetFullPath(file);
                    }
                }
            }
        }
    }

    static async IAsyncEnumerable<string> GetRequestLogFilesAsync(string rootPath)
    {
        if (!Directory.Exists(rootPath))
            yield break;

        var validExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { ".log" };

        // Regex for folder pattern: YYYY_MM\DD
        var monthPattern = new Regex(@"^\d{4}_\d{2}$");
        var dayPattern = new Regex(@"^\d{2}$");

        foreach (var monthDir in Directory.EnumerateDirectories(rootPath))
        {
            if (!monthPattern.IsMatch(Path.GetFileName(monthDir)))
                continue;

            foreach (var dayDir in Directory.EnumerateDirectories(monthDir))
            {
                if (!dayPattern.IsMatch(Path.GetFileName(dayDir)))
                    continue;

                foreach (var file in Directory.EnumerateFiles(dayDir))
                {
                    if (validExtensions.Contains(Path.GetExtension(file)))
                    {
                        await Task.Yield();
                        yield return Path.GetFullPath(file);
                    }
                }
            }
        }
    }
}
