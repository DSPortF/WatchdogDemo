using LibWatchdog;

namespace App.WindowsService;

public sealed class WindowsBackgroundService(
    WatchdogImpl watchdogImpl,
    ILogger<WindowsBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (watchdogImpl.ShouldReboot(DateTime.Now))
                {
                    logger.LogWarning("Should reboot");
                    Reboot();
                }
                else
                {
                    // Temporarily here for debug; will remove at some point
                    logger.LogWarning("Should not reboot");
                }

                // For debugging I don't want to have to keep waiting a minute
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

                // await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            // When the stopping token is canceled, for example, a call made from services.msc,
            // we shouldn't exit with a non-zero exit code. In other words, this is expected...
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);

            // Terminates this process and returns an exit code to the operating system.
            // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
            // performs one of two scenarios:
            // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
            // 2. When set to "StopHost": will cleanly stop the host, and log errors.
            //
            // In order for the Windows Service Management system to leverage configured
            // recovery options, we need to terminate the process with a non-zero exit code.
            Environment.Exit(1);
        }
    }

    private void Reboot()
    {
        logger.LogWarning("Rebooting the system");

        string sentinel = "C:\\WatchdogRebootSentinel.txt";
        if (File.Exists(sentinel))
        {
            logger.LogWarning($"Reboot prevented by file {sentinel}");
            return;
        }

        // If the sentinel file doesn't exist we'll reboot, but create a new one to prevent repeated reboots
        File.Create(sentinel).Dispose(); // Dispose ensures the file handle is released

        // This is commented out for debugging purposes, so we don't actually reboot the system
        logger.LogWarning("Reboot commented out");
        //var process = new Process
        //{
        //    StartInfo = new ProcessStartInfo
        //    {
        //        FileName = "shutdown",
        //        Arguments = "/r /t 0",
        //        RedirectStandardOutput = true,
        //        UseShellExecute = false,
        //        CreateNoWindow = true,
        //    }
        //};
    }
}
