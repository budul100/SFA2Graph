@echo off

echo.
set /p "INPUTFILE=Enter path of geojson file: "

echo.
echo Convert %INPUTFILE%
echo.

.\SFA2GraphCLI\bin\Debug\net6.0\SFA2GraphCLI.exe -i "%INPUTFILE%" -o ".\Additionals\MakeGrph\Lines\lines.txt"

echo.
echo *** Remove existing folder ***
echo.

rmdir /s /q .\Additionals\MakeGrph\RoutingGraph

echo.
echo *** Create graph ***
echo.

pushd .\Additionals\MakeGrph

C:\IVU.plan\bin64\ICR_MakeGraph.exe -v .\MakeGraph.param

popd

echo.
echo *** Copy additional files ***
echo.

xcopy /d /e /i .\Additionals\MakeGrph\Template .\Additionals\MakeGrph\RoutingGraph

echo.
echo *** Create zip file ***
echo.

pushd .\Additionals\MakeGrph\RoutingGraph

tar -a -c -f ..\RoutingGraph.zip *

popd