@echo off
setlocal enabledelayedexpansion

rem Specify the directory containing the .html files
set "target_dir=path\to\your\Views\Home"

rem Change to the target directory
cd /d "%target_dir%"

rem Rename .html files to .cshtml
for %%f in (*.html) do (
    set "filename=%%~nf"
    ren "%%f" "!filename!.cshtml"
)

echo Renaming complete.
