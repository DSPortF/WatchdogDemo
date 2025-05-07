using App.WindowsService;
using LibWatchdog;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;

namespace WatchdogService;

public class Program
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Application is Windows-only")]
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddWindowsService(options =>
        {
            options.ServiceName = "DS Watchdog Service";
        });

        LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(builder.Services);

        builder.Services.AddSingleton<IFileSystem, FileSystem>();
        builder.Services.AddSingleton<WatchdogImpl>();
        builder.Services.AddHostedService<WindowsBackgroundService>();
        var host = builder.Build();
        host.Run();
    }
}
