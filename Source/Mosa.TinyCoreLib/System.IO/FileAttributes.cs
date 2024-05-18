namespace System.IO;

[Flags]
public enum FileAttributes
{
	None = 0,
	ReadOnly = 1,
	Hidden = 2,
	System = 4,
	Directory = 0x10,
	Archive = 0x20,
	Device = 0x40,
	Normal = 0x80,
	Temporary = 0x100,
	SparseFile = 0x200,
	ReparsePoint = 0x400,
	Compressed = 0x800,
	Offline = 0x1000,
	NotContentIndexed = 0x2000,
	Encrypted = 0x4000,
	IntegrityStream = 0x8000,
	NoScrubData = 0x20000
}
