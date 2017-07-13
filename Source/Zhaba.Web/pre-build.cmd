set SOLUTION_DIR=%1
set PROJECT_DIR=%~dp0

echo %PROJECT_DIR%

"%SOLUTION_DIR%packages\NFX.3.5.0.0\tools\ntc" "%PROJECT_DIR%Pages\*.nht" -r -ext ".auto.cs" -src -c "NFX.Templatization.NHTCompiler, NFX"
"%SOLUTION_DIR%packages\NFX.3.5.0.0\tools\ntc" "%PROJECT_DIR%Controls\*.nht" -r -ext ".auto.cs" -src -c "NFX.Templatization.NHTCompiler, NFX"
REM "%SOLUTION_DIR%packages\NFX.3.5.0.0\tools\ntc" "%PROJECT_DIR%Pages\__Masters\*.nht" -r -ext ".auto.cs" -src -c "NFX.Templatization.NHTCompiler, NFX"
REM "%SOLUTION_DIR%packages\NFX.3.5.0.0\tools\ntc" "%PROJECT_DIR%Pages\List\*.nht" -r -ext ".auto.cs" -src -c "NFX.Templatization.NHTCompiler, NFX"
REM "%SOLUTION_DIR%packages\NFX.3.5.0.0\tools\ntc" "%PROJECT_DIR%Pages\Edit\*.nht" -r -ext ".auto.cs" -src -c "NFX.Templatization.NHTCompiler, NFX"
REM "%SOLUTION_DIR%packages\NFX.3.5.0.0\tools\ntc" "%PROJECT_DIR%Pages\Reports\*.nht" -r -ext ".auto.cs" -src -c "NFX.Templatization.NHTCompiler, NFX"

"%SOLUTION_DIR%packages\NFX.3.5.0.0\tools\ntc" "%PROJECT_DIR%Static\ntc\zhb.ntc.js" -ext ".js" -replace ".ntc.js" -dest "%PROJECT_DIR%Static\js" -src -c "NFX.Templatization.TextJSTemplateCompiler, NFX" -o dom-gen="cmp{pretty=1}"

call lessc "%PROJECT_DIR%Static\less\zhb.less" "%PROJECT_DIR%Static\css\zhb.css"
call lessc "%PROJECT_DIR%Static\less\report.less" "%PROJECT_DIR%Static\css\report.css"