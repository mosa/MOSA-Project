// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;
using Mosa.Runtime;
using Mosa.Runtime.x86;
using Mosa.TestWorld.x86.Tests;
using System.Threading;

namespace Mosa.TestWorld.x86
{
	/// <summary>
	/// Boot
	/// </summary>
	public static class Boot
	{
		public static ConsoleSession Console;

		/// <summary>
		/// Main
		/// </summary>
		public static void Main()
		{
			Start();
		}

		public static void Start()
		{
			Screen.Clear();
			Screen.Color = 0x0E;
			Screen.Write('M');
			Screen.Write('O');
			Screen.Write('S');
			Screen.Write('A');
			Screen.Write(' ');
			Screen.Write('O');
			Screen.Write('S');
			Screen.Write("!");
			Screen.Write(" ");

			Debugger.Setup(Serial.COM1);
			Screen.Write('0');
			IDT.SetInterruptHandler(null);
			Screen.Write('1');
			Multiboot.Setup();
			Screen.Write('2');
			PIC.Setup();
			Screen.Write('3');
			GDT.Setup();
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

			Console.Color = 0x0E;
			Console.BackgroundColor = 1;
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

		private static SpinLock spinlock = new SpinLock();
		private static uint totalticks = 0;

		private static void UpdateThreadTicks(uint thread, uint ticks)
		{
			++totalticks;

			if (totalticks % 10000 == 0)
			{
				bool taken = false;
				spinlock.Enter(ref taken);

				Console.Goto(0, 14 + thread * 13);
				Console.Write("T" + thread.ToString() + ":" + ticks.ToString());

				spinlock.Exit();

				//Native.Hlt();
			}
		}

		//private static object test = new object();

		//public static void Test()
		//{
		//	lock (test)
		//	{
		//		totalticks++;
		//	}
		//}

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
				var methodDef = Mosa.Runtime.x86.Internal.GetMethodDefinitionFromStackFrameDepth(depth);

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
			byte col = Console.Color;
			byte back = Console.BackgroundColor;

			Console.Column = 50;
			Console.Row = 24;
			Console.Color = Color.Cyan;
			Console.BackgroundColor = Color.Black;

			Console.Write(counter, 10, 7);
			Console.Write(':');
			Console.Write(interrupt, 16, 2);
			Console.Write(':');
			Console.Write(errorCode, 16, 2);

			if (interrupt == 0x20)
			{
				// Timer Interrupt! Switch Tasks!
			}
			else
			{
				Console.Write('-');
				Console.Write(counter, 10, 7);
				Console.Write(':');
				Console.Write(interrupt, 16, 2);

				if (interrupt == 0x21)
				{
					byte scancode = Keyboard.ReadScanCode();
					Console.Write('-');
					Console.Write(scancode, 16, 2);
				}
			}

			Console.Column = c;
			Console.Row = r;
			Console.Color = col;
			Console.BackgroundColor = back;
		}

		public static void EnterDebugger()
		{
			Screen.Color = 0x0;
			Screen.Clear();
			Screen.GotoTop();
			Screen.Color = 0x0E;
			Screen.Write('D');
			Screen.Write('E');
			Screen.Write('B');
			Screen.Write('U');
			Screen.Write('G');
			Screen.NextLine();
			Screen.NextLine();

			Debugger.Setup(Serial.COM1);

			while (true)
			{
				Native.Hlt();
			}
		}

		public static void LockTest()
		{
			var o = new object();

			lock (o)
			{
				var i = o.GetHashCode();
			}
		}

		//public static void Mandelbrot()
		//{
		//	double xmin = -2.1;
		//	double ymin = -1.3;
		//	double xmax = 1;
		//	double ymax = 1.3;

		//	int Width = 200;
		//	int Height = 200;

		//	double x, y, x1, y1, xx;

		//	int looper, s, z = 0;
		//	double intigralX, intigralY = 0.0;

		//	intigralX = (xmax - xmin) / Width; // Make it fill the whole window
		//	intigralY = (ymax - ymin) / Height;
		//	x = xmin;

		//	for (s = 1; s < Width; s++)
		//	{
		//		y = ymin;
		//		for (z = 1; z < Height; z++)
		//		{
		//			x1 = 0;
		//			y1 = 0;
		//			looper = 0;
		//			while (looper < 100 && Math.Sqrt((x1 * x1) + (y1 * y1)) < 2)
		//			{
		//				looper++;
		//				xx = (x1 * x1) - (y1 * y1) + x;
		//				y1 = 2 * x1 * y1 + y;
		//				x1 = xx;
		//			}

		//			// Get the percent of where the looper stopped
		//			double perc = looper / (100.0);
		//			// Get that part of a 255 scale
		//			int val = ((int)(perc * 255));
		//			// Use that number to set the color

		//			//map[s, z]= value;

		//			y += intigralY;
		//		}
		//		x += intigralX;
		//	}
		//}
	}
}
