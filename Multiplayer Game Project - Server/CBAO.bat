REM Important that Delayed Expansion is Enabled
setlocal enabledelayedexpansion
REM This sets what folder the batch is looking for and the root in which it starts the search:
set foldername=%1
set root=%2
REM Checks each directory in the given root
FOR /R %root% %%A IN (.) DO (
    if '%%A'=='' goto end   
    REM Correctly parses info for executing the loop and RM functions
    set dir="%%A"
    set dir=!dir:.=!
    set directory=%%A
    set directory=!directory::=!
    set directory=!directory:\=;!   
    REM Checks each directory
    for /f "tokens=* delims=;" %%P in ("!directory!") do call :loop %%P
)
REM After each directory is checked the batch will allow you to see folders deleted.
:end
endlocal

REM This loop checks each folder inside the directory for the specified folder name. This allows you to check multiple nested directories.
:loop
if '%1'=='' goto endloop
if '%1'=='%foldername%' (
    rd /S /Q !dir!
    echo !dir! was deleted.
)
SHIFT
goto :loop
:endloop