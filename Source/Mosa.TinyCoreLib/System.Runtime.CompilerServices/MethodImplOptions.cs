namespace System.Runtime.CompilerServices;

[Flags]
public enum MethodImplOptions
{
	Unmanaged = 4,
	NoInlining = 8,
	ForwardRef = 0x10,
	Synchronized = 0x20,
	NoOptimization = 0x40,
	PreserveSig = 0x80,
	AggressiveInlining = 0x100,
	AggressiveOptimization = 0x200,
	InternalCall = 0x1000
}
