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
		// Maximum memory Usage (4Gb)
		private const uint MaximumMemory = 0xFFFFFFFF;

		// Start of memory map
		private static uint _map;
		// Current position in map data structure
		private static uint _at;

		private static uint _totalPages;
		private static uint _totalUsedPages;

		/// <summary>
		/// Setup the physical page manager
		/// </summary>
		public static void Setup()
		{
			_map = StartLocation;
			_totalPages = 0;
			_totalUsedPages = 0;

			SetupFreeMemory();
		}

		/// <summary>
		/// Setups the free memory.
		/// </summary>
		private static void SetupFreeMemory()
		{
			uint cnt = 0;

			if (!Multiboot.IsMultiboot())
				return;

			for (uint index = 0; index < Multiboot.MemoryMapCount; index++) {
				byte value = Multiboot.GetMemoryMapType(index);

				Mosa.HelloWorld.Screen.SetCursor(22, index);
				Mosa.HelloWorld.Screen.Write(value);

				ulong start = Multiboot.GetMemoryMapBase(index);
				ulong size = Multiboot.GetMemoryMapLength(index);

				if (value == 1)
					AddFreeMemory(cnt++, (uint)start, (uint)size);
			}
		}

		/// <summary>
		/// Adds the free memory.
		/// </summary>
		/// <param name="cnt">The count.</param>
		/// <param name="start">The start.</param>
		/// <param name="size">The size.</param>
		public static void AddFreeMemory(uint cnt, uint start, uint size)
		{
			if ((start > MaximumMemory) || (start + size < ReserveMemory))
				return;
			
			//if ((start + size) > MaximumMemory)
			//    size = MaximumMemory - start;

			// Normalize 
			uint normstart = (uint)((start + PageSize - 1) & ~(PageSize - 1));
			uint normend = (uint)((start + size) & ~(PageSize - 1));
			uint normsize = (uint)(normend - normstart);

			// Adjust if memory below is reserved
			if (normstart < ReserveMemory) {
				normsize = (normstart + normsize) - ReserveMemory;
				normstart = ReserveMemory;

				if (normsize <= 0)
					return;
			}

			// Populate free table
			for (uint mem = normstart; mem < normstart + normsize; mem = mem + PageSize, _at = _at + sizeof(int))
				Memory.Set32(_at, mem);

			_totalPages = _totalPages + (normsize / PageSize);
		}

		/// <summary>
		/// Allocate a physical page from the free list
		/// </summary>
		/// <returns>The page</returns>
		public static uint Allocate()
		{
			if (_at == _map) return 0; // out of memory

			_totalUsedPages++;
			uint avail = Memory.Get32(_at);
			_at = _at - sizeof(uint);
			return avail;
		}

		/// <summary>
		/// Releases a page to the free list
		/// </summary>
		/// <param name="page">The page.</param>
		public static void Free(uint page)
		{
			_totalUsedPages--;
			_at = _at + sizeof(uint);
			Memory.Set32(_at, page);
		}

		/// <summary>
		/// Retrieves the size of a single memory page.
		/// </summary>
		public static uint PageSize { get { return 4096; } }

		/// <summary>
		/// Retrieves the amount of total physical memory pages available in the system.
		/// </summary>
		public static uint TotalPages { get { return _totalPages; } }

		/// <summary>
		/// Retrieves the amount of number of physical pages in use.
		/// </summary>
		public static uint TotalPagesInUse { get { return _totalUsedPages; } }
	}
}
