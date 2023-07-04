// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.GC;

public static class InitialGCMemory
{
	private static Pointer Available;

	public static void Initialize()
	{
		//Debug.WriteLine("InitialGCMemory:Setup()");

		var region = Platform.GetInitialGCMemoryPool();

		Available = region.Address;

		BootStatus.IsGCEnabled = false;
	}

	public static Pointer AllocateMemory(uint size)
	{
		var available = Available;

		Available += size;

		return available;
	}
}
