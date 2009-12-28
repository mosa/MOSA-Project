/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Kernel.Memory.X86;

namespace Mosa.HelloWorld
{
	/// <summary>
	/// 
	/// </summary>
	public static class Kernel
	{
		private static uint _processtable = 0x1000;
		private static uint _threadtable = 0x2000;

		public static void Setup()
		{
			PageTable.MapVirtualAddressToPhysical(_processtable, PageFrameAllocator.Allocate());
			PageTable.MapVirtualAddressToPhysical(_threadtable, PageFrameAllocator.Allocate());

			Memory.Clear(_processtable, PageFrameAllocator.PageSize);
			Memory.Clear(_threadtable, PageFrameAllocator.PageSize);

			Memory.Set32(_processtable, 0x01);
			Memory.Set32(_threadtable, 0x02);
		}

	}
}
