namespace System.Reflection;

[Flags]
public enum EventAttributes
{
	None = 0,
	SpecialName = 0x200,
	ReservedMask = 0x400,
	RTSpecialName = 0x400
}
