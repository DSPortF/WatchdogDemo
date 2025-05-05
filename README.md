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


2. Build the other components:
(a) a class library to implement the logic, initially comprising a single function that returns false;
(b) an XUnit project to test the class library.

Remove any remaining references to the Joke service.

New service maintenance commands are:
sc.exe create "DS Watchdog Service" binpath= C:\_Dave\Dev\_Publish\WatchdogService\WatchdogService.exe"
sc.exe start "DS Watchdog Service"
sc.exe stop "DS Watchdog Service"


Note on the sentinel file C:\\WatchdogRebootSentinel.txt:  
This is a debugging technique I sometimes use. It gives manual control over critical parts of the application.  
If the file exists, the watchdog will not reboot the computer.  
If the file does not exist, the watchdog will recreate the file then reboot the computer.  
This will prevent repeated reboots in case the logic is not quite right.  

