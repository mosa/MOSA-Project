/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using Mosa.Kernel.x86;
using Mosa.Platform.Internal.x86;
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

			SSE.Setup();
			Screen.Write('0');
			//DebugClient.Setup(Serial.COM1);
			IDT.SetInterruptHandler(null);
			Screen.Write('1');
			Multiboot.Setup();
			Screen.Write('2');
			ProgrammableInterruptController.Setup();
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
			ProcessManager.Setup();
			Screen.Write('9');
			TaskManager.Setup();
			Screen.Write('0');
			IDT.SetInterruptHandler(ProcessInterrupt);
			Screen.Write('A');
			ConsoleManager.Setup();
			Screen.Write('B');
			CMOS cmos = new CMOS();
			Screen.Write('C');
			int second = cmos.Second;
			Screen.Write('D');
			Console = ConsoleManager.Controller.Boot;
			Screen.Write('E');

			Console.Color = 0x0E;
			Console.BackgroundColor = 1;
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("MOSA is alive!");

			Console.WriteLine();

			KernelTest.RunTests();

			Process();
		}

		public static void Process()
		{
			CMOS cmos = new CMOS();
			byte last = 0;

			while (true)
			{
				if (cmos.Second != last)
				{
					last = cmos.Second;
					//DebugClient.SendAlive();
					Screen.Write('.');
				}

				//DebugClient.Process();

				Native.Hlt();
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

		public static void Test()
		{
			//Mosa.Test.Collection.UInt64Tests.DivU8U8(18446744073709551615, 4294967294);
		}
	}
}
