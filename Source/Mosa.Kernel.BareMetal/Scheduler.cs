// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Mosa.Kernel.BareMetal.IPC;
using Mosa.Runtime;

// NOTE: The scheduler called from the interrupt handler may not allocate memory!

namespace Mosa.Kernel.BareMetal;

public static class Scheduler
{
	#region Private Members

	private const int MaxThreads = 4096;

	private static Pointer SignalThreadTerminationMethodAddress;

	private static Thread[] Threads;

	private static bool Enabled;

	private static Thread CurrentThread;

	private static int clockTicks;

	private static uint NextThreadID;

	private static uint DefaultStackSize => Page.Size * 8;

	#endregion Private Members

	#region Public Members

	public static uint ClockTicks => (uint)clockTicks;

	#endregion Public Members

	#region Public API

	public static void Start()
	{
		Debug.WriteLine("Scheduler:Start()");

		SetCurrentThread(null);
		NextThreadID = 1;
		Enabled = true;

		Platform.Scheduler.Start();

		Debug.WriteLine("Scheduler:Start() [Exit]");
	}

	public static uint CreateThread(ThreadStart thread)
	{
		return CreateThread(thread, DefaultStackSize).Index;
	}

	public static void ClockInterrupt(Pointer stackSate)
	{
		if (!Enabled)
			return;

		Interlocked.Increment(ref clockTicks);

		// Save current stack state
		var thread = GetCurrentThread();
		SaveThreadState(thread, stackSate);

		ScheduleNextThread(thread);
	}

	public static void TerminateCurrentThread()
	{
		if (!Enabled)
			return;

		var thread = GetCurrentThread();

		if (thread != null)
		{
			TerminateThread(thread);
		}

		ScheduleNextThread(thread);
	}

	public static void YieldCurrentThread(Pointer stackSate)
	{
		if (!Enabled)
			return;

		var thread = GetCurrentThread();

		SaveThreadState(thread, stackSate);
		ScheduleNextThread(thread);
	}

	public static void SystemCall(Pointer stackSate, Pointer data)
	{
		if (!Enabled)
			return;

		var thread = GetCurrentThread();

		SaveThreadState(thread, stackSate);

		Sleep(thread);

		ScheduleNextThread(thread);
	}

	public static void SetSystemCallReturn(uint threadID, object response)
	{
		var thread = Threads[threadID];

		if (thread.Status == ThreadStatus.Sleeping)
		{
			Platform.Scheduler.SetReturnObject(thread.StackTop, Intrinsic.GetObjectAddress(response));

			thread.Status = ThreadStatus.Running;
		}
		else
		{
			Debug.WriteLine("WARNING: Thread not sleep");
		}
	}

	#endregion Public API

	#region Internal API

	internal static void Setup()
	{
		Debug.WriteLine("Scheduler:Setup()");

		Enabled = false;
		Threads = new Thread[MaxThreads];
		CurrentThread = null;
		clockTicks = 0;

		for (var i = 0u; i < MaxThreads; i++)
		{
			Threads[i] = new Thread(i);
		}

		SignalThreadTerminationMethodAddress = GetAddress(SignalTermination);

		var address = GetAddress(IdleThread);

		CreateThread(address, 2, 0);

		Debug.WriteLine("Scheduler:Setup() [Exit]");
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void SignalTermination()
	{
		Platform.Scheduler.SignalTermination();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static object SignalSystemCall()
	{
		return Platform.Scheduler.SignalSystemCall();
	}

	internal static void QueueRequestMessage(MessageQueue messageQueue, object data)
	{
		var thread = GetCurrentThread();

		var message = new Message(data, thread);

		//
	}

	#endregion Internal API

	#region Private API

	private static Thread CreateThread(ThreadStart thread, uint stackSize)
	{
		Debug.WriteLine("Scheduler:CreateThread()");

		var address = GetAddress(thread);

		var newthread = CreateThread(address, stackSize);

		Debug.WriteLine("Scheduler:CreateThread() [Exit]");

		return newthread;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void IdleThread()
	{
		while (true)
		{
			Platform.Scheduler.Yield();
		}
	}

	private static void ScheduleNextThread(Thread currentThread)
	{
		var thread = GetNextThread(currentThread);
		SwitchToThread(thread);
	}

	private static void TerminateThread(Thread thread)
	{
		if (thread.Status == ThreadStatus.Running || thread.Status == ThreadStatus.Sleeping)
		{
			thread.Status = ThreadStatus.Terminating;

			// TODO: release stack memory
		}
	}

	private static Thread GetNextThread(Thread currentThread)
	{
		var currentIndex = currentThread.Index;

		if (currentIndex == 0)
			currentIndex = 1;

		var index = currentIndex;

		while (true)
		{
			index++;

			if (index == MaxThreads)
				index = 1;

			var thread = Threads[index];

			if (thread.Status == ThreadStatus.Running)
				return thread;

			if (currentIndex == index)
				return null; // idle thread
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

	private static Thread CreateThread(Pointer methodAddress, uint stackSize)
	{
		Debug.WriteLine("Scheduler:CreateThread(Pointer,uint)");

		var index = FindEmptyThreadSlot();

		if (index == 0)
		{
			ResetTerminatedThreads();
			index = FindEmptyThreadSlot();
		}

		var thread = CreateThread(methodAddress, stackSize, index);

		Debug.WriteLine("Scheduler:CreateThread(Pointer,uint) [Exit]");

		return thread;
	}

	private static Thread CreateThread(Pointer methodAddress, uint pages, uint threadID)
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
		thread.Ticks = 0;
		thread.ID = NextThreadID++;

		Debug.WriteLine("Scheduler:CreateThread(Pointer, uint, uint) [Exit]");

		return thread;
	}

	private static void SaveThreadState(Thread thread, Pointer stackSate)
	{
		thread.StackStatePointer = stackSate;
	}

	private static void Sleep(Thread thread)
	{
		thread.Status = ThreadStatus.Sleeping;
	}

	private static Thread GetCurrentThread()
	{
		return CurrentThread;
	}

	private static void SetCurrentThread(Thread thread)
	{
		CurrentThread = thread;
	}

	private static void SwitchToThread(Thread thread)
	{
		thread.Ticks++;

		SetCurrentThread(thread);

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
