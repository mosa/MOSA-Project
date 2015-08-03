// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Threading;
using Mosa.Platform.Internal.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	///
	/// </summary>
	public static class PageFaultHandler
	{
		private static System.Threading.SpinLock spinLock = new System.Threading.SpinLock();

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

			//bool taken = false;
			//spinLock.Enter(ref taken);

			uint physicalpage = PageFrameAllocator.Allocate();

			if (physicalpage == 0x0)
				Panic.Now(1);	// Panic! Out of memory

			PageTable.MapVirtualAddressToPhysical(virtualpage, physicalpage);

			//spinLock.Exit();
		}
	}
}