namespace System.Reflection.PortableExecutable;

[Flags]
public enum Characteristics : ushort
{
	RelocsStripped = 1,
	ExecutableImage = 2,
	LineNumsStripped = 4,
	LocalSymsStripped = 8,
	AggressiveWSTrim = 0x10,
	LargeAddressAware = 0x20,
	BytesReversedLo = 0x80,
	Bit32Machine = 0x100,
	DebugStripped = 0x200,
	RemovableRunFromSwap = 0x400,
	NetRunFromSwap = 0x800,
	System = 0x1000,
	Dll = 0x2000,
	UpSystemOnly = 0x4000,
	BytesReversedHi = 0x8000
}
