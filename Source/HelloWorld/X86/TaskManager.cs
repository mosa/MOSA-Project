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
	public static class TaskManager
	{
		private static uint _defaultStackSize = 1024 * 1024 * 4; // 4MB
		private static uint _slots = 4096 * 8;
		private static uint _table;

		#region Data members

		internal struct Status
		{
			public static readonly byte Empty = 0;
			public static readonly byte Running = 1;
			public static readonly byte Terminating = 2;
			public static readonly byte Terminated = 3;
		}

		internal struct Offset
		{
			public static readonly uint Status = 0;
			public static readonly uint ProcessID = 4;
			public static readonly uint TaskID = 8;
			public static readonly uint StackTop = 12;
			public static readonly uint StackBottom = 14;
			public static readonly uint Last = 16;
		}

		#endregion

		/// <summary>
		/// Setups the task manager.
		/// </summary>
		public unsafe static void Setup()
		{
			// Allocate memory for the task table
			_table = VirtualPageAllocator.Reserve((uint)(_slots * Offset.Last));

			uint stack = ProcessManager.AllocateMemory(0, _defaultStackSize);

			// Create idle task
			Memory.Set32(_table + Offset.Status, Status.Running);
			Memory.Set32(_table + Offset.ProcessID, 0);
			Memory.Set32(_table + Offset.TaskID, 0);
			Memory.Set32(_table + Offset.StackTop, stack + _defaultStackSize);
			Memory.Set32(_table + Offset.StackBottom, stack);
		}

		/// <summary>
		/// Creates the task.
		/// </summary>
		/// <returns></returns>
		public unsafe static uint CreateTask(uint processid)
		{
			// TODO: Lock

			uint slot = FindEmptySlot();

			if (slot == 0)
				Panic.Now(5);

			uint task = GetTaskLocation(slot);

			uint stack = ProcessManager.AllocateMemory(processid, _defaultStackSize);

			Memory.Set32(task + Offset.Status, Status.Running);
			Memory.Set32(task + Offset.ProcessID, processid);
			Memory.Set32(task + Offset.TaskID, slot);
			Memory.Set32(task + Offset.StackTop, stack + _defaultStackSize);
			Memory.Set32(task + Offset.StackBottom, stack);

			// TODO: Unlock

			return slot;
		}

		/// <summary>
		/// Terminates the task.
		/// </summary>
		/// <param name="pid">The pid.</param>
		public static void TerminateTask(uint process)
		{
			// TODO

			// 1. Set status to terminating
			// 2. Stop all threads
			// 3. Release all memory
		}

		/// <summary>
		/// Finds an empty slot in the process table.
		/// </summary>
		/// <returns></returns>
		private unsafe static uint FindEmptySlot()
		{
			for (uint slot = 1; slot < _slots; slot++)
				if (Memory.Get32(GetTaskLocation(slot) + Offset.Status) == Status.Empty)
					return slot;

			return 0;
		}

		/// <summary>
		/// Gets the task entry location.
		/// </summary>
		/// <param name="slot">The slot.</param>
		/// <returns></returns>
		private unsafe static uint GetTaskLocation(uint slot)
		{
			return (uint)(_table + (Offset.Last * slot));
		}
	}
}
