@echo off

set /p "INPUTFILE=Enter input path: "

for %%f in ("%INPUTFILE%") do set ROUTINGFILE=%%~dpnf.txt

echo.
echo Convert %INPUTFILE%
echo.

.\SFA2GraphCLI\bin\Debug\net6.0\SFA2GraphCLI.exe -i "%INPUTFILE%" -o "%ROUTINGFILE%"