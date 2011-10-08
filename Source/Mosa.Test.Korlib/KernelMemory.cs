/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


namespace Mosa.Kernel
{
	public class KernelMemory
	{
		const uint memoryptr = 0x21700000;	// Location for pointer to allocated memory!

		static private uint memory = 0;

		static unsafe public uint AllocateMemory(uint size)
		{
			if (memory == 0)
			{
				memory = ((uint*)memoryptr)[0];
			}

			uint alloc = memory;
			memory += size;
			return alloc;
		}

	}
}
