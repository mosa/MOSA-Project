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
		private static uint _slots = 4096;
		private static uint _table;

		/// <summary>
		/// Setups this instance.
		/// </summary>
		public unsafe static void Setup()
		{
			// Allocate memory for the process table
			_table = VirtualPageAllocator.Reserve((uint)(_slots * sizeof(Process)));

			// Create idle process

			Process* process = (Process*)(_table);
			process->ProcessId = 0;
			process->MemoryMap = VirtualPageAllocator.Reserve(32 * 4096);	// 32 pages for entire 4GB
			process->Status = State.Running;
		}

		/// <summary>
		/// Creates the process.
		/// </summary>
		/// <returns></returns>
		public unsafe static uint CreateProcess()
		{
			// TODO: Lock

			uint slot = FindEmptySlot();

			if (slot == 0)
				Panic.Now(5);

			Process* process = (Process*)(GetProcessEntryLocation(slot));
			process->ProcessId = slot;
			process->MemoryMap = VirtualPageAllocator.Reserve(32 * 4096);	// 32 pages for entire 4GB
			process->Status = State.Running;

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
			uint address = VirtualPageAllocator.Reserve(size);

			UpdateMemoryBitMap(process, address, size, false);

			return address;
		}

		/// <summary>
		/// Updates the memory bit map.
		/// </summary>
		/// <param name="process">The process.</param>
		/// <param name="address">The address.</param>
		/// <param name="pages">The pages.</param>
		/// <param name="free">if set to <c>true</c> [free].</param>
		private unsafe static void UpdateMemoryBitMap(uint process, uint address, uint size, bool free)
		{
			uint bitmap = ((Process*)process)->MemoryMap;

			for (uint at = address; at < address + size; at = at + PageFrameAllocator.PageSize)
				SetPageStatus(bitmap, at / PageFrameAllocator.PageSize, free);
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
		private unsafe static uint FindEmptySlot()
		{
			for (uint slot = 1; slot < _slots; slot++)
				if (((Process*)(GetProcessEntryLocation(slot)))->Status == State.Empty)
					return slot;

			return 0;
		}

		/// <summary>
		/// Gets the process entry location.
		/// </summary>
		/// <param name="slot">The slot.</param>
		/// <returns></returns>
		private unsafe static uint GetProcessEntryLocation(uint slot)
		{
			return (uint)(_table + (sizeof(Process) * slot));
		}
	}
}
