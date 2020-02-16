cd Docs

call make html

rmdir /S ../../docs

cd build
rename html docs
move docs ../../../docs

cd ../..

