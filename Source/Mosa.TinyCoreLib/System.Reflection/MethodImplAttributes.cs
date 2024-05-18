namespace System.Reflection;

public enum MethodImplAttributes
{
	IL = 0,
	Managed = 0,
	Native = 1,
	OPTIL = 2,
	CodeTypeMask = 3,
	Runtime = 3,
	ManagedMask = 4,
	Unmanaged = 4,
	NoInlining = 8,
	ForwardRef = 16,
	Synchronized = 32,
	NoOptimization = 64,
	PreserveSig = 128,
	AggressiveInlining = 256,
	AggressiveOptimization = 512,
	InternalCall = 4096,
	MaxMethodImplVal = 65535
}
