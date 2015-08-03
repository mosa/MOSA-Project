// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Internal.Plug;

namespace Mosa.Kernel.x86Test
{
	public static class KernelMemory
	{
		private static uint memoryPtr = 0x1000000;

		[Method("Mosa.Platform.Internal.x86.Runtime.AllocateMemory")]
		static public uint AllocateMemory(uint size)
		{
			uint alloc = memoryPtr;
			memoryPtr = alloc + size;
			return alloc;
		}
	}
}