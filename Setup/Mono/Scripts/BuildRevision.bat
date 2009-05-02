echo "This will take a few minutes..."
CALL scripts\GetSource.bat %1
CALL scripts\ExtractSource.bat %1
CALL scripts\TransformSource.bat %1
CALL scripts\PatchSource.bat %1
CALL scripts\UpdateProjectFiles %1
