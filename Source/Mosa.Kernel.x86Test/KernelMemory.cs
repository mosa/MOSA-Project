// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Internal.Plug;
using Mosa.Platform.Internal.x86;
using Mosa.Internal;

namespace Mosa.Kernel.x86Test
{
	public static class KernelMemory
	{
		private static uint memoryPtr = 0x00900000;

		static KernelMemory()
		{
			GC.Setup();
		}

		[Method("Mosa.Internal.GC.AllocateMemory")]
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
