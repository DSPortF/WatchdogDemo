namespace LibWatchdog.Tests;

public class WatchdogImplTests
{
    [Fact]
    public void ShouldReboot_InitialState_ReturnsFalse()
    {
        var sut = new LibWatchdog.WatchdogImpl();
        Assert.False(sut.ShouldReboot());
    }
}
