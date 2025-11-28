namespace System.IO.Pipes;

[Flags]
public enum PipeAccessRights
{
	ReadData = 1,
	WriteData = 2,
	CreateNewInstance = 4,
	ReadExtendedAttributes = 8,
	WriteExtendedAttributes = 0x10,
	ReadAttributes = 0x80,
	WriteAttributes = 0x100,
	Write = 0x112,
	Delete = 0x10000,
	ReadPermissions = 0x20000,
	Read = 0x20089,
	ReadWrite = 0x2019B,
	ChangePermissions = 0x40000,
	TakeOwnership = 0x80000,
	Synchronize = 0x100000,
	FullControl = 0x1F019F,
	AccessSystemSecurity = 0x1000000
}
