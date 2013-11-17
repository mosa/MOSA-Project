/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Internal.Plug;

namespace Mosa.Kernel.x86Test
{
	public static class KernelMemory
	{
		private static uint memoryPtr;

		[PlugMethod("Mosa.Platform.Internal.x86.AllocateMemory")]
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