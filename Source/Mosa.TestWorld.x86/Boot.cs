// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;
using Mosa.Runtime;
using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;
using Mosa.TestWorld.x86.Tests;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mosa.TestWorld.x86
{
	/// <summary>
	/// Boot
	/// </summary>
	public static class Boot
	{
		[Plug("Mosa.Runtime.StartUp::SetInitialMemory")]
		public static void SetInitialMemory()
		{
			KernelMemory.SetInitialMemory(Address.GCInitialMemory, 0x01000000);
		}

		public static void Main()
		{
			Screen.Clear();
			Screen.BackgroundColor = ScreenColor.Blue;
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

			Scheduler.Setup();
			Screen.Write('B');
			IDT.SetInterruptHandler(ProcessInterrupt);
			Screen.Write('C');
			ConsoleManager.Setup();
			Screen.Write('D');
			Screen.Write('E');
			Screen.WriteLine();
			Screen.WriteLine();

			UnitTest();

			KernelTest.RunTests();
			StackTrace();

			int value = CallReturn10();

			Screen.Write("Return10 Test: ");
			if (value == 10)
				Screen.WriteLine("Ok");
			else
				Screen.WriteLine("Failed");

			StartThreadTest();

			// should never get here
			Screen.Write("!BAD!");

			while (true)
			{
				Native.Hlt();
			}
		}

		private static void StartThreadTest()
		{
			Screen.WriteLine();
			Screen.Color = ScreenColor.Yellow;
			Screen.Write('[');
			Screen.Color = ScreenColor.White;
			Screen.Write("Thread Test");
			Screen.Color = ScreenColor.Yellow;
			Screen.Write(']');
			Screen.WriteLine();

			Scheduler.CreateThread(Thread1, PageFrameAllocator.PageSize);
			Scheduler.CreateThread(Thread2, PageFrameAllocator.PageSize);
			Scheduler.CreateThread(Thread3, PageFrameAllocator.PageSize);
			Scheduler.CreateThread(Thread4, PageFrameAllocator.PageSize);
			Scheduler.CreateThread(Thread5, PageFrameAllocator.PageSize);

			Scheduler.Start();
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
					Screen.Goto(18 + thread, 0);
					Screen.Write("Thread #" + thread.ToString() + ": " + ticks.ToString());
				}

				//Native.Hlt();
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Thread1()
		{
			uint ticks = 0;

			while (true)
			{
				UpdateThreadTicks(1, ++ticks);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Thread2()
		{
			uint ticks = 0;

			while (true)
			{
				UpdateThreadTicks(2, ++ticks);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Thread3()
		{
			uint ticks = 0;

			while (true)
			{
				UpdateThreadTicks(3, ++ticks);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Thread4()
		{
			uint ticks = 0;

			while (true)
			{
				UpdateThreadTicks(4, ++ticks);
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void Thread5()
		{
			uint ticks = 0;

			while (true)
			{
				UpdateThreadTicks(5, ++ticks);
			}
		}

		public unsafe static void StackTrace()
		{
			Screen.Color = ScreenColor.Yellow;
			Screen.Write('[');
			Screen.Color = ScreenColor.White;
			Screen.Write("Stack Trace");
			Screen.Color = ScreenColor.Yellow;
			Screen.Write(']');
			Screen.WriteLine();
			Screen.WriteLine();

			uint depth = 0;

			while (true)
			{
				var methodDef = Runtime.Internal.GetMethodDefinitionFromStackFrameDepth(depth);

				if (methodDef.IsNull)
					return;

				string caller = methodDef.Name;

				if (caller == null)
					return;

				Screen.Write(depth, 10, 2);
				Screen.Write(":");
				Screen.WriteLine(caller);

				depth++;
			}
		}

		private static uint counter = 0;

		public static void ProcessInterrupt(uint interrupt, uint errorCode)
		{
			counter++;

			uint c = Screen.Column;
			uint r = Screen.Row;
			var col = Screen.Color;
			var back = Screen.BackgroundColor;

			Screen.Column = 50;
			Screen.Row = 24;
			Screen.Color = ScreenColor.Cyan;
			Screen.BackgroundColor = ScreenColor.Black;

			Screen.Write(counter, 10, 7);
			Screen.Write(':');
			Screen.Write(interrupt, 16, 2);
			Screen.Write(':');
			Screen.Write(errorCode, 16, 2);

			Screen.Column = c;
			Screen.Row = r;
			Screen.Color = col;
			Screen.BackgroundColor = back;
		}

		[DllImport("Asm/Return10.o", EntryPoint = "Return10")]
		public static extern int Return10();

		public static int CallReturn10()
		{
			return Return10();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void UnitTest()
		{
		}

		public static int Test1()
		{
			return Unsafe.SizeOf<int>();
		}
	}
}
