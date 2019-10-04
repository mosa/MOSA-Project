// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.x86;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Mosa.Kernel.x86
{
	public static class Scheduler
	{
		public const int MaxThreads = 256;
		public const int ClockIRQ = 0x20;
		public const int ThreadTerminationSignalIRQ = 254;

		private static bool Enabled;
		private static Pointer SignalThreadTerminationMethodAddress;

		private static Thread[] Threads;

		private static uint CurrentThreadID;

		private static int clockTicks = 0;

		public static uint ClockTicks { get { return (uint)clockTicks; } }

		public static void Setup()
		{
			Enabled = false;
			Threads = new Thread[MaxThreads];
			CurrentThreadID = 0;
			clockTicks = 0;

			for (int i = 0; i < MaxThreads; i++)
			{
				Threads[i] = new Thread();
			}

			var address = GetAddress(IdleThread);

			SignalThreadTerminationMethodAddress = GetAddress(SignalThreadTerminationMethod);

			CreateThread(address, PageFrameAllocator.PageSize, 0);
		}

		public static void Start()
		{
			SetThreadID(0);
			Enabled = true;

			//Native.Cli();
			Native.Int(ClockIRQ);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void IdleThread()
		{
			while (true)
			{
				Native.Hlt();
			}
		}

		public static void ClockInterrupt(Pointer stackSate)
		{
			Interlocked.Increment(ref clockTicks);

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

		[MethodImpl(MethodImplOptions.NoInlining)]
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

		private static Pointer GetAddress(ThreadStart d)
		{
			return Intrinsic.GetDelegateMethodAddress(d);
		}

		private static Pointer GetAddress(ParameterizedThreadStart d)
		{
			return Intrinsic.GetDelegateTargetAddress(d);
		}

		public static uint CreateThread(ThreadStart thread, uint stackSize)
		{
			var address = GetAddress(thread);

			return CreateThread(address, stackSize);
		}

		public static uint CreateThread(Pointer methodAddress, uint stackSize)
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

		private static void CreateThread(Pointer methodAddress, uint stackSize, uint threadID)
		{
			var thread = Threads[threadID];

			var stack = new Pointer(VirtualPageAllocator.Reserve(stackSize));
			var stackTop = stack + stackSize;

			// Setup stack state
			stackTop.Store32(-4, 0);          // Zero Sentinel
			stackTop.Store32(-8, SignalThreadTerminationMethodAddress.ToInt32());  // Address of method that will raise a interrupt signal to terminate thread

			stackTop.Store32(-12, 0x00000202);// EFLAG
			stackTop.Store32(-16, 0x08);      // CS
			stackTop.Store32(-20, methodAddress.ToInt32()); // EIP

			stackTop.Store32(-24, 0);     // ErrorCode - not used
			stackTop.Store32(-28, 0);     // Interrupt Number - not used

			stackTop.Store32(-32, 0);     // EAX
			stackTop.Store32(-36, 0);     // ECX
			stackTop.Store32(-40, 0);     // EDX
			stackTop.Store32(-44, 0);     // EBX
			stackTop.Store32(-48, 0);     // ESP (original) - not used
			stackTop.Store32(-52, (stackTop - 8).ToInt32()); // EBP
			stackTop.Store32(-56, 0);     // ESI
			stackTop.Store32(-60, 0);     // EDI

			thread.Status = ThreadStatus.Running;
			thread.StackBottom = stack;
			thread.StackTop = stackTop;
			thread.StackStatePointer = stackTop - 60;
		}

		private static void SaveThreadState(uint threadID, Pointer stackSate)
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

			Native.InterruptReturn((uint)thread.StackStatePointer.ToInt32());
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
