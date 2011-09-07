using System;

namespace Mosa.Runtime.Metadata
{

	[Flags]
	public enum AssemblyAttributes : uint
	{
		PublicKey = 0x0001,
		SideBySideCompatible = 0x0000,
		Retargetable = 0x0100,
		DisableJITCompileOptimizer = 0x4000,
		EnableJITCompileTracking = 0x8000,
	}
}
