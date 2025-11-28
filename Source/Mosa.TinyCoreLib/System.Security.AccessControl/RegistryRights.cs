namespace System.Security.AccessControl;

[Flags]
public enum RegistryRights
{
	QueryValues = 1,
	SetValue = 2,
	CreateSubKey = 4,
	EnumerateSubKeys = 8,
	Notify = 0x10,
	CreateLink = 0x20,
	Delete = 0x10000,
	ReadPermissions = 0x20000,
	WriteKey = 0x20006,
	ExecuteKey = 0x20019,
	ReadKey = 0x20019,
	ChangePermissions = 0x40000,
	TakeOwnership = 0x80000,
	FullControl = 0xF003F
}
