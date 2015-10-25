// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.Internal.x86;
using System.Runtime.InteropServices;

namespace Mosa.Kernel.x86
{
	/// <summary>
	///
	/// </summary>
	public static class ProcessManager
	{
		private static uint slots = 256;

		//private static uint lock = 0;

		#region Data members

		internal struct Status
		{
			public static readonly byte Empty = 0;
			public static readonly byte Running = 1;
			public static readonly byte Terminating = 2;
			public static readonly byte Terminated = 3;
		}

		[StructLayout(LayoutKind.Explicit)]
		internal struct Process
		{
			[FieldOffset(0x00)]
			public uint Status;

			[FieldOffset(0x04)]
			public uint ProcessID;

			[FieldOffset(0x08)]
			public uint MemoryMap;

			[FieldOffset(0x0C)]
			public uint DefaultPriority;

			[FieldOffset(0x10)]
			public uint MaximumPriority;

			[FieldOffset(0x14)]
			public uint Lock;
		}

		#endregion Data members

		/// <summary>
		/// Setups the process manager.
		/// </summary>
		public static void Setup()
		{
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
		/// <param name="slot">The slot.</param>
		/// <returns>The slot.</returns>
		private unsafe static uint CreateProcess(uint slot)
		{
			var process = GetProcessLocation(slot);

			process->Status = Status.Running;
			process->ProcessID = slot;
			process->MemoryMap = VirtualPageAllocator.Reserve(32U * 4096U);
			process->Lock = 0;
			process->DefaultPriority = 7;
			process->MaximumPriority = 255;

			return slot;
		}

		/// <summary>
		/// Terminates the process.
		/// </summary>
		/// <param name="slot">The slot.</param>
		public unsafe static void TerminateProcess(uint slot)
		{
			var process = GetProcessLocation(slot);

			// Set status to terminating
			process->Status = Status.Terminating;

			// Deallocate used memory (pages)
			VirtualPageAllocator.Release(process->MemoryMap, 32U * 4096U);

			// Now here's the weird part, what do we want to do?
			// For the moment decided to set the status to
			// terminated and just leave it, and let the process manager
			// can shift through in the next few cycles and see
			// whether or not it wants to free the slot.
			process->Status = Status.Terminated;
		}

		/// <summary>
		/// Allocates the memory.
		/// </summary>
		/// <param name="slot">The slot.</param>
		/// <param name="size">The size.</param>
		/// <returns>The address.</returns>
		public static uint AllocateMemory(uint slot, uint size)
		{
			uint address = VirtualPageAllocator.Reserve(size);

			UpdateMemoryBitMap(slot, address, size, false);

			return address;
		}

		/// <summary>
		/// Deallocates the memory.
		/// </summary>
		/// <param name="slot">The slot.</param>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		public static void DeallocateMemory(uint slot, uint address, uint size)
		{
			UpdateMemoryBitMap(slot, address, size, true);
		}

		/// <summary>
		/// Updates the memory bit map.
		/// </summary>
		/// <param name="slot">The slot.</param>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		/// <param name="free">if set to <c>true</c> [free].</param>
		private unsafe static void UpdateMemoryBitMap(uint slot, uint address, uint size, bool free)
		{
			var process = GetProcessLocation(slot);
			uint bitmap = process->MemoryMap;

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
			uint at = bitmap + (page / 32);
			byte bit = (byte)(page % 32);
			uint mask = (byte)(1 << bit);

			uint value = Native.Get32(at);

			if (free)
				value = (byte)(value & ~mask);
			else
				value = (byte)(value | mask);

			Native.Set32(at, value);
		}

		/// <summary>
		/// Finds an empty slot in the process table.
		/// </summary>
		/// <returns></returns>
		private unsafe static uint FindEmptySlot()
		{
			for (uint slot = 1; slot < slots; slot++)
				if (GetProcessLocation(slot)->Status == Status.Empty)
					return slot;

			return 0;
		}

		/// <summary>
		/// Gets the process entry location.
		/// </summary>
		/// <param name="slot">The slot.</param>
		/// <returns></returns>
		private unsafe static Process* GetProcessLocation(uint slot)
		{
			return (Process*)(Address.ProcessSlots + (sizeof(Process) * slot));
		}
	}
}
