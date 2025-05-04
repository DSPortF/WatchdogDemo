# WatchdogDemo

A demo watchdog that reboots the computer under certain failure scenarios.

1. Start with the .NET Joke Service from https://learn.microsoft.com/en-us/dotnet/core/extensions/windows-service
and confirm this all works OK.

Create the service in an elevated command prompt or PowerShell:
sc.exe create ".NET Joke Service" binpath= C:\_Dave\Dev\_Publish\WatchdogService\WatchdogService.exe"

Other useful commands:
sc.exe start ".NET Joke Service"
sc.exe stop ".NET Joke Service"

To find the jokes, look in Event Viewer - Windows Logs - Application

