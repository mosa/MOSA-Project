/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Internal.Plug;

namespace Mosa.Kernel
{
	public class KernelMemory
	{
		const uint memoryPtr = 0x21700000;	// Location for pointer to allocated memory!

		[PlugMethod("Mosa.Internal.Runtime.AllocateMemory")]
		static unsafe public uint AllocateMemory(uint size)
		{
			uint alloc = ((uint*)memoryPtr)[0];
			((uint*)memoryPtr)[0] = alloc + size;
 
			return alloc;
		}

	}
}
