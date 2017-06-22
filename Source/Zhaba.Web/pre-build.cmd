set SOLUTION_DIR=%1
set PROJECT_DIR=%~dp0

echo %PROJECT_DIR%

"%SOLUTION_DIR%packages\NFX.3.4.0.1\tools\ntc" "%PROJECT_DIR%Pages\*.nht" -r -ext ".auto.cs" -src -c "NFX.Templatization.NHTCompiler, NFX"
"%SOLUTION_DIR%packages\NFX.3.4.0.1\tools\ntc" "%PROJECT_DIR%Pages\__Masters\*.nht" -r -ext ".auto.cs" -src -c "NFX.Templatization.NHTCompiler, NFX"
"%SOLUTION_DIR%packages\NFX.3.4.0.1\tools\ntc" "%PROJECT_DIR%Pages\List\*.nht" -r -ext ".auto.cs" -src -c "NFX.Templatization.NHTCompiler, NFX"
"%SOLUTION_DIR%packages\NFX.3.4.0.1\tools\ntc" "%PROJECT_DIR%Pages\Edit\*.nht" -r -ext ".auto.cs" -src -c "NFX.Templatization.NHTCompiler, NFX"
"%SOLUTION_DIR%packages\NFX.3.4.0.1\tools\ntc" "%PROJECT_DIR%Controls\*.nht" -r -ext ".auto.cs" -src -c "NFX.Templatization.NHTCompiler, NFX"
call lessc "%PROJECT_DIR%Static\less\zhb.less" "%PROJECT_DIR%Static\css\zhb.css"