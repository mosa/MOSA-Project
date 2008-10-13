@echo off
SETLOCAL

SET PREBUILD=..\tools\prebuild\Prebuild.exe
SET TARGET=%1
SET NOFOLDERS=0

IF /I "%TARGET%"=="VS2008Express" (
	SET TARGET="VS2008"
	SET NOFOLDERS=1
) ELSE IF /I "%TARGET%"=="VS2005Express" (
	SET TARGET="VS2005"
	SET NOFOLDERS=1
) ELSE IF /I "%TARGET%"=="NAnt" (
	SET TARGET="nant"
	SET NOFOLDERS=1
) ELSE IF /I "%TARGET%"=="VS2005" (
	SET TARGET="VS2005"
	SET NOFOLDERS=0
) ELSE (
	SET TARGET="VS2008"
	SET NOFOLDERS=0
)

IF %NOFOLDERS%==1 (
	FIND /V "EmbeddedSolution" <Mosa.prebuild >Mosa.nofolders.prebuild
	%PREBUILD% /file Mosa.nofolders.prebuild /target %TARGET%
	DEL Mosa.nofolders.prebuild
) ELSE (
	%PREBUILD% /file Mosa.prebuild /target %TARGET%
)