@echo off
setlocal
set PHP_EXE=D:\xampp82\php\php.exe
if not exist "%PHP_EXE%" set PHP_EXE=php
"%PHP_EXE%" "%~dp0generate_authorization_key.php" %*
endlocal
