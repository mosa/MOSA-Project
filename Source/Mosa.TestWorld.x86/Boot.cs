﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;
using Mosa.Runtime;
using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;
using Mosa.TestWorld.x86.Tests;

namespace Mosa.TestWorld.x86
{
	/// <summary>
	/// Boot
	/// </summary>
	public static class Boot
	{
		public static ConsoleSession Console;

		[Plug("Mosa.Runtime.StartUp::SetInitialMemory")]
		public static void SetInitialMemory()
		{
			KernelMemory.SetInitialMemory(Address.GCInitialMemory, 0x01000000);
		}

		public static void Main()
		{
			Screen.Clear();
			Screen.Color = ScreenColor.Yellow;
			Screen.Write('M');
			Screen.Write('O');
			Screen.Write('S');
			Screen.Write('A');
			Screen.Write(' ');
			Screen.Write('O');
			Screen.Write('S');
			Screen.Write("!");
			Screen.Write(" ");

			Multiboot.Setup();
			Screen.Write('0');
			GDT.Setup();
			Screen.Write('1');

			IDT.SetInterruptHandler(null);
			Screen.Write('2');
			Debugger.Setup(Serial.COM1);

			Screen.Write('3');
			PIC.Setup();
			Screen.Write('4');
			IDT.Setup();
			Screen.Write('5');
			PageFrameAllocator.Setup();
			Screen.Write('6');
			PageTable.Setup();
			Screen.Write('7');
			VirtualPageAllocator.Setup();
			Screen.Write('8');
			GC.Setup();
			Screen.Write('9');
			Runtime.Internal.Setup();
			Screen.Write('A');

			Scheduler.Setup();
			Screen.Write('B');
			IDT.SetInterruptHandler(ProcessInterrupt);
			Screen.Write('C');
			ConsoleManager.Setup();
			Screen.Write('D');
			Console = ConsoleManager.Controller.Boot;
			Screen.Write('E');

			Console.Color = ScreenColor.Yellow;
			Console.BackgroundColor = ScreenColor.Blue;
			Console.WriteLine();
			Console.WriteLine();

			KernelTest.RunTests();

			DumpStackTrace();

			Console.Goto(2, 0);

			Scheduler.CreateThread(Thread1, PageFrameAllocator.PageSize);
			Scheduler.CreateThread(Thread2, PageFrameAllocator.PageSize);
			Scheduler.CreateThread(Thread3, PageFrameAllocator.PageSize);
			Scheduler.CreateThread(Thread4, PageFrameAllocator.PageSize);

			Scheduler.Start();

			// should never get here
			Screen.Write("!BAD!");

			while (true)
			{
				Native.Hlt();
			}
		}

		private static readonly object spinlock = new object();

		private static uint totalticks = 0;

		private static void UpdateThreadTicks(uint thread, uint ticks)
		{
			++totalticks;

			if (totalticks % 10000 == 0)
			{
				lock (spinlock)
				{
					Console.Goto(0, 14 + (thread * 13));
					Console.Write("T" + thread.ToString() + ":" + ticks.ToString());
				}

				//Native.Hlt();
			}
		}

		public static void Thread1()
		{
			uint ticks = 0;

			while (true)
			{
				UpdateThreadTicks(1, ++ticks);
			}
		}

		public static void Thread2()
		{
			uint ticks = 0;

			while (true)
			{
				UpdateThreadTicks(2, ++ticks);
			}
		}

		public static void Thread3()
		{
			uint ticks = 0;

			while (true)
			{
				UpdateThreadTicks(3, ++ticks);
			}
		}

		public static void Thread4()
		{
			uint ticks = 0;

			while (true)
			{
				UpdateThreadTicks(4, ++ticks);
			}
		}

		public unsafe static void DumpStackTrace()
		{
			uint depth = 0;

			while (true)
			{
				var methodDef = Runtime.x86.Internal.GetMethodDefinitionFromStackFrameDepth(depth);

				if (methodDef.IsNull)
					return;

				string caller = methodDef.Name;

				if (caller == null)
					return;

				Console.Write(depth, 10, 2);
				Console.Write(":");
				Console.WriteLine(caller);

				depth++;
			}
		}

		private static uint counter = 0;

		public static void ProcessInterrupt(uint interrupt, uint errorCode)
		{
			counter++;

			uint c = Console.Column;
			uint r = Console.Row;
			var col = Console.Color;
			var back = Console.BackgroundColor;

			Console.Column = 50;
			Console.Row = 24;
			Console.Color = ScreenColor.Cyan;
			Console.BackgroundColor = ScreenColor.Black;

			Console.Write(counter, 10, 7);
			Console.Write(':');
			Console.Write(interrupt, 16, 2);
			Console.Write(':');
			Console.Write(errorCode, 16, 2);

			Console.Column = c;
			Console.Row = r;
			Console.Color = col;
			Console.BackgroundColor = back;
		}
	}
}
