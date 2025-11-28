namespace System.Security.AccessControl;

[Flags]
public enum MutexRights
{
	Modify = 1,
	Delete = 0x10000,
	ReadPermissions = 0x20000,
	ChangePermissions = 0x40000,
	TakeOwnership = 0x80000,
	Synchronize = 0x100000,
	FullControl = 0x1F0001
}
