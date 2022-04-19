@echo off
call compile.bat
cd Inno-Setup-Script
call create-installer.bat
cd ..