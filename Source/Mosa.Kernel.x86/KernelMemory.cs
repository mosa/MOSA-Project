// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Kernel Memory Allocator
	/// </summary>
	public static class KernelMemory
	{
		static private uint heap = 0;
		static private uint allocated = 0;
		static private uint used = 0;

		[Method("Mosa.Runtime.GC.AllocateMemory")]
		static unsafe private void* _AllocateMemory(uint size)
		{
			return (void*)AllocateMemory(size);
		}

		static public uint AllocateMemory(uint size)
		{
			if ((heap == 0) || (size > (allocated - used)))
			{
				// Go allocate memory

				allocated = 1024 * 1024 * 64; // 64Mb
				heap = VirtualPageAllocator.Reserve(size);
				used = 0;
			}

			uint at = heap + used;
			used = used + size;
			return at;
		}
	}
}
