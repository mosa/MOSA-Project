/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Kernel.Memory.X86
{
	/// <summary>
	/// Interface of a virtual page manager.
	/// </summary>
	/// <remarks>
	/// This interface defines the abstract operations to allocate, free and manage ranges
	/// of virtual memory at the page level.
	/// </remarks>
	public sealed class VirtualPageManager
	{
		private const ulong StartVirtualSpace = 1024 * 1024 * 32; // 32Mb

		private static IPhysicalPageManager _physicalPageManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="VirtualPageManager"/> class.
		/// </summary>
		/// <param name="physicalPageManager">The physical page manager.</param>
		public VirtualPageManager(IPhysicalPageManager physicalPageManager)
		{
			_physicalPageManager = physicalPageManager;

			DumbVirtualPageAllocator.Setup(StartVirtualSpace, 0xFFFFFFFFFFFFFFFF, physicalPageManager.PageSize);
		}

		/// <summary>
		/// Reserves or commits a range of pages.
		/// </summary>
		/// <param name="pages">The pages to allocate</param>
		/// <returns>An IntPtr to the allocated memory.</returns>
		public ulong Allocate(ulong pages)
		{
			// First reserve virtual memory space
			ulong reservedStart = DumbVirtualPageAllocator.Allocate(pages);

			if (reservedStart == 0)
				return 0; // failed to reserve any pages

			// Get pages from physical page manager
			// [TODO]

			// Map pages to virtual memory spaces 
			// [TODO]

			return 0x00;	// TODO
		}

		/// <summary>
		/// Releases or decommits a range of pages.
		/// </summary>
		/// <param name="address">The starting address, where pages are freed.</param>
		/// <param name="pages">The number of pages.</param>
		public void Free(ulong address, ulong pages)
		{
			DumbVirtualPageAllocator.Free(address, pages);

			// Unmap pages to virtual memory spaces 
			// [TODO]

			// Release to physical page manager			
			// [TODO]
		}

		/// <summary>
		/// Changes the protection bits of the pages associated with the given range of memory.
		/// </summary>
		/// <param name="address">The starting address.</param>
		/// <param name="pages">The number of pages.</param>
		/// <param name="protectionFlags">The new set of protection flags.</param>
		/// <returns>The old protection flags of the first page in the range of memory. </returns>
		public PageProtectionFlags Protect(ulong address, ulong pages, PageProtectionFlags protectionFlags)
		{
			return PageProtectionFlags.NoAccess;	// TODO
		}

		/// <summary>
		/// Retrieves the size of a single memory page.
		/// </summary>
		public ulong PageSize { get { return _physicalPageManager.PageSize; } }

		/// <summary>
		/// Retrieves the amount of virtual memory available in the system.
		/// </summary>
		public ulong TotalMemory { get { return _physicalPageManager.TotalPages * _physicalPageManager.PageSize; } }

		/// <summary>
		/// Retrieves the amount of virtual memory currently in use.
		/// </summary>
		public ulong TotalMemoryInUse { get { return _physicalPageManager.TotalPagesInUse * _physicalPageManager.PageSize; } }

		//private uint VirtualToPhysicalAddress(uint address)

	}
}
