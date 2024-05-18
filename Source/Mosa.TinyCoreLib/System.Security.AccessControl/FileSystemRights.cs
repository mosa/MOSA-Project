namespace System.Security.AccessControl;

[Flags]
public enum FileSystemRights
{
	ListDirectory = 1,
	ReadData = 1,
	CreateFiles = 2,
	WriteData = 2,
	AppendData = 4,
	CreateDirectories = 4,
	ReadExtendedAttributes = 8,
	WriteExtendedAttributes = 0x10,
	ExecuteFile = 0x20,
	Traverse = 0x20,
	DeleteSubdirectoriesAndFiles = 0x40,
	ReadAttributes = 0x80,
	WriteAttributes = 0x100,
	Write = 0x116,
	Delete = 0x10000,
	ReadPermissions = 0x20000,
	Read = 0x20089,
	ReadAndExecute = 0x200A9,
	Modify = 0x301BF,
	ChangePermissions = 0x40000,
	TakeOwnership = 0x80000,
	Synchronize = 0x100000,
	FullControl = 0x1F01FF
}
