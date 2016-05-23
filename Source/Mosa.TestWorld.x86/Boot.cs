// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;
using Mosa.Runtime;
using Mosa.Runtime.x86;
using Mosa.TestWorld.x86.Tests;

namespace Mosa.TestWorld.x86
{
	/// <summary>
	///
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

			//EnterDebugger();
		}

		public static void Start()
		{
			Screen.Color = 0x0;
			Screen.Clear();
			Screen.GotoTop();
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

			DebugClient.Setup(Serial.COM1);
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
			IDT.SetInterruptHandler(ProcessInterrupt);
			Screen.Write('B');
			ConsoleManager.Setup();
			Screen.Write('C');
			Console = ConsoleManager.Controller.Boot;
			Screen.Write('D');

			Console.Color = 0x0E;
			Console.BackgroundColor = 1;
			Console.WriteLine();
			Console.WriteLine();
			Console.Write("!MOSA is alive!");

			Console.WriteLine();

			KernelTest.RunTests();

			DumpStackTrace();

			//System.Threading.SpinLock splk = new System.Threading.SpinLock();

			//bool lockTaken = false;
			//splk.Enter(ref lockTaken);
			//if (splk.IsHeld)
			//	Console.Write("Entered...");

			//lockTaken = false;
			//splk.Enter(ref lockTaken);

			//Console.Write("Should have looped!!!");

			Console.Goto(22, 0);

			Process();
		}

		public static void Process()
		{
			byte last = 0;

			while (true)
			{
				if (CMOS.Second != last)
				{
					last = CMOS.Second;
					DebugClient.SendAlive();
					Screen.Write('.');
				}

				Native.Hlt();
			}
		}

		public unsafe static void DumpStackTrace()
		{
			uint depth = 0;

			while (true)
			{
				var methodDef = Mosa.Runtime.x86.Internal.GetMethodDefinitionFromStackFrameDepth(depth);

				if (methodDef == null)
					return;

				string caller = methodDef->Name;

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
			Console.Color = Colors.Cyan;
			Console.BackgroundColor = Colors.Black;

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

			DebugClient.Setup(Serial.COM1);

			byte last = 0;

			while (true)
			{
				byte second = CMOS.Second;

				if (second % 10 != 5 & last != second)
				{
					last = CMOS.Second;
					DebugClient.SendAlive();
				}
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
