@echo off

set NFX_HOME=%AGNICORE_HOME%\NFX\
set ZHABA_HOME=%AGNICORE_HOME%\Zhaba\
set RSC_EXE="%ZHABA_HOME%Output\Debug\rsc.exe"
set DOMAINS="Zhaba.Data.Domains.*, Zhaba"

echo Building ZHABA main script ---------------------------------------------------
echo %RSC_EXE% "%ZHABA_HOME%Source\Zhaba.DBAccess\Schema\db.rschema" /o domain-search-paths=%DOMAINS%
%RSC_EXE% "%ZHABA_HOME%Source\Zhaba.DBAccess\Schema\db.rschema" /o domain-search-paths=%DOMAINS%
if errorlevel 1 goto ERROR

echo Done!
goto :FINISH

:ERROR
echo Error happened!
:FINISH
pause


