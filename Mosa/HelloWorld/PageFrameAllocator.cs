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
	/// A physical page frame allocator.
	/// </summary>
	public static class PageFrameAllocator
	{
		// Location for memory map starts at 16Mb
		private const uint StartLocation = 1024 * 1024 * 16;
		// Reserve memory up to 24Mb
		private const uint ReserveMemory = 1024 * 1024 * 24;

		// Start of memory map
		private static uint map;
		private static uint at;

		private static uint totalPages;
		private static uint totalUsedPages;

		/// <summary>
		/// Setups the physical page manager
		/// </summary>
		public static void Setup()
		{
			map = StartLocation;
			totalPages = 0;
			totalUsedPages = 0;

			SetupFreeMemory();
		}

		/// <summary>
		/// Setups the free memory.
		/// </summary>
		private static void SetupFreeMemory()
		{
			if (!Multiboot.IsMultiboot())
				return;

			for (uint index = 0; index < Multiboot.MemoryMapCount; index++) {
				uint value = (uint)Multiboot.GetMemoryMapType(index);

				if (value == 1)
					AddFreeMemory((uint)Multiboot.GetMemoryMapBase(index), (uint)Multiboot.GetMemoryMapLength(index));
			}
		}

		/// <summary>
		/// Adds the free memory.
		/// </summary>
		/// <param name="start">The start.</param>
		/// <param name="size">The size.</param>
		public static void AddFreeMemory(uint start, uint size)
		{
			// Normalize 
			uint normstart = (uint)((start + PageSize - 1) & ~(PageSize - 1));
			uint normsize = size - (normstart - start);

			// Adjust if memory below is reserved
			if (normstart < ReserveMemory) {
				normstart = ReserveMemory;
				normsize = normsize - normstart;

				if (normsize < 0)
					return;
			}

			// Populate free table
			for (uint mem = normstart; mem < normstart + normsize; mem = mem + PageSize, at = at + sizeof(int))
				Memory.Set32(at, mem);

			totalPages = totalPages + (normsize / PageSize);
		}

		/// <summary>
		/// Allocate a physical page from the free list
		/// </summary>
		/// <returns>The page</returns>
		public static uint Allocate()
		{
			if (at == map) return 0; // out of memory

			totalUsedPages++;
			uint avail = Memory.Get32(at);
			at = at - sizeof(uint);
			return avail;
		}

		/// <summary>
		/// Releases a page to the free list
		/// </summary>
		/// <param name="page">The page.</param>
		public static void Free(uint page)
		{
			totalUsedPages--;
			at = at + sizeof(uint);
			Memory.Set32(at, page);
		}

		/// <summary>
		/// Retrieves the size of a single memory page.
		/// </summary>
		public static uint PageSize { get { return 4096; } }

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
