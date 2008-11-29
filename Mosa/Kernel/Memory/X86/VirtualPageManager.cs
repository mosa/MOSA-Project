/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
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
		private IPhysicalPageManager physicalPageManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="VirtualPageManager"/> class.
		/// </summary>
		/// <param name="physicalPageManager">The physical page manager.</param>
		public VirtualPageManager(IPhysicalPageManager physicalPageManager)
		{
			this.physicalPageManager = physicalPageManager;
		}

		/// <summary>
		/// Reserves or commits a range of pages.
		/// </summary>
		/// <param name="pages">The pages to allocate</param>
		/// <param name="protectionFlags">One or more flag that controls the protection of the retrieved pages.</param>
		/// <returns>An IntPtr to the allocated memory.</returns>
		public System.IntPtr Allocate(ulong pages, PageProtectionFlags protectionFlags)
		{
			// Get pages from physical page manager

			// Map pags to virtual memory spaces (that fits)

			return System.IntPtr.Zero;	// TODO
		}

		/// <summary>
		/// Releases or decommits a range of pages.
		/// </summary>
		/// <param name="address">The starting address, where pages are freed.</param>
		/// <param name="pages">The number of pages.</param>
		public void Free(System.IntPtr address, ulong pages)
		{
			// TODO

			// Remove pages from virtual memory spaces

			// Release to physical page manager			
		}

		/// <summary>
		/// Changes the protection bits of the pages associated with the given range of memory.
		/// </summary>
		/// <param name="address">The starting address.</param>
		/// <param name="pages">The number of pages.</param>
		/// <param name="protectionFlags">The new set of protection flags.</param>
		/// <returns>The old protection flags of the first page in the range of memory. </returns>
		public PageProtectionFlags Protect(System.IntPtr address, ulong pages, PageProtectionFlags protectionFlags)
		{
			return PageProtectionFlags.NoAccess;	// TODO
		}

		/// <summary>
		/// Retrieves the size of a single memory page.
		/// </summary>
		public ulong PageSize { get { return physicalPageManager.PageSize; } }

		/// <summary>
		/// Retrieves the amount of virtual memory available in the system.
		/// </summary>
		public ulong TotalMemory { get { return physicalPageManager.TotalPages * physicalPageManager.PageSize; } }

		/// <summary>
		/// Retrieves the amount of virtual memory currently in use.
		/// </summary>
		public ulong TotalMemoryInUse { get { return physicalPageManager.TotalPagesInUse * physicalPageManager.PageSize; } }

		//private uint VirtualToPhysicalAddress(uint address)

	}
}
