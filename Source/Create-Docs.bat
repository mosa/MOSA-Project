CD Docs
CALL make html
RMDIR /S /Q ..\..\docs
CD build
RENAME html docs
MOVE docs ..\..\..\docs
CD ..
COPY /Y *.nojekyll ..\..\docs
CD ..
