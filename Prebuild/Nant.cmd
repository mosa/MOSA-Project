@echo off
SETLOCAL

SET PREBUILD=..\tools\prebuild\Prebuild.exe
SET TARGET=nant
SET NOFOLDERS=0

FIND /V "EmbeddedSolution" <Mosa.prebuild >Mosa.nofolders.prebuild
%PREBUILD% /file Mosa.nofolders.prebuild /target %TARGET%
DEL Mosa.nofolders.prebuild

pause
