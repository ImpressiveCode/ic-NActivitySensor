@ECHO off
set AddinDir=%userprofile%\Documents\Visual Studio 2012\Addins
set AddinFileName=NActivitySensor.AddIn

set AddinContentDir=C:\NActivitySensor

if not exist "%AddinDir%" mkdir "%AddinDir%"

echo Deleting old files...
if exist %AddinDir%\%AddinFileName% del /Q %AddinDir%\%AddinFileName%
if exist %AddinContentDir% del /Q %AddinContentDir%\*

echo Installing plugin...
if not exist %AddinContentDir% mkdir %AddinContentDir%
copy .\bin\* %AddinContentDir%\

echo %AddinDir%
copy .\NActivitySensor.AddIn "%AddinDir%\"

PAUSE