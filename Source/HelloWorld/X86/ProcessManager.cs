/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Kernel.X86
{
	/// <summary>
	/// 
	/// </summary>
	public static class ProcessManager
	{
		private static uint _tablepages = 16;

		private static uint _table;
		private static uint _nextid;
		private static uint _slots;

		/// <summary>
		/// Setups this instance.
		/// </summary>
		public unsafe static void Setup()
		{
			// Allocate pages for the process table
			_table = VirtualPageAllocator.Reserve(_tablepages);
			_slots = (uint)(_tablepages * PageFrameAllocator.PageSize / sizeof(Process));
			_nextid = 0;
		}

		/// <summary>
		/// Creates the process.
		/// </summary>
		/// <returns></returns>
		public unsafe static uint CreateProcess()
		{
			// TODO: Lock

			Process* process = FindEmptySlot();
			process->ProcessId = ++_nextid;
			process->MemoryMap = VirtualPageAllocator.Reserve(32);	// 32 pages for entire 4GB
			process->Status = Process.State.Running;

			// TODO: Unlock

			return (uint)process;
		}

		/// <summary>
		/// Terminates the process.
		/// </summary>
		/// <param name="pid">The pid.</param>
		public static void TerminateProcess(uint process)
		{
			// TODO

			// 1. Set status to terminating
			// 2. Stop all threads
			// 3. Release all memory
		}

		/// <summary>
		/// Allocates the memory.
		/// </summary>
		/// <param name="process">The process.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		public static uint AllocateMemory(uint process, uint size)
		{
			// Normalize 
			uint normsize = (uint)((size + PageFrameAllocator.PageSize - 1) & ~(PageFrameAllocator.PageSize - 1));
			uint pages = normsize / PageFrameAllocator.PageSize;
			uint address = VirtualPageAllocator.Reserve(pages);

			UpdateMemoryBitMap(process, address, pages, false);

			return address;
		}

		/// <summary>
		/// Updates the memory bit map.
		/// </summary>
		/// <param name="process">The process.</param>
		/// <param name="address">The address.</param>
		/// <param name="pages">The pages.</param>
		/// <param name="free">if set to <c>true</c> [free].</param>
		private unsafe static void UpdateMemoryBitMap(uint process, uint address, uint pages, bool free)
		{
			uint bitmap = ((Process*)process)->MemoryMap;
			uint start = address / PageFrameAllocator.PageSize;

			for (uint index = 0; index < pages; index++)
				SetPageStatus(bitmap, start + index, free);
		}

		/// <summary>
		/// Sets the page status in the bitmap.
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="free">if set to <c>true</c> [free].</param>
		private static void SetPageStatus(uint bitmap, uint page, bool free)
		{
			uint at = (uint)(bitmap + (page / 32));
			byte bit = (byte)(page % 32);
			uint mask = (byte)(1 << bit);

			uint value = Memory.Get32(at);

			if (free)
				value = (byte)(value & ~mask);
			else
				value = (byte)(value | mask);

			Memory.Set32(at, value);
		}

		/// <summary>
		/// Finds an empty slot.
		/// </summary>
		/// <returns></returns>
		private unsafe static Process* FindEmptySlot()
		{
			for (uint slot = 0; slot < _slots; slot++) {
				Process* process = (Process*)(_table + sizeof(Process) * slot);

				if (process->Status == Process.State.Empty)
					return process;
			}

			return null;
		}
	}
}
