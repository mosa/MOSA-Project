/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Kernel.Memory.X86
{
	/// <summary>
	/// A physical page manager.
	/// </summary>
	/// <remarks>
	/// This interface defines the abstract operations to allocate and free at the physical page level.
	/// </remarks>
	public class PhysicalPageManager
	{
		/// <summary>
		/// 
		/// </summary>
		protected const ushort X86PageSize = 4096;
		/// <summary>
		/// 
		/// </summary>
		protected uint totalPages;
		/// <summary>
		/// 
		/// </summary>
		protected uint totalUsedPages;
		/// <summary>
		/// 
		/// </summary>
		protected uint firstIndexPage;
		/// <summary>
		/// 
		/// </summary>
		protected uint lastIndexPage;
		/// <summary>
		/// 
		/// </summary>
		protected uint indexSize;
		/// <summary>
		/// 
		/// </summary>
		protected uint indexAddress;

		/// <summary>
		/// Initializes a new instance of the <see cref="PhysicalPageManager"/> class.
		/// </summary>
		public PhysicalPageManager(ulong startPage, ulong totalPages)
		{
			this.totalPages = (uint)totalPages;
			this.totalUsedPages = 0;
			this.firstIndexPage = (uint)startPage;
			this.lastIndexPage = (uint)(firstIndexPage + (totalPages * sizeof(uint) / X86PageSize) + 1);
			this.indexSize = lastIndexPage - firstIndexPage + 1;
			this.indexAddress = firstIndexPage * X86PageSize;

			// Populate free table
			for (uint i = 0; i < totalPages - indexSize; i++)
				unsafe {
					(*(uint*)(indexAddress + (i * sizeof(uint)))) = (lastIndexPage + 1 + i);
				}
		}

		/// <summary>
		/// Allocate a physical page from the free list
		/// </summary>
		/// <returns>The page</returns>
		public ulong Allocate()
		{
			if (totalPages == totalUsedPages)
				return 0; // out of memory

			unsafe {
				return (ulong)(*((uint*)(indexAddress + (totalUsedPages++) * sizeof(uint))));
			}
		}

		/// <summary>
		/// Releases a page to the free list
		/// </summary>
		/// <param name="page">The page.</param>
		public unsafe void Free(ulong page)
		{
			(*((uint*)(indexAddress + (++totalUsedPages) * sizeof(uint)))) = (uint)page;
		}

		/// <summary>
		/// Retrieves the size of a single memory page.
		/// </summary>
		public ulong PageSize { get { return X86PageSize; } }

		/// <summary>
		/// Retrieves the amount of total physical memory pages available in the system.
		/// </summary>
		public ulong TotalPages { get { return totalPages; } }

		/// <summary>
		/// Retrieves the amount of number of physical pages in use.
		/// </summary>
		public ulong TotalPagesInUse { get { return totalUsedPages; } }

	}
}
