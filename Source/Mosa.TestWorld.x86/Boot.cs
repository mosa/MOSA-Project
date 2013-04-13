/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using Mosa.TestWorld.x86.Tests;
using Mosa.Kernel.x86;
using Mosa.Platform.x86.Intrinsic;

namespace Mosa.TestWorld.x86
{
	/// <summary>
	///
	/// </summary>
	public static class Boot
	{
		public static ConsoleSession Console;

		/// <summary>
		/// Mains this instance.
		/// </summary>
		public static void Main()
		{
			Mosa.Kernel.x86.Kernel.Setup();
			DebugClient.Setup(Serial.COM1);
			IDT.SetInterruptHandler(ProcessInterrupt);

			Console = ConsoleManager.Controller.Boot;

			Console.Clear();
			Console.Color = Colors.Yellow;
			Console.BackgroundColor = Colors.Black;

			Console.Write(@"MOSA OS Version 1.2 '");
			Console.Color = Colors.Red;
			Console.Write(@"Titan");
			Console.Color = Colors.Yellow;
			Console.Write(@"'                                Copyright 2008-2012");
			Console.WriteLine();

			Console.Row = 23;
			for (uint index = 0; index < 80; index++)
			{
				Console.Column = index;
				Console.Write((char)205);
			}

			Console.Goto(24, 29);
			Console.Color = Colors.Yellow;

			Console.Write(@"www.mosa-project.org");

			CMOS cmos = new CMOS();

			KernelTest.RunTests();
			Mosa.Test.AssemblyB.Test.Test1();
			Mosa.Test.AssemblyC.Test.Test1();

			byte last = 0;

			while (true)
			{
				DisplayTime(cmos);

				if (cmos.Second != last)
				{
					last = cmos.Second;
					DebugClient.SendAlive();
				}

				DebugClient.Process();
				Native.Hlt();
			}
		}

		/// <summary>
		/// Displays the seconds.
		/// </summary>
		private static void DisplayTime(CMOS cmos)
		{
			Console.Goto(24, 52);
			Console.Color = Colors.Green;
			Console.Write(@"Time: ");

			byte bcd = 10;

			if (cmos.BCD)
				bcd = 16;

			Console.Color = Colors.White;
			Console.Write(cmos.Hour, bcd, 2);
			Console.Color = Colors.Gray;
			Console.Write(':');
			Console.Color = Colors.White;
			Console.Write(cmos.Minute, bcd, 2);
			Console.Color = Colors.Gray;
			Console.Write(':');
			Console.Color = Colors.White;
			Console.Write(cmos.Second, bcd, 2);
			Console.Write(' ');
			Console.Color = Colors.Gray;
			Console.Write('(');
			Console.Color = Colors.White;
			Console.Write(cmos.Month, bcd, 2);
			Console.Color = Colors.Gray;
			Console.Write('/');
			Console.Color = Colors.White;
			Console.Write(cmos.Day, bcd, 2);
			Console.Color = Colors.Gray;
			Console.Write('/');
			Console.Color = Colors.White;
			Console.Write('2');
			Console.Write('0');
			Console.Write(cmos.Year, bcd, 2);
			Console.Color = Colors.Gray;
			Console.Write(')');
		}

		private static uint counter = 0;

		public static void ProcessInterrupt(byte interrupt, byte errorCode)
		{
			uint c = Console.Column;
			uint r = Console.Row;
			byte col = Console.Color;
			byte back = Console.BackgroundColor;

			Console.Column = 31;
			Console.Row = 0;
			Console.Color = Colors.Cyan;
			Console.BackgroundColor = Colors.Black;

			counter++;
			Console.Write(counter, 10, 7);
			Console.Write(':');
			Console.Write(interrupt, 16, 2);
			Console.Write(':');
			Console.Write(errorCode, 16, 2);

			if (interrupt == 14)
			{
				// Page Fault!
				PageFaultHandler.Fault(errorCode);
			}
			else if (interrupt == 0x20)
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
	}
}