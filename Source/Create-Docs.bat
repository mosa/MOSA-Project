CD Docs
CALL make html
RMDIR /S /Q ..\..\docs\_static
DEL /F /S /Q ..\..\docs\*.*
DEL /F /S /Q ..\..\docs\images\*.*
XCOPY /S build\html ..\..\docs\
COPY /Y *.nojekyll ..\..\docs
CD ..
