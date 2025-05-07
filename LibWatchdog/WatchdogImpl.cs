namespace LibWatchdog;

public class WatchdogImpl
{
    private readonly IFileSystem _fileSystem;

    public WatchdogImpl(IFileSystem fs)
    {
        _fileSystem = fs;
    }

    public bool ShouldReboot()
    {
        // If the process file doesn't exist, we should reboot
        if (!_fileSystem.ProcessFileExists())
        {
            return true;
        }

        // If the file exists and is older than 15 minutes, we should reboot
        if (_fileSystem.GetProcessFileAge().TotalMinutes > 15)
        {
            return true;
        }

        // If there's no reason to reboot, don't.
        return false;
    }
}
