// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Internal.Plug;
using Mosa.Platform.Internal.x86;

namespace Mosa.Kernel.x86Test
{
	public static class KernelMemory
	{
		private static uint memoryPtr = 0x00900000;

		static KernelMemory()
		{
			GC.Setup();
		}

		[Method("Mosa.Platform.Internal.x86.GC.AllocateMemory")]
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
