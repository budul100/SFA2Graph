@echo off

echo.
set /p "INPUTFILE=Enter path of lines file: "
echo.

echo.
echo *** Remove existing folder ***
echo.

rmdir /s /q .\RoutingGraph

echo.
echo *** Copy input file ***
echo.

xcopy /f /y "%INPUTFILE%" ".\Lines\lines.txt"

echo.
echo *** Create graph ***
echo.

C:\IVU.plan\bin64\ICR_MakeGraph.exe -v .\MakeGraph.param

echo.
echo *** Copy additional files ***
echo.

xcopy /e /i .\Template .\RoutingGraph

echo.
echo *** Create zip file ***
echo.

pushd .\RoutingGraph
tar -a -c -f ..\RoutingGraph.zip *
popd