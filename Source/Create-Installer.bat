@echo off
call compile-debug.bat
cd Inno-Setup-Script
call create-installer.bat
cd ..