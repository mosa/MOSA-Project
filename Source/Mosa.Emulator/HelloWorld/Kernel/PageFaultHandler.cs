/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Platform.Internal.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	///
	/// </summary>
	public static class PageFaultHandler
	{
		private static uint counter = 0;

		/// <summary>
		/// Handle Page Faults
		/// </summary>
		/// <param name="errorCode">The error code.</param>
		public static void Fault(uint errorCode)
		{
			uint virtualpage = Native.GetCR2();

			if (virtualpage == 0x0)
			{
				Panic.Now(2);	// Can't map null! what happened?
			}

			// TODO: acquire lock

			counter++;

			uint physicalpage = PageFrameAllocator.Allocate();

			if (physicalpage == 0x0)
				Panic.Now(1);	// Panic! Out of memory

			PageTable.MapVirtualAddressToPhysical(virtualpage, physicalpage);

			// TODO: release lock
		}
	}
}