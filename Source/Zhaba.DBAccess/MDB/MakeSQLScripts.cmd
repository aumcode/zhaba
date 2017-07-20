@echo off

set DOMAINS="Zhaba.Data.Domains.*, Zhaba;NFX.RelationalModel.DataTypes.*, NFX"
set TOOLS=..\..\..\packages\NFX.3.5.0.1\tools
set DEBUG=..\..\..\Output\Debug

xcopy %TOOLS%\rsc.exe %DEBUG% /y

echo Building ZHABA main script ---------------------------------------------------
%DEBUG%\rsc.exe ..\Schema\db.rschema /o domain-search-paths=%DOMAINS%
if errorlevel 1 goto ERROR

echo Done!
goto :FINISH

:ERROR
echo Error happened!
:FINISH
pause


