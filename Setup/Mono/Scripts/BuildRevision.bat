echo "This will take a few minutes..."

CALL scripts\GetSource.bat %1
CALL scripts\ExtractSource.bat %1
CALL scripts\PatchSource.bat %1
CALL scripts\UpdateProjectFiles.bat %1

CALL "scripts\PatchSource-%1.bat" %1

