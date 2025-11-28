namespace System.DirectoryServices;

[Flags]
public enum DirectoryServicesPermissionAccess
{
	None = 0,
	Browse = 2,
	Write = 6
}
