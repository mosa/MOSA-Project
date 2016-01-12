// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;
using Mosa.Runtime;

namespace Mosa.Kernel.x86Test
{
	public static class KernelMemory
	{
		private static uint memoryPtr = 0x00900000;

		static KernelMemory()
		{
			GC.Setup();
		}

		[Method("Mosa.Runtime.GC.AllocateMemory")]
		static unsafe private void* _AllocateMemory(uint size)
		{
			return (void*)AllocateMemory(size);
		}

		static public uint AllocateMemory(uint size)
		{
			uint alloc = memoryPtr;
			memoryPtr = alloc + size;
			return alloc;
		}

		static private uint SetMemory(uint ptr)
		{
			memoryPtr = ptr;
			return memoryPtr;
		}
	}
}
