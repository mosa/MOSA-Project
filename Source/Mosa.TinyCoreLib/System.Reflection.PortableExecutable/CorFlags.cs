namespace System.Reflection.PortableExecutable;

[Flags]
public enum CorFlags
{
	ILOnly = 1,
	Requires32Bit = 2,
	ILLibrary = 4,
	StrongNameSigned = 8,
	NativeEntryPoint = 0x10,
	TrackDebugData = 0x10000,
	Prefers32Bit = 0x20000
}
