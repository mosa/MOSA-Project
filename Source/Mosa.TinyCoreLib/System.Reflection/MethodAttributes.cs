namespace System.Reflection;

[Flags]
public enum MethodAttributes
{
	PrivateScope = 0,
	ReuseSlot = 0,
	Private = 1,
	FamANDAssem = 2,
	Assembly = 3,
	Family = 4,
	FamORAssem = 5,
	Public = 6,
	MemberAccessMask = 7,
	UnmanagedExport = 8,
	Static = 0x10,
	Final = 0x20,
	Virtual = 0x40,
	HideBySig = 0x80,
	NewSlot = 0x100,
	VtableLayoutMask = 0x100,
	CheckAccessOnOverride = 0x200,
	Abstract = 0x400,
	SpecialName = 0x800,
	RTSpecialName = 0x1000,
	PinvokeImpl = 0x2000,
	HasSecurity = 0x4000,
	RequireSecObject = 0x8000,
	ReservedMask = 0xD000
}
