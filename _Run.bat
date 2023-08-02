@echo off

set /p "INPUTFILE=Enter input path: "

for %%f in ("%INPUTFILE%") do set ROUTINGFILE=%%~dpnf.txt

echo.
echo Convert %INPUTFILE%
echo.

.\SFASimplifierCLI\bin\Debug\net6.0\SFASimplifierCLI.exe -i "%INPUTFILE%" -o "%ROUTINGFILE%"