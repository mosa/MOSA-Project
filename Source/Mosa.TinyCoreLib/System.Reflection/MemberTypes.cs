namespace System.Reflection;

[Flags]
public enum MemberTypes
{
	Constructor = 1,
	Event = 2,
	Field = 4,
	Method = 8,
	Property = 0x10,
	TypeInfo = 0x20,
	Custom = 0x40,
	NestedType = 0x80,
	All = 0xBF
}
