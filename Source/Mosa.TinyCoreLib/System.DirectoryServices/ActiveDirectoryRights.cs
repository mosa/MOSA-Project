namespace System.DirectoryServices;

[Flags]
public enum ActiveDirectoryRights
{
	CreateChild = 1,
	DeleteChild = 2,
	ListChildren = 4,
	Self = 8,
	ReadProperty = 0x10,
	WriteProperty = 0x20,
	DeleteTree = 0x40,
	ListObject = 0x80,
	ExtendedRight = 0x100,
	Delete = 0x10000,
	ReadControl = 0x20000,
	GenericExecute = 0x20004,
	GenericWrite = 0x20028,
	GenericRead = 0x20094,
	WriteDacl = 0x40000,
	WriteOwner = 0x80000,
	GenericAll = 0xF01FF,
	Synchronize = 0x100000,
	AccessSystemSecurity = 0x1000000
}
