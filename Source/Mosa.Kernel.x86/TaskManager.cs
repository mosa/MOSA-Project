// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.Internal.x86;
using System.Runtime.InteropServices;

namespace Mosa.Kernel.x86
{
	/// <summary>
	///
	/// </summary>
	public static class TaskManager
	{
		private static uint defaultStackSize = 1024 * 1024 * 1; // 1MB
		private static uint slots = 3072;
		private static uint currenttask; // Not SMP

		private static System.Threading.SpinLock spinlock;

		#region Data members

		internal struct Status
		{
			public static readonly byte Empty = 0;
			public static readonly byte Running = 1;
			public static readonly byte Terminating = 2;
			public static readonly byte Terminated = 3;
		}

		[StructLayout(LayoutKind.Explicit)]
		internal struct Task
		{
			[FieldOffset(0x00)]
			public uint Status;

			[FieldOffset(0x04)]
			public uint ProcessID;

			[FieldOffset(0x08)]
			public uint TaskID;

			[FieldOffset(0x0C)]
			public uint StackBottom;

			[FieldOffset(0x10)]
			public uint StackTop;

			[FieldOffset(0x14)]
			public uint TickCounter;

			[FieldOffset(0x18)]
			public uint LastCounter;

			[FieldOffset(0x1C)]
			public uint Priority;

			[FieldOffset(0x20)]
			public uint Lock;

			[FieldOffset(0x24)]
			public uint ESP;
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
			unsafe
			{
				// Setup task 0 manually
				var task = GetTaskLocation(0);

				// Setup Task Entry
				task->Status = Status.Running;
				task->ProcessID = 0; // We are process 0
				task->TaskID = 0; // We are task 0
				task->StackBottom = 0x00300000;
				task->StackTop = 0x003FFFFC;
				task->ESP = 0; // We are already running so this will be populated on task switch
			}

			// Set current stack
			currenttask = 0;
		}

		/// <summary>
		/// Creates the task.
		/// </summary>
		/// <returns></returns>
		public static uint CreateTask(uint processid, uint eip)
		{
			bool lck = false;
			spinlock.Enter(ref lck);

			uint slot = FindEmptySlot();

			if (slot == 0)
				Panic.Now(5);

			CreateTask(processid, slot, eip);

			spinlock.Exit();

			return slot;
		}

		/// <summary>
		/// Creates the task.
		/// </summary>
		/// <returns></returns>
		private unsafe static uint CreateTask(uint processid, uint slot, uint eip)
		{
			var task = GetTaskLocation(slot);
			uint stack = ProcessManager.AllocateMemory(processid, defaultStackSize);
			uint stacktop = stack + defaultStackSize;

			// TODO: Add guard pages before and after stack

			// Setup Task Entry
			task->Status = Status.Running;
			task->ProcessID = processid;
			task->TaskID = slot;
			task->StackBottom = stack;
			task->StackTop = stacktop;
			task->ESP = stacktop - StackSetupOffset.InitialSize;

			// Setup Stack
			Native.Set32(stacktop - StackSetupOffset.SENTINEL, 0);  // important for traversing the stack backwards
			Native.Set32(stacktop - StackSetupOffset.EFLAG, 0);
			Native.Set32(stacktop - StackSetupOffset.CS, 0);
			Native.Set32(stacktop - StackSetupOffset.EIP, eip);
			Native.Set32(stacktop - StackSetupOffset.ErrorCode, 0);
			Native.Set32(stacktop - StackSetupOffset.IRQ, 0);
			Native.Set32(stacktop - StackSetupOffset.EAX, 0);
			Native.Set32(stacktop - StackSetupOffset.ECX, 0);
			Native.Set32(stacktop - StackSetupOffset.EDX, 0);
			Native.Set32(stacktop - StackSetupOffset.EBX, 0);
			Native.Set32(stacktop - StackSetupOffset.ESP, stacktop - 4);
			Native.Set32(stacktop - StackSetupOffset.EBP, stacktop - 4);
			Native.Set32(stacktop - StackSetupOffset.ESI, 0);
			Native.Set32(stacktop - StackSetupOffset.EDI, 0);

			return slot;
		}

		/// <summary>
		/// Terminates the task.
		/// </summary>
		/// <param name="slot">The slot.</param>
		public static void TerminateTask(uint slot)
		{
			bool lck = false;
			spinlock.Enter(ref lck);

			// TODO

			// 1. Set status to terminating
			// 2. Stop the thread
			// 3. Release the stack memory

			spinlock.Exit();
		}

		/// <summary>
		/// Finds an empty slot in the process table.
		/// </summary>
		/// <returns></returns>
		private unsafe static uint FindEmptySlot()
		{
			for (uint slot = 1; slot < slots; slot++)
				if (GetTaskLocation(slot)->Status == Status.Empty)
					return slot;

			return 0;
		}

		/// <summary>
		/// Gets the task entry location.
		/// </summary>
		/// <param name="slot">The slot.</param>
		/// <returns></returns>
		private unsafe static Task* GetTaskLocation(uint slot)
		{
			return (Task*)(Address.TaskSlots + (sizeof(Task) * slot));
		}

		public unsafe static void ThreadOut(uint esp)
		{
			// Get Stack Slot Location and Save Stack location
			GetTaskLocation(currenttask)->ESP = esp;
		}

		/// <summary>
		/// Switches the specified esp.
		/// </summary>
		/// <param name="nexttask">The nexttask.</param>
		public unsafe static void Switch(uint nexttask)
		{
			PIC.SendEndOfInterrupt(0x20);

			// Update current task
			currenttask = nexttask;

			// Get Stack location
			uint esp = GetTaskLocation(currenttask)->ESP;

			// Switch task
			Native.SwitchTask(esp);

			// will never reach here
		}
	}
}
