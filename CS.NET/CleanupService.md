
## Compare-and-swap pattern (prevent concurrent runs)

```csharp
  
public class DocumentCleanupService : IDocumentCleanupService
{
    private long _lastRunTicks;
    private readonly TimeSpan CleanupRunInterval = TimeSpan.FromHours(24);
    private readonly TimeSpan MaxFilesAge = TimeSpan.FromDays(2);

    public async Task RunAsync()
    {
        var nowTicks = DateTime.UtcNow.Ticks;

        var lastRun = Interlocked.Read(ref _lastRunTicks);
        
        if (lastRun != 0 && new TimeSpan(nowTicks - lastRun) < MinInterval)
            return;
        
        // Ensure only one thread runs cleanup per interval
        if (Interlocked.CompareExchange(ref _lastRunTicks, nowTicks, lastRun) != lastRun)
            return;
        
        await RunCleanup(); // Dirs, Types, Dates logic inside.
        
        // Atomically publish the new timestamp
        Interlocked.Exchange(ref _lastRunTicks, DateTime.UtcNow.Ticks);
        
        return Task.CompletedTask;
    }
}
```