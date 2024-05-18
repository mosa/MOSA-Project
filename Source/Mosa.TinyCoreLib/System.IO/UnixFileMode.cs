namespace System.IO;

[Flags]
public enum UnixFileMode
{
	None = 0,
	OtherExecute = 1,
	OtherWrite = 2,
	OtherRead = 4,
	GroupExecute = 8,
	GroupWrite = 0x10,
	GroupRead = 0x20,
	UserExecute = 0x40,
	UserWrite = 0x80,
	UserRead = 0x100,
	StickyBit = 0x200,
	SetGroup = 0x400,
	SetUser = 0x800
}
