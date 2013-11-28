/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.TinyCPUSimulator.x86.Adaptor
{
	public class SimTestSystemAdapter : SimAdapter
	{
		public SimTestSystemAdapter()
		{
			ulong freeMemPtr = 0x21700000;
			ulong freeMem = 0xF0000000;

			CPU.AddMemory(freeMemPtr, 0x0000000F, 1); // Must match Mosa.Kernel.Test.KernelMemory

			CPU.AddMemory(freeMem, 0x200000, 1);

			CPU.Write32(freeMemPtr, (uint)freeMem);
		}
	}
}