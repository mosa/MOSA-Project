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
	/// 
	/// </summary>
	public static class TaskManager
	{
		private static uint _defaultStackSize = 1024 * 1024 * 4; // 4MB
		private static uint _slots = 4096 * 8;
		private static uint _table;
		private static uint _currenttask; // Not SMP 
		//private static uint _lock = 0;

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
			public static readonly uint StackBottom = 12;
			public static readonly uint StackTop = 16;
			public static readonly uint TickCounter = 20;
			public static readonly uint LastCounter = 24;
			public static readonly uint Priority = 28;
			public static readonly uint Lock = 32;
			public static readonly uint ESP = 36;
			public static readonly uint TotalSize = 40;
		}

		internal struct StackSetupOffset
		{
			public static readonly uint EFLAG = 0;
			public static readonly uint CS = 4;
			public static readonly uint EIP = 8;
			public static readonly uint ErrorCode = 12;
			public static readonly uint IRQ = 16;
			public static readonly uint EAX = 20;
			public static readonly uint ECX = 24;
			public static readonly uint EDX = 28;
			public static readonly uint EBX = 32;
			public static readonly uint ESP = 36;
			public static readonly uint EBP = 40;
			public static readonly uint ESI = 44;
			public static readonly uint EDI = 48;
			public static readonly uint InitialSize = 54;
		}

		#endregion

		/// <summary>
		/// Setups the task manager.
		/// </summary>
		public static unsafe void Setup()
		{
			// Allocate memory for the task table
			_table = (uint)VirtualPageAllocator.Reserve((uint)(_slots * Offset.TotalSize));

			uint stack = ProcessManager.AllocateMemory(0, _defaultStackSize);

			// Create idle task
			CreateTask(0, 0);

			// Set current stack
			_currenttask = 0;
		}

		/// <summary>
		/// Creates the task.
		/// </summary>
		/// <returns></returns>
		public static uint CreateTask(uint processid, uint eip)
		{
			// TODO: Lock

			uint slot = FindEmptySlot();

			if (slot == 0)
				Panic.Now(5);

			CreateTask(processid, slot, eip);

			// TODO: Unlock

			return slot;
		}

		/// <summary>
		/// Creates the task.
		/// </summary>
		/// <returns></returns>
		private static uint CreateTask(uint processid, uint slot, uint eip)
		{
			// TODO: Lock

			uint task = GetTaskLocation(slot);
			uint stack = ProcessManager.AllocateMemory(processid, _defaultStackSize);
			uint stacktop = stack + _defaultStackSize;
			// TODO: Add guard pages before and after stack

			// Setup Task Entry
			Native.Set32(task + Offset.Status, Status.Running);
			Native.Set32(task + Offset.ProcessID, processid);
			Native.Set32(task + Offset.TaskID, slot);
			Native.Set32(task + Offset.StackBottom, stack);
			Native.Set32(task + Offset.StackTop, stacktop);
			Native.Set32(task + Offset.ESP, stack + StackSetupOffset.InitialSize); // TODO

			// Setup Stack
			Native.Set32(stacktop - StackSetupOffset.EFLAG, 0);
			Native.Set32(stacktop - StackSetupOffset.CS, 0);
			Native.Set32(stacktop - StackSetupOffset.EIP, eip);
			Native.Set32(stacktop - StackSetupOffset.ErrorCode, 0);
			Native.Set32(stacktop - StackSetupOffset.IRQ, 0);
			Native.Set32(stacktop - StackSetupOffset.EAX, 0);
			Native.Set32(stacktop - StackSetupOffset.ECX, 0);
			Native.Set32(stacktop - StackSetupOffset.EDX, 0);
			Native.Set32(stacktop - StackSetupOffset.EBX, 0);
			Native.Set32(stacktop - StackSetupOffset.ESP, stacktop);
			Native.Set32(stacktop - StackSetupOffset.EBP, stacktop);
			Native.Set32(stacktop - StackSetupOffset.ESI, 0);
			Native.Set32(stacktop - StackSetupOffset.EDI, 0);

			// TODO: Unlock

			return slot;
		}

		/// <summary>
		/// Terminates the task.
		/// </summary>
		/// <param name="slot">The slot.</param>
		public static void TerminateTask(uint slot)
		{
			// TODO

			// 1. Set status to terminating
			// 2. Stop the thread
			// 3. Release the stack memory
		}

		/// <summary>
		/// Finds an empty slot in the process table.
		/// </summary>
		/// <returns></returns>
		private static uint FindEmptySlot()
		{
			for (uint slot = 1; slot < _slots; slot++)
				if (Native.Get32(GetTaskLocation(slot) + Offset.Status) == Status.Empty)
					return slot;

			return 0;
		}

		/// <summary>
		/// Gets the task entry location.
		/// </summary>
		/// <param name="slot">The slot.</param>
		/// <returns></returns>
		private static uint GetTaskLocation(uint slot)
		{
			return (uint)(_table + (Offset.TotalSize * slot));
		}

		public static void ThreadOut(uint esp)
		{
			// Get Stack Slot Location
			uint task = GetTaskLocation(_currenttask);

			// Save Stack location
			Native.Set32(task + Offset.ESP, esp);
		}

		/// <summary>
		/// Switches the specified esp.
		/// </summary>
		/// <param name="nexttask">The nexttask.</param>
		public static void Switch(uint nexttask)
		{
			ProgrammableInterruptController.SendEndOfInterrupt(0x20);

			// Update current task
			_currenttask = nexttask;

			// Get Stack Slot Location
			uint task = GetTaskLocation(_currenttask);

			// Get Stack location
			uint esp = Native.Get32(task + Offset.ESP);

			// Switch task
			Native.SwitchTask(esp);

			// will never reach here
		}
	}
}
