```cs
using System;
using System.IO;

class Program
{
    static void Main()
    {
        string path = @"C:\MyFolder";

        // Get all entries (files + directories)
        string[] entries = Directory.GetFileSystemEntries(path);

        foreach (string entry in entries)
        {
            if (File.Exists(entry))
            {
                // Action for files
                Console.WriteLine($"File: {entry}");
                // e.g., process file content
            }
            else if (Directory.Exists(entry))
            {
                // Action for directories
                Console.WriteLine($"Directory: {entry}");
                // e.g., recurse into subdirectory
            }
        }
    }
}

```