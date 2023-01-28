// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Runtime.Plug;

namespace Mosa.Kernel.x64;

/// <summary>
/// Kernel Memory Allocator
/// </summary>
public static class KernelMemory
{
	private static uint heapStart = Address.GCInitialMemory;
	private static uint heapSize = 0x02000000;
	private static uint heapUsed = 0;

	[Plug("Mosa.Runtime.GC::AllocateMemory")]
	private static unsafe IntPtr _AllocateMemory(uint size)
	{
		return AllocateMemory(size);
	}

	public static IntPtr AllocateMemory(uint size)
	{
		if (heapStart == 0 || heapSize - heapUsed < size)
		{
			// Go allocate memory
			heapSize = 1024 * 1023 * 8; // 8Mb

			// FIXME
			//heapStart = VirtualPageAllocator.Reserve(heapSize);
			heapUsed = 0;
		}

		var at = new IntPtr(heapStart + heapUsed);
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
