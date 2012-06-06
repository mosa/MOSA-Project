/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Platform.x86.Intrinsic;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// A virtual page allocator.
	/// </summary>
	/// <remarks>
	/// This is a simple bitmap implementation with no optimizations.
	/// </remarks>
	public static class VirtualPageAllocator
	{
		// Location of bitmap starts at 21MB
		private static uint _bitmap = 1024 * 1024 * 21; // 0x1500000
		private static uint _pages;

		/// <summary>
		/// Setups this instance.
		/// </summary>
		public static void Setup()
		{
			_pages = (PageFrameAllocator.TotalPages - PageFrameAllocator.ReserveMemory) / PageFrameAllocator.PageSize;

			// Bits: 0 = Available, 1 = Not Available
			Memory.Clear(_bitmap, _pages / 8);
		}

		/// <summary>
		/// Gets the index of the page.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		private static unsafe uint GetPageIndex(void* address)
		{
			return (((uint)address) - PageFrameAllocator.ReserveMemory) / PageFrameAllocator.PageSize;
		}

		/// <summary>
		/// Sets the page status in the bitmap.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="free">if set to <c>true</c> [free].</param>
		private static void SetPageStatus(uint page, bool free)
		{
			uint at = (uint)(_bitmap + (page / 32));
			byte bit = (byte)(page % 32);
			uint mask = (byte)(1 << bit);

			uint value = Native.Get32(at);

			if (free)
				value = (uint)(value & ~mask);
			else
				value = (uint)(value | mask);

			Native.Set32(at, value);
		}

		/// <summary>
		/// Gets the page status from the bitmap
		/// </summary>
		/// <param name="page">The page.</param>
		/// <returns></returns>
		private static bool GetPageStatus(uint page)  // true = available
		{
			uint at = (uint)(_bitmap + (page / 8));
			byte bit = (byte)(page % 8);
			byte mask = (byte)(1 << bit);

			byte value = Native.Get8(at);

			return (value & mask) == 0;
		}

		/// <summary>
		/// Reserves the pages.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		public static uint Reserve(uint size)
		{
			uint first = 0xFFFFFFFF; // Marker
			uint pages = ((size - 1) / PageFrameAllocator.PageSize) + 1;

			for (uint at = 0; at < _pages; at++)
			{
				if (GetPageStatus(at))
				{
					if (first == 0xFFFFFFFF)
						first = at;

					if (at - first == pages)
					{
						for (uint index = 0; index < pages; index++)
							SetPageStatus(first + index, false);

						return ((first * PageFrameAllocator.PageSize) + PageFrameAllocator.ReserveMemory);
					}
				}
				else
					first = 0xFFFFFFFF;
			}

			return 0;
		}

		/// <summary>
		/// Releases the pages.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="count">The count.</param>
		public static unsafe void Release(void* address, uint count)
		{
			uint start = GetPageIndex(address);
			for (uint index = 0; index < count; index++)
				SetPageStatus(start + index, true);
		}

	}
}
