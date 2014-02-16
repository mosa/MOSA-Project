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

			CPU.Write32(freeMemPtr, (uint)freeMem);
		}
	}
}