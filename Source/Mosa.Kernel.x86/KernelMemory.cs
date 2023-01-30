// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Plug;

namespace Mosa.Kernel.x86;

/// <summary>
/// Kernel Memory Allocator
/// </summary>
public static class KernelMemory
{
	private static uint heapStart;
	private static uint heapSize;
	private static uint heapUsed;

	[Plug("Mosa.Runtime.GC::AllocateMemory")]
	private static unsafe Pointer _AllocateMemory(uint size)
	{
		return AllocateVirtualMemory(size);
	}

	public static Pointer AllocateVirtualMemory(uint size)
	{
		if (heapStart == 0 || (heapSize - heapUsed) < size)
		{
			// Go allocate memory
			heapSize = 1024 * 1023 * 8; // 8Mb
			heapStart = VirtualPageAllocator.Reserve(heapSize);
			heapUsed = 0;
		}

		var at = new Pointer(heapStart + heapUsed);
		heapUsed += size;
		return at;
	}

	public static void SetInitialMemory(uint address, uint size)
	{
		heapStart = address;
		heapSize = size;
		heapUsed = 0;
	}
}
