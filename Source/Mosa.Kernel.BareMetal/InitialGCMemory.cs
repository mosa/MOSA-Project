// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

public static class InitialGCMemory
{
	private static Pointer Available;

	public static void Initialize()
	{
		Debug.WriteLine("InitialGCMemory:Setup()\n");

		var region = Platform.GetInitialGCMemoryPool();

		Internal.MemoryClear(region.Address, (uint)region.Size);

		Available = region.Address;

		BootStatus.IsGCEnabled = false;

		Debug.WriteLine("InitialGCMemory:Setup() [Exit]\n");
	}

	public static Pointer AllocateMemory(uint size)
	{
		//Debug.WriteLine("+ Initial Allocation Object: size = ", size, " @ ", new Hex(Available));

		var available = Available;

		Available += size;

		return available;
	}
}
