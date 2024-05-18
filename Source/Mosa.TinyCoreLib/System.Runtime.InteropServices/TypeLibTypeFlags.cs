namespace System.Runtime.InteropServices;

[Flags]
public enum TypeLibTypeFlags
{
	FAppObject = 1,
	FCanCreate = 2,
	FLicensed = 4,
	FPreDeclId = 8,
	FHidden = 0x10,
	FControl = 0x20,
	FDual = 0x40,
	FNonExtensible = 0x80,
	FOleAutomation = 0x100,
	FRestricted = 0x200,
	FAggregatable = 0x400,
	FReplaceable = 0x800,
	FDispatchable = 0x1000,
	FReverseBind = 0x2000
}
