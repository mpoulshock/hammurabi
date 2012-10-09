:: This script converts Akkadian (.akk) files to C# (.cs) files
:: and then compiles the C# project in MonoDevelop.

:: Convert the .akk files to .cs
cd %~dp0\AkkadianCompiler\bin\Debug
AkkadianCompiler.exe

:: Build the MonoDevelop project (Hammurabi "core" and "law" components only)
:: Note: You may have to change the file path to the MonoDevelop bin folder, 
:: depending on where it is on your machine.
cd C:\Program Files\MonoDevelop\bin
mdtool build --f --buildfile:"%~dp0\Hammurabi\Hammurabi.csproj"

:: Build Hammurabi Interactive
mdtool build --f --buildfile:"%~dp0\HammurabiInteractive\HammurabiInteractive.csproj"

:: Start the interactive test interview
cd %~dp0\HammurabiInteractive\bin\Debug
HammurabiInteractive.exe

:: Leave the DOS window open so user can see if there are any compilation errors
:: pause


:: NOTE: To pin this .bat file to the Windows 7 taskbar:
:: 1. Obey http://social.technet.microsoft.com/Forums/en-US/w7itproui/thread/a44e74a1-20cd-4924-8f2b-3e6b688f1ad7/
:: 1. In the Windows Start menu, type "cmd" into the search box.
:: 2. On the "cmd" program icon that appears (it looks like a DOS window), right click and select "Pin to task bar."
:: 3. On the icon in the taskbar, right click, and then right click again on "Command Prompt."
:: 4. Right click on "Properties."
:: 6. Then paste the following line into the "Target" text box (replacing the "..." with the appropriate file path:
::    %windir%\system32\cmd.exe /C "C:\Users\...\Hammurabi\Hammurabi_build.bat"
















