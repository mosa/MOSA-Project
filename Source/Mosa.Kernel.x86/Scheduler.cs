// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.x86;
using System.Threading;

namespace Mosa.Kernel.x86
{
	public static class Scheduler
	{
		public const int MaxThreads = 256;
		public const int ClockIRQ = 40;
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

		public static void SchedulerInterrupt(uint stackSate)
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

		private static void CreateThread(uint methoAddress, uint stackSize, uint threadID)
		{
			var thread = Threads[threadID];

			var stack = VirtualPageAllocator.Reserve(stackSize);
			uint stackStateSize = 52;
			var stackTop = stack + stackSize;

			// Setup stack state
			Native.Set32(stackTop - 4, 0);          // Zero Sentinel
			Native.Set32(stackTop - 8, SignalThreadTerminationMethodAddress);  // Address of method that will raise a interrupt signal to terminate thread

			Native.Set32(stackTop - 4 - 8, 0);      // EFLAG
			Native.Set32(stackTop - 8 - 8, 0);      // CS
			Native.Set32(stackTop - 12 - 8, methoAddress);    // EIP
			Native.Set32(stackTop - 16 - 8, 0);     // ErrorCode - not used
			Native.Set32(stackTop - 20 - 8, 0);     // Interrupt Number - not used
			Native.Set32(stackTop - 24 - 8, 0);     // EAX
			Native.Set32(stackTop - 28 - 8, 0);     // ECX
			Native.Set32(stackTop - 32 - 8, 0);     // EDX
			Native.Set32(stackTop - 36 - 8, 0);     // EBX
			Native.Set32(stackTop - 40 - 8, stackTop - 8); // ESP (original) - not used
			Native.Set32(stackTop - 44 - 8, stackTop - 8); // EBP
			Native.Set32(stackTop - 48 - 8, 0);     // ESI
			Native.Set32(stackTop - 52 - 8, 0);     // EDI

			thread.Status = ThreadStatus.Running;
			thread.StackBottom = stack;
			thread.StackTop = stackTop;
			thread.StackStatePointer = stackTop - stackStateSize;
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
