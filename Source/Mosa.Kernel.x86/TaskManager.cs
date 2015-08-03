// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.Internal.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	///
	/// </summary>
	public static class TaskManager
	{
		private static uint defaultStackSize = 1024 * 1024 * 4; // 4MB
		private static uint slots = 4096 * 8;
		private static uint table;
		private static uint currenttask; // Not SMP

		//private static uint lock = 0;

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
			public static readonly uint SENTINEL = 0;
			public static readonly uint EFLAG = 4;
			public static readonly uint CS = 8;
			public static readonly uint EIP = 12;
			public static readonly uint ErrorCode = 16;
			public static readonly uint IRQ = 20;
			public static readonly uint EAX = 24;
			public static readonly uint ECX = 28;
			public static readonly uint EDX = 32;
			public static readonly uint EBX = 36;
			public static readonly uint ESP = 40;
			public static readonly uint EBP = 44;
			public static readonly uint ESI = 48;
			public static readonly uint EDI = 52;
			public static readonly uint InitialSize = 56;
		}

		#endregion Data members

		/// <summary>
		/// Setups the task manager.
		/// </summary>
		public static void Setup()
		{
			// Allocate memory for the task table
			table = (uint)VirtualPageAllocator.Reserve(slots * Offset.TotalSize);

			uint stack = ProcessManager.AllocateMemory(0, defaultStackSize);

			// Create idle task
			CreateTask(0, 0);

			// Set current stack
			currenttask = 0;
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
			uint stack = ProcessManager.AllocateMemory(processid, defaultStackSize);
			uint stacktop = stack + defaultStackSize;

			// TODO: Add guard pages before and after stack

			// Setup Task Entry
			Native.Set32(task + Offset.Status, Status.Running);
			Native.Set32(task + Offset.ProcessID, processid);
			Native.Set32(task + Offset.TaskID, slot);
			Native.Set32(task + Offset.StackBottom, stack);
			Native.Set32(task + Offset.StackTop, stacktop);
			Native.Set32(task + Offset.ESP, stack + StackSetupOffset.InitialSize); // TODO

			// Setup Stack
			Native.Set32(stacktop - StackSetupOffset.SENTINEL, 0);	// important for traversing the stack backwards
			Native.Set32(stacktop - StackSetupOffset.EFLAG, 0);
			Native.Set32(stacktop - StackSetupOffset.CS, 0);
			Native.Set32(stacktop - StackSetupOffset.EIP, eip);
			Native.Set32(stacktop - StackSetupOffset.ErrorCode, 0);
			Native.Set32(stacktop - StackSetupOffset.IRQ, 0);
			Native.Set32(stacktop - StackSetupOffset.EAX, 0);
			Native.Set32(stacktop - StackSetupOffset.ECX, 0);
			Native.Set32(stacktop - StackSetupOffset.EDX, 0);
			Native.Set32(stacktop - StackSetupOffset.EBX, 0);
			Native.Set32(stacktop - StackSetupOffset.ESP, stacktop + 4);
			Native.Set32(stacktop - StackSetupOffset.EBP, stacktop + 4);
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
			for (uint slot = 1; slot < slots; slot++)
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
			return (uint)(table + (Offset.TotalSize * slot));
		}

		public static void ThreadOut(uint esp)
		{
			// Get Stack Slot Location
			uint task = GetTaskLocation(currenttask);

			// Save Stack location
			Native.Set32(task + Offset.ESP, esp);
		}

		/// <summary>
		/// Switches the specified esp.
		/// </summary>
		/// <param name="nexttask">The nexttask.</param>
		public static void Switch(uint nexttask)
		{
			PIC.SendEndOfInterrupt(0x20);

			// Update current task
			currenttask = nexttask;

			// Get Stack Slot Location
			uint task = GetTaskLocation(currenttask);

			// Get Stack location
			uint esp = Native.Get32(task + Offset.ESP);

			// Switch task
			Native.SwitchTask(esp);

			// will never reach here
		}
	}
}