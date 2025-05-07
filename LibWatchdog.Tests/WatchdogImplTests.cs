namespace LibWatchdog.Tests;

public class WatchdogImplTests
{
    private static readonly DateTime DTInsideRebootWindow = new DateTime(2025, 5, 7, 2, 0, 0);
    private static readonly DateTime DTOutsideRebootWindow = new DateTime(2025, 5, 7, 11, 0, 0);

    [Fact]
    public void ShouldReboot_ReturnsTrue_IfProcessFileNotPresent()
    {
        var testFS = new MockFileSystem();
        testFS.SetProcessFileExists(false);

        var sut = new WatchdogImpl(testFS);
        Assert.True(sut.ShouldReboot(DTInsideRebootWindow));
    }

    [Fact]
    public void ShouldReboot_ReturnsFalse_IfProcessFilePresentAndFresh()
    {
        var testFS = new MockFileSystem();
        testFS.SetProcessFileExists(true);
        testFS.SetProcessFileAge(TimeSpan.FromMinutes(1));

        var sut = new WatchdogImpl(testFS);
        Assert.False(sut.ShouldReboot(DTInsideRebootWindow));
    }

    [Fact]
    public void ShouldReboot_ReturnsTrue_IfProcessFilePresentAndOutOfDate()
    {
        var testFS = new MockFileSystem();
        testFS.SetProcessFileExists(true);
        testFS.SetProcessFileAge(TimeSpan.FromMinutes(30));

        var sut = new WatchdogImpl(testFS);
        Assert.True(sut.ShouldReboot(DTInsideRebootWindow));
    }

    [Fact]
    public void ShouldReboot_ReturnsFalse_IfOutsideDesignatedTimeslot()
    {
        var testFS = new MockFileSystem();
        testFS.SetProcessFileExists(false);

        var sut = new WatchdogImpl(testFS);
        Assert.False(sut.ShouldReboot(DTOutsideRebootWindow));
    }
}
