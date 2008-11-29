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
	/// 
	/// </summary>
	public class PhysicalPageManager : IPhysicalPageManager
	{
		/// <summary>
		/// Allocates the page.
		/// </summary>
		/// <returns>An IntPtr to the allocated memory.</returns>
		public ulong Allocate()
		{
			// Get physical page
			uint page = PageAllocator.Allocate();

			// Map page into virtual space (0x02 = Read/Write)
			PageTable.MapVirtualAddressToPhysical(page, page, 0x02);

			return page;
		}

		/// <summary>
		/// Frees the page.
		/// </summary>
		/// <param name="address">The starting address of the page to freed.</param>
		public void Free(ulong address)
		{
			// Remove virtual page from page table
			PageTable.ReleaseVirtualAddress((uint)address);

			// Release physical page
			PageAllocator.Free((uint)address);
		}

		/// <summary>
		/// Retrieves the size of a single memory page.
		/// </summary>
		public ulong PageSize
		{
			get
			{
				return PageAllocator.PageSize;
			}
		}

		/// <summary>
		/// Retrieves the amount of total physical memory pages available in the system.
		/// </summary>
		public ulong TotalPages
		{
			get
			{
				return PageAllocator.TotalPages;
			}
		}

		/// <summary>
		/// Retrieves the amount of number of physical pages in use.
		/// </summary>
		public ulong TotalPagesInUse
		{
			get
			{
				return PageAllocator.TotalPagesInUse;
			}
		}
	}
}
