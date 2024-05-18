namespace System.Reflection;

[Flags]
public enum PropertyAttributes
{
	None = 0,
	SpecialName = 0x200,
	RTSpecialName = 0x400,
	HasDefault = 0x1000,
	Reserved2 = 0x2000,
	Reserved3 = 0x4000,
	Reserved4 = 0x8000,
	ReservedMask = 0xF400
}
