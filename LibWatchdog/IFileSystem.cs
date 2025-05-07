namespace LibWatchdog;

public interface IFileSystem
{
    TimeSpan GetProcessFileAge();
    public bool ProcessFileExists();
}
