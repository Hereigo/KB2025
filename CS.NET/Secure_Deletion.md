## Secure file deletion (making recovery much harder).

```cs
using System.Security.Cryptography;

public static class SecureFileDeleter
{
    public static void SecureDelete(string filePath, int passes = 3)
    {
        if (!File.Exists(filePath))
            return;

        FileInfo fileInfo = new FileInfo(filePath);
        long length = fileInfo.Length;

        using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Write))
        {
            byte[] buffer = new byte[4096];
            RandomNumberGenerator rng = RandomNumberGenerator.Create();

            for (int pass = 0; pass < passes; pass++)
            {
                long remaining = length;
                while (remaining > 0)
                {
                    rng.GetBytes(buffer);
                    int toWrite = (int)Math.Min(buffer.Length, remaining);
                    stream.Write(buffer, 0, toWrite);
                    remaining -= toWrite;
                }
                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin); // reset for next pass
            }
        }
        // Finally delete the file
        File.Delete(filePath);
    }
}

```

#### Below a simplified version (single overwrite pass with zeros) for cases where performance matters more than maximum security.

```cs
public static class SimpleSecureDelete
{
    public static void SecureDelete(string filePath)
    {
        if (!File.Exists(filePath))
            return;

        FileInfo fileInfo = new FileInfo(filePath);
        long length = fileInfo.Length;

        // Overwrite with zeros
        using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Write))
        {
            byte[] buffer = new byte[4096];
            Array.Clear(buffer, 0, buffer.Length);

            long remaining = length;
            while (remaining > 0)
            {
                int toWrite = (int)Math.Min(buffer.Length, remaining);
                stream.Write(buffer, 0, toWrite);
                remaining -= toWrite;
            }

            stream.Flush();
        }
        // Finally delete the file
        File.Delete(filePath);
    }
}
```
#### The same but async.

```cs
using System.Threading.Tasks;

public static class AsyncSecureDelete
{
    public static async Task SecureDeleteAsync(string filePath)
    {
        if (!File.Exists(filePath))
            return;

        FileInfo fileInfo = new FileInfo(filePath);
        long length = fileInfo.Length;

        // Overwrite with zeros asynchronously
        using (FileStream stream = new FileStream(
            filePath, FileMode.Open, FileAccess.Write, FileShare.None,
            bufferSize: 4096, useAsync: true))
        {
            byte[] buffer = new byte[4096];
            Array.Clear(buffer, 0, buffer.Length);

            long remaining = length;
            while (remaining > 0)
            {
                int toWrite = (int)Math.Min(buffer.Length, remaining);
                await stream.WriteAsync(buffer, 0, toWrite);
                remaining -= toWrite;
            }

            await stream.FlushAsync();
        }

        // Finally delete the file
        File.Delete(filePath);
    }
}
```