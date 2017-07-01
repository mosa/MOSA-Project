// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Kernel Memory Allocator
	/// </summary>
	public static class KernelMemory
	{
		static private uint heapStart = Address.GCInitialMemory;
		static private uint heapSize = 0x02000000;
		static private uint heapUsed = 0;

		[Method("Mosa.Runtime.GC.AllocateMemory")]
		static unsafe private void* _AllocateMemory(uint size)
		{
			return (void*)AllocateMemory(size);
		}

		static public uint AllocateMemory(uint size)
		{
			if (heapStart == 0 || (heapSize - heapUsed) < size)
			{
				// Go allocate memory
				heapSize = 1024 * 1023 * 8; // 8Mb
				heapStart = VirtualPageAllocator.Reserve(heapSize);
				heapUsed = 0;
			}

			uint at = heapStart + heapUsed;
			heapUsed = heapUsed + size;
			return at;
		}

		static public void SetInitialMemory(uint address, uint size)
		{
			heapStart = address;
			heapSize = size;
			heapUsed = 0;
		}
	}
}
