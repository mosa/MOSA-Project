// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.CompilerServices;
using Mosa.Kernel.BareMetal;
using Mosa.UnitTests.Optimization;

namespace Mosa.BareMetal.TestWorld.x86;

public static class Boot
{
	public static void Main()
	{
		Debug.WriteLine("Boot::Main()");

		Console.BackgroundColor = ConsoleColor.Blue;
		Console.ForegroundColor = ConsoleColor.White;
		Console.Clear();
		Console.WriteLine("Mosa.BareMetal.TextWold.x86");
		Console.WriteLine();

		Division.DivisionBy7(254u);

		InterruptManager.SetHandler(ProcessInterrupt);

		StartThreadTest();

		Program.EntryPoint();
	}

	private static uint counter = 0;

	public static void ProcessInterrupt(uint interrupt, uint errorCode)
	{
		counter++;

		Console.SetCursorPosition(0, 23);
		Console.Write("Counter: " + counter + " IRQ: " + interrupt + " Code: " + errorCode);
	}

	private static void StartThreadTest()
	{
		Debug.WriteLine("Boot::StartThreadTest()");

		Scheduler.CreateThread(Thread1, Page.Size);
		Scheduler.CreateThread(Thread2, Page.Size);
		Scheduler.CreateThread(Thread3, Page.Size);
		Scheduler.CreateThread(Thread4, Page.Size);
		Scheduler.CreateThread(Thread5, Page.Size);

		Scheduler.Start();

		Debug.WriteLine("Boot::StartThreadTest() [Exit]");
	}

	private static readonly object spinlock = new();
	private static uint totalticks = 0;

	private static void UpdateThreadTicks(int thread, uint ticks)
	{
		++totalticks;

		if (totalticks % 10000 == 0)
		{
			lock (spinlock)
			{
				Console.SetCursorPosition(18 + thread, 0);
				Console.Write("Thread #" + thread + ": " + ticks);
			}

			//Native.Hlt();
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
}
