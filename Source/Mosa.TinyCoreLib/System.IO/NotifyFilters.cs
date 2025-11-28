namespace System.IO;

[Flags]
public enum NotifyFilters
{
	FileName = 1,
	DirectoryName = 2,
	Attributes = 4,
	Size = 8,
	LastWrite = 0x10,
	LastAccess = 0x20,
	CreationTime = 0x40,
	Security = 0x100
}
