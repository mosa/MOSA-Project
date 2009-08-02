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
	/// <summary>
	/// A physical page manager.
	/// </summary>
	public static class PhysicalPageManager
	{
		private const ushort X86PageSize = 4096;

		private static uint totalPages;
		private static uint totalUsedPages;
		private static uint firstIndexPage;
		private static uint lastIndexPage;
		private static uint indexSize;
		private static uint indexAddress;

		/// <summary>
		/// Setups the physical page manager
		/// </summary>
		/// <param name="totalMemory">The total memory.</param>
		/// <param name="startFreeMemory">The start of free memory.</param>
		public static void Setup(uint totalMemory, uint startFreeMemory)
		{
			totalPages = (totalMemory - startFreeMemory) / X86PageSize;
			totalUsedPages = 0;

			firstIndexPage = (uint)((startFreeMemory / X86PageSize) + 1);
			lastIndexPage = (uint)(firstIndexPage + (totalPages * sizeof(uint) / X86PageSize) + 1);
			indexSize = lastIndexPage - firstIndexPage + 1;
			indexAddress = firstIndexPage * X86PageSize;

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
		public static uint Allocate()
		{
			if (totalPages == totalUsedPages)
				return 0; // out of memory

			unsafe {
				return (uint)(*((uint*)(indexAddress + (totalUsedPages++) * sizeof(uint))));
			}
		}

		/// <summary>
		/// Releases a page to the free list
		/// </summary>
		/// <param name="page">The page.</param>
		public static void Free(uint page)
		{
			unsafe {
				(*((uint*)(indexAddress + (++totalUsedPages) * sizeof(uint)))) = (uint)page;
			}
		}

		/// <summary>
		/// Retrieves the size of a single memory page.
		/// </summary>
		public static uint PageSize { get { return X86PageSize; } }

		/// <summary>
		/// Retrieves the amount of total physical memory pages available in the system.
		/// </summary>
		public static uint TotalPages { get { return totalPages; } }

		/// <summary>
		/// Retrieves the amount of number of physical pages in use.
		/// </summary>
		public static uint TotalPagesInUse { get { return totalUsedPages; } }

	}
}
