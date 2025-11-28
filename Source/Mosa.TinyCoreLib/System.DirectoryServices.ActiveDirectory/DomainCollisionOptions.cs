namespace System.DirectoryServices.ActiveDirectory;

[Flags]
public enum DomainCollisionOptions
{
	None = 0,
	SidDisabledByAdmin = 1,
	SidDisabledByConflict = 2,
	NetBiosNameDisabledByAdmin = 4,
	NetBiosNameDisabledByConflict = 8
}
