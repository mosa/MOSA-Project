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
	/// Super dumb virtual page allocated, eventually this allocator will crash
	/// </summary>
	public static class DumbVirtualPageAllocator
	{
		private static ulong lastReserved;
		private static ulong pagesAvailable;
		private static uint _pageSize;

		/// <summary>
		/// Setups the specified start.
		/// </summary>
		/// <param name="start">The start.</param>
		/// <param name="pages">The pages.</param>
		/// <param name="pageSize">Size of the page.</param>
		public static void Setup(ulong start, ulong pages, uint pageSize)
		{
			lastReserved = start;
			pagesAvailable = pages;
			_pageSize = pageSize;
		}

		/// <summary>
		/// Allocates the specified pages.
		/// </summary>
		/// <param name="pages">The pages.</param>
		/// <returns></returns>
		public static ulong Allocate(ulong pages)
		{
			if (pages > pagesAvailable)
				return 0;

			ulong allocate = lastReserved;

			lastReserved = lastReserved + (pages * _pageSize);
			pagesAvailable = pagesAvailable - pages;

			return allocate;
		}

		/// <summary>
		/// Frees the specified pages.
		/// </summary>
		/// <param name="start">The start.</param>
		/// <param name="pages">The pages.</param>
		public static void Free(ulong start, ulong pages)
		{
			return;
		}
	}
}
