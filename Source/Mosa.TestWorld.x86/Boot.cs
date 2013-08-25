/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using Mosa.Kernel.x86;
using Mosa.Platform.x86.Intrinsic;
using Mosa.Test.Collection;
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
		/// Mains this instance.
		/// </summary>
		public static void Main()
		{
			Write('*', 3);

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

			SSE.Setup();
			Write('0', 0x0E);
			//DebugClient.Setup(Serial.COM1);
			IDT.SetInterruptHandler(null);
			Write('1', 0x0E);
			Multiboot.Setup();
			Write('2', 0x0E);
			ProgrammableInterruptController.Setup();
			Write('3', 0x0E);
			GDT.Setup();
			Write('4', 0x0E);
			IDT.Setup();
			Write('5', 0x0E);
			PageFrameAllocator.Setup();
			Write('6', 0x0E);
			//PageTable.Setup();
			//VirtualPageAllocator.Setup();
			Write('7', 0x0E);

			RunTests();
			Write('8', 0x0E);

			CMOS cmos = new CMOS();
			Write('9', 0x0E);

			//Console.Write(@"www.mosa-project.org");

			byte last = 0;

			while (true)
			{

				if (cmos.Second != last)
				{
					last = cmos.Second;
					//DebugClient.SendAlive();
					Screen.Write(".");
				}

				//DebugClient.Process();

				Native.Hlt();
			}
		}

		public static void ProcessInterrupt(byte interrupt, byte errorCode)
		{
			Screen.Write("!");
		}

		public static void RunTests()
		{
			DoubleTests.CeqR8R8(1d, 1d);
			//if (DoubleTests.AddR8R8(1d, 1d) == 2d)
			//	Screen.Write("!");
		}

		public static void Write(char chr, byte color)
		{
			Native.Set8(0x0B8040, (byte)chr);
			Native.Set8(0x0B8041, color);
		}

	}
}