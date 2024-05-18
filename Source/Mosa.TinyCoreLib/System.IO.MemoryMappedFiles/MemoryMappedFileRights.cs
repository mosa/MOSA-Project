namespace System.IO.MemoryMappedFiles;

[Flags]
public enum MemoryMappedFileRights
{
	CopyOnWrite = 1,
	Write = 2,
	Read = 4,
	ReadWrite = 6,
	Execute = 8,
	ReadExecute = 0xC,
	ReadWriteExecute = 0xE,
	Delete = 0x10000,
	ReadPermissions = 0x20000,
	ChangePermissions = 0x40000,
	TakeOwnership = 0x80000,
	FullControl = 0xF000F,
	AccessSystemSecurity = 0x1000000
}
