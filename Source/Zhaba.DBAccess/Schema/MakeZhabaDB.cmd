@echo off

set MYSQL_HOME="C:\Program Files\MySQL\MySQL Server 5.7\bin\"

echo Building MYI Development DB ----------------------------------------------------
%MYSQL_HOME%mysql -u%MYSQL_ADMIN_USERID% -p%MYSQL_ADMIN_PASSWORD% < "MakeZhabaDB.sql" > makeDB.log 2>&1

if errorlevel 1 goto ERROR

echo Done!
goto :FINISH

:ERROR
echo Error happened!

:FINISH
pause
