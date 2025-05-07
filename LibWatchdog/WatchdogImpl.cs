namespace LibWatchdog;

public class WatchdogImpl
{
    private readonly IFileSystem _fileSystem;

    public WatchdogImpl(IFileSystem fs)
    {
        _fileSystem = fs;
    }

    public bool ShouldReboot(DateTime currentTime)
    {
        // If we're outside the designated reboot window, don't reboot (even if other conditions mean we should)
        var currentTimeOfDay = currentTime.TimeOfDay;
        var startTime = new TimeSpan(1, 0, 0); // 1:00 AM
        var endTime = new TimeSpan(4, 0, 0);   // 4:00 AM

        if (currentTimeOfDay < startTime || currentTimeOfDay > endTime)
            return false;

        // If the process file doesn't exist, we should reboot
        if (!_fileSystem.ProcessFileExists())
            return true;

        // If the file exists and is older than 15 minutes, we should reboot
        if (_fileSystem.GetProcessFileAge().TotalMinutes > 15)
            return true;

        // If there's no reason to reboot, don't.
        return false;
    }
}
