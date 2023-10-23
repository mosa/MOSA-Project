﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
		Debug.WriteLine("Scheduler:Setup()");

		Enabled = false;
		Threads = new Thread[MaxThreads];
		CurrentThreadID = 0;
		clockTicks = 0;

		for (var i = 0; i < MaxThreads; i++)
		{
			Threads[i] = new Thread();
		}

		var address = GetAddress(IdleThread);

		SignalThreadTerminationMethodAddress = GetAddress(SignalTermination);

		CreateThread(address, 2, 0);

		Debug.WriteLine("Scheduler:Setup() [Exit]");
	}

	public static void Start()
	{
		Debug.WriteLine("Scheduler:Start()");

		SetThreadID(0);
		Enabled = true;

		Platform.Scheduler.Start();

		Debug.WriteLine("Scheduler:Start() [Exit]");
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
		if (!Enabled)
			return;

		Interlocked.Increment(ref clockTicks);

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
		if (!Enabled)
			return;

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
		Debug.WriteLine("Scheduler:CreateThread()");

		var address = GetAddress(thread);

		var newthread = CreateThread(address, stackSize);

		Debug.WriteLine("Scheduler:CreateThread() [Exit]");

		return newthread;
	}

	public static uint CreateThread(Pointer methodAddress, uint stackSize)
	{
		Debug.WriteLine("Scheduler:CreateThread(Pointer,uint)");

		var threadID = FindEmptyThreadSlot();

		if (threadID == 0)
		{
			ResetTerminatedThreads();
			threadID = FindEmptyThreadSlot();
		}

		CreateThread(methodAddress, stackSize, threadID);

		Debug.WriteLine("Scheduler:CreateThread(Pointer,uint) [Exit]");

		return threadID;
	}

	private static void CreateThread(Pointer methodAddress, uint pages, uint threadID)
	{
		Debug.WriteLine("Scheduler:CreateThread(Pointer, uint, uint)");

		var thread = Threads[threadID];

		var stack = VirtualPageAllocator.ReservePages(pages);
		var stackTop = stack + (pages * Page.Size);

		var bottom = Platform.Scheduler.SetupThreadStack(stackTop, methodAddress, SignalThreadTerminationMethodAddress);

		thread.Status = ThreadStatus.Running;
		thread.StackBottom = stack;
		thread.StackTop = stackTop;
		thread.StackStatePointer = bottom;

		Debug.WriteLine("Scheduler:CreateThread(Pointer, uint, uint) [Exit]");
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
		Debug.WriteLine("Scheduler:FindEmptyThreadSlot()");

		for (var i = 0u; i < MaxThreads; i++)
		{
			if (Threads[i].Status == ThreadStatus.Empty)
				return i;
		}

		Debug.WriteLine("Scheduler:FindEmptyThreadSlot() [Exit]");

		return 0;
	}

	private static void ResetTerminatedThreads()
	{
		Debug.WriteLine("Scheduler:ResetTerminatedThreads()");

		for (var i = 0u; i < MaxThreads; i++)
		{
			if (Threads[i].Status == ThreadStatus.Terminated)
			{
				Threads[i].Status = ThreadStatus.Empty;
			}
		}

		Debug.WriteLine("Scheduler:ResetTerminatedThreads() [Exit]");
	}
}
