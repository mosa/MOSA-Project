namespace System.Reflection;

[Flags]
public enum AssemblyFlags
{
	PublicKey = 1,
	Retargetable = 0x100,
	WindowsRuntime = 0x200,
	ContentTypeMask = 0xE00,
	DisableJitCompileOptimizer = 0x4000,
	EnableJitCompileTracking = 0x8000
}
