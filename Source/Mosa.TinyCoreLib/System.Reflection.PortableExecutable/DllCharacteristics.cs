namespace System.Reflection.PortableExecutable;

[Flags]
public enum DllCharacteristics : ushort
{
	ProcessInit = 1,
	ProcessTerm = 2,
	ThreadInit = 4,
	ThreadTerm = 8,
	HighEntropyVirtualAddressSpace = 0x20,
	DynamicBase = 0x40,
	ForceIntegrity = 0x80,
	NxCompatible = 0x100,
	NoIsolation = 0x200,
	NoSeh = 0x400,
	NoBind = 0x800,
	AppContainer = 0x1000,
	WdmDriver = 0x2000,
	ControlFlowGuard = 0x4000,
	TerminalServerAware = 0x8000
}
