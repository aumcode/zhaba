SET ROOT_FOLDER=C:\Code\aumcode\zhaba\
SET PROJECT_DIR=%ROOT_FOLDER%Source\Zhaba.Web\
SET OUTPUT_DIR=%ROOT_FOLDER%Output\Debug\Static\js\

CALL %ROOT_FOLDER%packages\NFX.3.5.0.1\tools\ntc %PROJECT_DIR%Static\ntc\zhb.ntc.js -ext ".js" -replace ".ntc.js" -dest %PROJECT_DIR%Static\js -src -c "NFX.Templatization.TextJSTemplateCompiler, NFX" -o dom-gen="cmp{pretty=1}"

XCOPY /E /Y %PROJECT_DIR%Static\js\zhb.js %OUTPUT_DIR%