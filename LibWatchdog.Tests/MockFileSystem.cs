

namespace LibWatchdog.Tests;

internal class MockFileSystem : IFileSystem
{
    private bool _processFileExists = false;
    private TimeSpan _processFileAge;

    public MockFileSystem()
    {
    }

    public TimeSpan GetProcessFileAge()
    {
        return _processFileAge;
    }

    public bool ProcessFileExists()
    {
        return _processFileExists;
    }

    internal void SetProcessFileAge(TimeSpan timeSpan)
    {
        _processFileAge = timeSpan;
    }

    internal void SetProcessFileExists(bool v)
    {
        _processFileExists = v;
    }
}
