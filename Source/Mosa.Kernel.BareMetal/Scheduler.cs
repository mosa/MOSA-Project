// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;
using System.Threading;
using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

public static class Scheduler
{
	#region Private Members

	private const int MaxThreads = 256;

	private static Pointer SignalThreadTerminationMethodAddress;

	private static Thread[] Threads;

	private static bool Enabled;

	private static uint CurrentThreadID;

	private static int clockTicks;

	private static uint DefaultStackSize => Page.Size * 8;

	#endregion Private Members

	#region Public Members

	public static uint ClockTicks => (uint)clockTicks;

	#endregion Public Members

	#region Public API

	public static void Start()
	{
		Debug.WriteLine("Scheduler:Start()");

		SetThreadID(0);
		Enabled = true;

		Platform.Scheduler.Start();

		Debug.WriteLine("Scheduler:Start() [Exit]");
	}

	public static uint CreateThread(ThreadStart thread)
	{
		return CreateThread(thread, DefaultStackSize);
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

	public static void SystemCall(Pointer request)
	{
		var threadID = GetCurrentThreadID();

		// TODO: Put the request somewhere

		SleepThread(threadID);
		ScheduleNextThread(threadID);
	}

	public static void SetSystemCallReturn(uint threadID, object response)
	{
		var thread = Threads[threadID];

		Platform.Scheduler.SetReturnObject(thread.StackTop, Intrinsic.GetObjectAddress(response));

		if (thread.Status == ThreadStatus.Sleeping)
		{
			thread.Status = ThreadStatus.Running;
		}
	}

	#endregion Public API

	#region Internal API

	internal static void Setup()
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void SignalTermination()
	{
		Platform.Scheduler.SignalTermination();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static object SignalSystemCall(object obj)
	{
		return Platform.Scheduler.SignalSystemCall(obj);
	}

	private static uint CreateThread(ThreadStart thread, uint stackSize)
	{
		Debug.WriteLine("Scheduler:CreateThread()");

		var address = GetAddress(thread);

		var newthread = CreateThread(address, stackSize);

		Debug.WriteLine("Scheduler:CreateThread() [Exit]");

		return newthread;
	}

	#endregion Internal API

	#region Private API

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void IdleThread()
	{
		while (true)
		{
			Platform.Scheduler.Yield();
		}
	}

	private static void ScheduleNextThread(uint threadID)
	{
		threadID = GetNextThread(threadID);
		SwitchToThread(threadID);
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

	private static void SleepThread(uint threadID)
	{
		var thread = Threads[threadID];

		thread.Status = ThreadStatus.Sleeping;
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

	private static uint CreateThread(Pointer methodAddress, uint stackSize)
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

	#endregion Private API
}
