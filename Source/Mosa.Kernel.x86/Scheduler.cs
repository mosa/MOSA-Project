// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.x86;
using System.Threading;

namespace Mosa.Kernel.x86
{
	public static class Scheduler
	{
		public const int MaxThreads = 256;
		public const int ClockIRQ = 0x20;
		public const int ThreadTerminationSignalIRQ = 254;

		private static bool Enabled;
		private static uint SignalThreadTerminationMethodAddress;

		private static Thread[] Threads;

		private static uint CurrentThreadID;

		public static void Setup()
		{
			Enabled = false;
			Threads = new Thread[MaxThreads];
			CurrentThreadID = 0;

			for (int i = 0; i < MaxThreads; i++)
			{
				Threads[i] = new Thread();
			}

			uint address = GetAddress(IdleThread);

			SignalThreadTerminationMethodAddress = GetAddress(SignalThreadTerminationMethod);

			CreateThread(address, PageFrameAllocator.PageSize, 0);
		}

		public static void Start()
		{
			SetThreadID(0);
			Enabled = true;
			Native.Int(ClockIRQ);
		}

		private static void IdleThread()
		{
			while (true)
			{
				Native.Hlt();
			}
		}

		public static void ClockInterrupt(uint stackSate)
		{
			if (!Enabled)
				return;

			// Save current stack state
			var threadID = GetCurrentThreadID();
			SaveThreadState(threadID, stackSate);

			ScheduleNextThread(threadID);
		}

		private static void ScheduleNextThread(uint threadID)
		{
			threadID = GetNextThread(threadID);
			SwitchToThread(threadID);
		}

		public static void SignalThreadTerminationMethod()
		{
			Native.Int(ThreadTerminationSignalIRQ);
		}

		public static void TerminateCurrentThread()
		{
			var threadID = GetCurrentThreadID();

			if (threadID != 0)
			{
				TerminateThread(threadID);
			}

			ScheduleNextThread(threadID);
		}

		private static void TerminateThread(uint threadID)
		{
			var thread = Threads[threadID];

			if (thread.Status == ThreadStatus.Running)
			{
				thread.Status = ThreadStatus.Terminating;

				// TODO: release stack memory
			}
		}

		private static uint GetNextThread(uint currentThreadID)
		{
			uint threadID = currentThreadID;

			if (currentThreadID == 0)
				currentThreadID = 1;

			while (true)
			{
				threadID++;

				if (threadID == MaxThreads)
					threadID = 1;

				var thread = Threads[threadID];

				if (thread.Status == ThreadStatus.Running)
					return threadID;

				if (currentThreadID == threadID)
					return 0; // idle thread
			}
		}

		private static uint GetAddress(ThreadStart d)
		{
			return Intrinsic.GetDelegateMethodAddress(d);
		}

		private static uint GetAddress(ParameterizedThreadStart d)
		{
			return Intrinsic.GetDelegateTargetAddress(d);
		}

		public static uint CreateThread(ThreadStart thread, uint stackSize)
		{
			uint address = GetAddress(thread);

			return CreateThread(address, stackSize);
		}

		public static uint CreateThread(uint methodAddress, uint stackSize)
		{
			//Assert.True(stackSize != 0, "CreateThread(): invalid stack size = " + stackSize.ToString());
			//Assert.True(stackSize % PageFrameAllocator.PageSize == 0, "CreateThread(): invalid stack size % PageSize, stack size = " + stackSize.ToString());

			uint threadID = FindEmptyThreadSlot();

			if (threadID == 0)
			{
				ResetTerminatedThreads();
				threadID = FindEmptyThreadSlot();
			}

			//Assert.True(threadID != 0, "CreateThread(): invalid thread id = 0");

			CreateThread(methodAddress, stackSize, threadID);

			return threadID;
		}

		private static void CreateThread(uint methodAddress, uint stackSize, uint threadID)
		{
			var thread = Threads[threadID];

			var stack = VirtualPageAllocator.Reserve(stackSize);
			var stackTop = stack + stackSize;

			// Setup stack state
			Intrinsic.Store32(stackTop, -4, 0);          // Zero Sentinel
			Intrinsic.Store32(stackTop, -8, SignalThreadTerminationMethodAddress);  // Address of method that will raise a interrupt signal to terminate thread

			Intrinsic.Store32(stackTop, -12, 0x00000202);// EFLAG
			Intrinsic.Store32(stackTop, -16, 0x08);      // CS
			Intrinsic.Store32(stackTop, -20, methodAddress); // EIP

			Intrinsic.Store32(stackTop, -24, 0);     // ErrorCode - not used
			Intrinsic.Store32(stackTop, -28, 0);     // Interrupt Number - not used

			Intrinsic.Store32(stackTop, -32, 0);     // EAX
			Intrinsic.Store32(stackTop, -36, 0);     // ECX
			Intrinsic.Store32(stackTop, -40, 0);     // EDX
			Intrinsic.Store32(stackTop, -44, 0);     // EBX
			Intrinsic.Store32(stackTop, -48, 0);     // ESP (original) - not used
			Intrinsic.Store32(stackTop, -52, stackTop - 8); // EBP
			Intrinsic.Store32(stackTop, -56, 0);     // ESI
			Intrinsic.Store32(stackTop, -60, 0);     // EDI

			thread.Status = ThreadStatus.Running;
			thread.StackBottom = stack;
			thread.StackTop = stackTop;
			thread.StackStatePointer = stackTop - 60;
		}

		private static void SaveThreadState(uint threadID, uint stackSate)
		{
			//Assert.True(threadID < MaxThreads, "SaveThreadState(): invalid thread id > max");

			var thread = Threads[threadID];

			//Assert.True(thread != null, "SaveThreadState(): thread id = null");

			thread.StackStatePointer = stackSate;
		}

		private static uint GetCurrentThreadID()
		{
			return CurrentThreadID;

			//return Native.GetFS();
		}

		private static void SetThreadID(uint threadID)
		{
			CurrentThreadID = threadID;

			//Native.SetFS(threadID);
		}

		private static void SwitchToThread(uint threadID)
		{
			var thread = Threads[threadID];

			//Assert.True(thread != null, "invalid thread id");

			thread.Ticks++;

			SetThreadID(threadID);

			PIC.SendEndOfInterrupt(ClockIRQ);

			Native.InterruptReturn(thread.StackStatePointer);
		}

		private static uint FindEmptyThreadSlot()
		{
			for (uint i = 0; i < MaxThreads; i++)
			{
				if (Threads[i].Status == ThreadStatus.Empty)
					return i;
			}

			return 0;
		}

		private static void ResetTerminatedThreads()
		{
			for (uint i = 0; i < MaxThreads; i++)
			{
				if (Threads[i].Status == ThreadStatus.Terminated)
				{
					Threads[i].Status = ThreadStatus.Empty;
				}
			}
		}
	}
}
