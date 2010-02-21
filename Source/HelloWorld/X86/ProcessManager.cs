/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Platforms.x86;

namespace Mosa.Kernel.X86
{
	/// <summary>
	/// 
	/// </summary>
	public static class ProcessManager
	{
		private static uint _slots = 4096;
		private static uint _table;
		//private static uint _lock = 0;

		#region Data members

		internal struct _Status
		{
			public static readonly byte Empty = 0;
			public static readonly byte Running = 1;
			public static readonly byte Terminating = 2;
			public static readonly byte Terminated = 3;
		}

		internal struct _Offset
		{
			public static readonly uint Status = 0;
			public static readonly uint ProcessID = 4;
			public static readonly uint MemoryMap = 8;
			public static readonly uint DefaultPriority = 12;
			public static readonly uint MaximumPriority = 16;
			public static readonly uint Lock = 20;
			public static readonly uint TotalSize = 24;
		}

		#endregion

		/// <summary>
		/// Setups the process manager.
		/// </summary>
		public unsafe static void Setup()
		{
			// Allocate memory for the process table
			_table = VirtualPageAllocator.Reserve((uint)(_slots * _Offset.TotalSize));

			// Create idle process
			CreateProcess(0);
		}

		/// <summary>
		/// Creates the process.
		/// </summary>
		/// <returns></returns>
		public static uint CreateProcess()
		{
			// TODO: Lock

			uint slot = FindEmptySlot();

			if (slot == 0)
				Panic.Now(5);

			CreateProcess(slot);

			// TODO: Unlock

			return slot;
		}

		/// <summary>
		/// Creates the process.
		/// </summary>
		/// <returns></returns>
		private static uint CreateProcess(uint slot)
		{
			uint process = GetProcessLocation(slot);

			Memory.Set32(process + _Offset.Status, _Status.Running);
			Memory.Set32(process + _Offset.ProcessID, slot);
			Memory.Set32(process + _Offset.MemoryMap, VirtualPageAllocator.Reserve(32 * 4096));
			Memory.Set32(process + _Offset.Lock, 0);

			return slot;
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
		private static void UpdateMemoryBitMap(uint slot, uint address, uint size, bool free)
		{
			uint process = GetProcessLocation(slot);
			uint bitmap = Memory.Get32(process + _Offset.MemoryMap);

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
		/// Finds an empty slot in the process table.
		/// </summary>
		/// <returns></returns>
		private static uint FindEmptySlot()
		{
			for (uint slot = 1; slot < _slots; slot++)
				if (Memory.Get32(GetProcessLocation(slot) + _Offset.Status) == _Status.Empty)
					return slot;

			return 0;
		}

		/// <summary>
		/// Gets the process entry location.
		/// </summary>
		/// <param name="slot">The slot.</param>
		/// <returns></returns>
		private static uint GetProcessLocation(uint slot)
		{
			return (uint)(_table + (_Offset.TotalSize * slot));
		}
	}
}
