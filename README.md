# WatchdogDemo

A demo watchdog that reboots the computer under certain failure conditions.

From a customer perspective these conditions mean that the information displayed by the system is either missing or out of date.

Analysis of support tickets has shown that a high percentage of these problems are resolved by a reboot, and management want to reduce the number of tickets.

#### 1. Start with the .NET Joke Service  
https://learn.microsoft.com/en-us/dotnet/core/extensions/windows-service  
and confirm this all works OK.

Create the service in an elevated command prompt or PowerShell:
sc.exe create ".NET Joke Service" binpath= C:\_Dave\Dev\_Publish\WatchdogService\WatchdogService.exe"

Other useful commands:  
sc.exe start ".NET Joke Service"  
sc.exe stop ".NET Joke Service"  

To find the jokes, look in Event Viewer - Windows Logs - Application


#### 2. Build the other components:  
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

The original application had additional logic to ensure the system didn't get caught in a reboot loop.


#### 3. Check if a certain process is running.  
This isn't possible directly, so that process writes a text file periodically. We check if this file exists and if it is less than 15 minutes since the last update. If not then we reboot.  
The file is C:\FancyProcessStatusFile.txt  

To test this we'll use a mock filesystem object.

With the tests in place and all passing we now get the expected error at runtime:  
Description: The process was terminated due to an unhandled exception.  
Exception Info: System.InvalidOperationException: Unable to resolve service for type 'LibWatchdog.IFileSystem' while attempting to activate 'LibWatchdog.WatchdogImpl'.  
This is because of the modified constructor that requires an IFileSystem object.  


#### 4. Some customers only want the system to autoreboot during specific times.  
Ideally this would be in config but we'll just hardcode 0100-0400.

NB: If ShouldReboot() checks DateTime.Now directly then this means automated tests will be time dependent. Therefore we have pass in the current time.  

