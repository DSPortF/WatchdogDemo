namespace LibWatchdog;

public class FileSystem : IFileSystem
{
    private const string processFilePath = "C:\\FancyProcessStatusFile.txt";

    public TimeSpan GetProcessFileAge()
    {
        // Last write time of a missing file is 01/01/1601 00:00:00 which suits us well
        DateTime lastWriteTime = File.GetLastWriteTime(processFilePath);
        DateTime now = DateTime.Now;
        TimeSpan age = now - lastWriteTime;
        return age;
    }

    public bool ProcessFileExists()
    {
        return File.Exists(processFilePath);
    }
}
