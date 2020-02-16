cd Docs

call make html

rmdir /S /Q ..\..\docs

cd build
rename html docs
move docs ../../../docs

cd ../..

