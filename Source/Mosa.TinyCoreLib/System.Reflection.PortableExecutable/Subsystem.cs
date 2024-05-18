namespace System.Reflection.PortableExecutable;

public enum Subsystem : ushort
{
	Unknown = 0,
	Native = 1,
	WindowsGui = 2,
	WindowsCui = 3,
	OS2Cui = 5,
	PosixCui = 7,
	NativeWindows = 8,
	WindowsCEGui = 9,
	EfiApplication = 10,
	EfiBootServiceDriver = 11,
	EfiRuntimeDriver = 12,
	EfiRom = 13,
	Xbox = 14,
	WindowsBootApplication = 16
}
