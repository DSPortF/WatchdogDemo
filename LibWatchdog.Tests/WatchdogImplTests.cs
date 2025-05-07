namespace LibWatchdog.Tests;

public class WatchdogImplTests
{
    [Fact]
    public void ShouldReboot_ReturnsTrue_IfProcessFileNotPresent()
    {
        var testFS = new MockFileSystem();
        testFS.SetProcessFileExists(false);

        var sut = new WatchdogImpl(testFS);
        Assert.True(sut.ShouldReboot());
    }

    [Fact]
    public void ShouldReboot_ReturnsFalse_IfProcessFilePresentAndFresh()
    {
        var testFS = new MockFileSystem();
        testFS.SetProcessFileExists(true);
        testFS.SetProcessFileAge(TimeSpan.FromMinutes(1));

        var sut = new WatchdogImpl(testFS);
        Assert.False(sut.ShouldReboot());
    }

    [Fact]
    public void ShouldReboot_ReturnsTrue_IfProcessFilePresentAndOutOfDate()
    {
        var testFS = new MockFileSystem();
        testFS.SetProcessFileExists(true);
        testFS.SetProcessFileAge(TimeSpan.FromMinutes(30));

        var sut = new WatchdogImpl(testFS);
        Assert.True(sut.ShouldReboot());
    }
}
