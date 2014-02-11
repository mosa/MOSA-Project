﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Internal.Plug;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Kernel Memory Allocator - This is a pure HACK!
	/// </summary>
	public static class KernelMemory
	{
		static private uint heap = 0x300000;
		static private uint allocated = 0;
		static private uint used = 0;

		// FIXME: Temporary fix until allocate memory function works correctly

		[PlugMethod("Mosa.Platform.Internal.x86.Runtime.AllocateMemory")]
		static public uint AllocateMemory(uint size)
		{
			uint at = heap;
			heap = heap + size;
			return at;
		}

		//[PlugMethod("Mosa.Platform.Internal.x86.Runtime.AllocateMemory")]
		//static public uint AllocateMemory(uint size)
		//{
		//	if ((heap == 0) || (size > (allocated - used)))
		//	{
		//		// Go allocate memory

		//		allocated = 1024 * 1024 * 64; // 64Mb
		//		heap = x86.ProcessManager.AllocateMemory(0, allocated);
		//		used = 0;
		//	}

		//	uint at = heap + used;
		//	used = used + size;
		//	return at;
		//}
	}
}