@echo off
set libs=%cd%\Libraries\
echo Crab Game Libraries Copying tool by JNNJ
echo.
echo.
echo Enter your Crab Game installation Directory(You need BepInEx installed)...
set /p dir=""
set dir=%dir%\BepInEx
mkdir Libraries
xcopy /s /Y /C "%dir%\interop\" "%libs%"
echo Done!
pause