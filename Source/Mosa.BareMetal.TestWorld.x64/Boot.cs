// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Mosa.Kernel.BareMetal;
using Mosa.UnitTests.Optimization;

namespace Mosa.BareMetal.TestWorld.x64;

public static class Boot
{
	public static void Main()
	{
		Debug.WriteLine("Boot::Main()");

		Console.BackgroundColor = ConsoleColor.Blue;
		Console.ForegroundColor = ConsoleColor.White;
		Console.Clear();
		Console.WriteLine("Mosa.BareMetal.TextWorld.x64");
		Console.WriteLine();

		Division.DivisionBy7(254u);

		//InterruptManager.SetHandler(ProcessInterrupt);

		//StartThreading();

		// Will never get here!
		//Debug.WriteLine("ERROR: Thread Start Failure");

		Debug.WriteLine("ERROR: Done!");
		Debug.Fatal();
	}

	private static int counter = 0;

	public static void ProcessInterrupt(uint interrupt, uint errorCode)
	{
		Interlocked.Increment(ref counter);

		lock (spinlock)
		{
			Console.SetCursorPosition(0, 23);
			Console.Write("Counter: " + counter + " IRQ: " + interrupt + " Code: " + errorCode);
		}
	}

	private static void StartThreading()
	{
		Debug.WriteLine("Boot::StartThreadTest()");

		Scheduler.CreateThread(Program.EntryPoint);

		Scheduler.CreateThread(Thread1);
		Scheduler.CreateThread(Thread2);
		Scheduler.CreateThread(Thread3);
		Scheduler.CreateThread(Thread4);
		Scheduler.CreateThread(Thread5);

		Scheduler.Start();

		Debug.WriteLine("Boot::StartThreadTest() [Exit]");
	}

	private static readonly object spinlock = new();
	private static int totalticks = 0;

	private static void UpdateThreadTicks(int thread, uint ticks)
	{
		Interlocked.Increment(ref totalticks);

		if (totalticks % 10000 == 0)
		{
			lock (spinlock)
			{
				Console.SetCursorPosition(0, 1 + thread);
				Console.Write("Thread #" + thread + ": " + ticks);
			}
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Thread1()
	{
		var ticks = 0u;

		while (true)
		{
			UpdateThreadTicks(1, ++ticks);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Thread2()
	{
		var ticks = 0u;

		while (true)
		{
			UpdateThreadTicks(2, ++ticks);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Thread3()
	{
		var ticks = 0u;

		while (true)
		{
			UpdateThreadTicks(3, ++ticks);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Thread4()
	{
		var ticks = 0u;

		while (true)
		{
			UpdateThreadTicks(4, ++ticks);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Thread5()
	{
		var ticks = 0u;

		while (true)
		{
			UpdateThreadTicks(5, ++ticks);
		}
	}

	public static void ForceInclude()
	{
		Mosa.Kernel.BareMetal.x64.PlatformPlug.ForceInclude();
	}
}
