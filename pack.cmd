@echo off
rem One-click release for WPE x64 : runs tools\pack\Pack.ps1 (bypassing execution policy)
rem Extra arguments are passed through, e.g.  pack.cmd -SkipBuild   (reuse the last build output)
rem NOTE: never put < > | & in echo text below - cmd treats them as redirection / pipes
setlocal
cd /d "%~dp0"
title WPE x64 - pack
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0tools\pack\Pack.ps1" %*
if errorlevel 1 (
    echo.
    echo ************  PACK FAILED  -  see the red messages above  ************
    echo.
    pause
    exit /b 1
)
echo.
echo ************  PACK OK  -  output: dist\WPE64 x.y.z.exe  ************
echo.
if exist "%~dp0dist" start "" "%~dp0dist"
pause
