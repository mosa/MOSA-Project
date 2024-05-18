namespace System.DirectoryServices.ActiveDirectory;

[Flags]
public enum TopLevelNameCollisionOptions
{
	None = 0,
	NewlyCreated = 1,
	DisabledByAdmin = 2,
	DisabledByConflict = 4
}
