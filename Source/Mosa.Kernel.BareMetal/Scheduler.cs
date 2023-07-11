// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;
using System.Threading;
using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

public static class Scheduler
{
	private const int MaxThreads = 256;

	private static Pointer SignalThreadTerminationMethodAddress;

	private static Thread[] Threads;

	private static bool Enabled;

	private static uint CurrentThreadID;

	private static int clockTicks;

	public static uint ClockTicks => (uint)clockTicks;

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

		SignalThreadTerminationMethodAddress = GetAddress(SignalTermination);

		CreateThread(address, 2, 0);
	}

	public static void Start()
	{
		SetThreadID(0);
		Enabled = true;

		Platform.Scheduler.Start();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void IdleThread()
	{
		while (true)
		{
			Platform.Scheduler.Yield();
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
	public static void SignalTermination()
	{
		Platform.Scheduler.SignalTermination();
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
		uint threadID = FindEmptyThreadSlot();

		if (threadID == 0)
		{
			ResetTerminatedThreads();
			threadID = FindEmptyThreadSlot();
		}

		CreateThread(methodAddress, stackSize, threadID);

		return threadID;
	}

	private static void CreateThread(Pointer methodAddress, uint pages, uint threadID)
	{
		var thread = Threads[threadID];

		var stack = VirtualPageAllocator.ReservePages(pages);
		var stackTop = stack + (pages * Page.Size);

		var bottom = Platform.Scheduler.SetupThreadStack(stackTop, methodAddress, SignalThreadTerminationMethodAddress);

		thread.Status = ThreadStatus.Running;
		thread.StackBottom = stack;
		thread.StackTop = stackTop;
		thread.StackStatePointer = bottom;
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
	}

	private static void SetThreadID(uint threadID)
	{
		CurrentThreadID = threadID;
	}

	private static void SwitchToThread(uint threadID)
	{
		var thread = Threads[threadID];

		thread.Ticks++;

		SetThreadID(threadID);

		Platform.Scheduler.SwitchToThread(thread);
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
