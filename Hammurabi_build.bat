:: This script converts Akkadian (.akk) files to C# (.cs) files
:: and then compiles the C# project in MonoDevelop.

:: Convert the .akk files to .cs
cd %~dp0\AkkadianCompiler\bin\Debug
AkkadianCompiler.exe

:: Build the MonoDevelop project (Hammurabi "core" and "law" components only)
cd C:\Program Files\MonoDevelop\bin
mdtool build --f --buildfile:"%~dp0\Hammurabi\Hammurabi.csproj"

:: Leave the DOS window open so user can see if there are any compilation errors
pause


:: NOTE: To pin this .bat file to the Windows 7 toolbar:
:: 1. Obey http://social.technet.microsoft.com/Forums/en-US/w7itproui/thread/a44e74a1-20cd-4924-8f2b-3e6b688f1ad7/
:: 2. Then paste the following line into the "Target" text box:
::    %windir%\system32\cmd.exe /C "C:\Users\...\Hammurabi\Hammurabi_build.bat"
