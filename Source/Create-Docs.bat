cd Docs
call make html
rmdir /S /Q ..\..\docs\_static
del /F /S /Q ..\..\docs\*.*
del /F /S /Q ..\..\docs\images\*.*
xcopy /S build\html ..\..\docs\
copy /Y *.nojekyll ..\..\docs
echo www.mosa-project.org > ..\..\docs\CNAME
cd ..
