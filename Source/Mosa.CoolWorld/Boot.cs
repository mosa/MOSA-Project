/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System;

using Mosa.Platform.x86.Intrinsic;
using Mosa.Kernel;
using Mosa.Kernel.x86;
using Mosa.DeviceSystem;

namespace Mosa.CoolWorld
{
	/// <summary>
	/// 
	/// </summary>
	public static class Boot
	{
		/// <summary>
		/// Main
		/// </summary>
		public static void Main()
		{
			Mosa.Kernel.x86.Kernel.Setup();

			IDT.handleInterrupt = ProcessInterrupt;

			Console.Initialize();

			Screen.GotoTop();
			Screen.Color = Colors.White;
			Screen.BackgroundColor = Colors.Green;

			Screen.Write(@"                   MOSA OS Version 0.1 - Compiler Version 1.0");
			Screen.FillLine();
			Screen.Color = Colors.White;
			Screen.BackgroundColor = Colors.Black;

			Mosa.Kernel.x86.Smbios.BiosInformationStructure biosInfo = new Kernel.x86.Smbios.BiosInformationStructure();
			Mosa.Kernel.x86.Smbios.CpuStructure cpuInfo = new Kernel.x86.Smbios.CpuStructure();

			Console.WriteLine("> Checking BIOS...");
			BulletPoint(); Console.Write("Vendor  "); InBrackets(biosInfo.BiosVendor, Colors.White, Colors.LightBlue); Console.WriteLine();
			BulletPoint(); Console.Write("Version "); InBrackets(biosInfo.BiosVersion, Colors.White, Colors.LightBlue); Console.WriteLine();

			Console.WriteLine("> Checking CPU...");
			BulletPoint(); Console.Write("Vendor  "); InBrackets(cpuInfo.Vendor, Colors.White, Colors.LightBlue); Console.WriteLine();
			BulletPoint(); Console.Write("Version "); InBrackets(cpuInfo.Version, Colors.White, Colors.LightBlue); Console.WriteLine();

			Console.WriteLine("> Initializing hardware abstraction layer...");
			Setup.Initialize();

			Console.WriteLine("> Adding hardware devices...");
			Setup.Start();

			Console.WriteLine("> System ready");
			Console.WriteLine();
			Screen.Goto(24, 0);
			Screen.Color = Colors.White;
			Screen.BackgroundColor = Colors.Green;
			Screen.Write("          Copyright (C) 2008-2001 [Managed Operating System Alliance]");
			Screen.FillLine();

			while (true)
			{
				Native.Hlt();
			}
		}

		public static void PrintDone()
		{
			InBrackets("Done", Colors.White, Colors.LightGreen); Console.WriteLine();
		}

		public static void BulletPoint()
		{
			Screen.Color = Colors.Yellow;
			Console.Write("  * ");
			Screen.Color = Colors.White;
		}

		public static void InBrackets(string message, byte outerColor, byte innerColor)
		{
			Screen.Color = outerColor;
			Console.Write("[");
			Screen.Color = innerColor; 
			Console.Write(message);
			Screen.Color = outerColor; 
			Console.Write("]"); 
		}


		private static uint counter = 0;

		public static void ProcessInterrupt(byte interrupt, uint errorCode)
		{
			uint c = Screen.Column;
			uint r = Screen.Row;
			byte col = Screen.Color;
			byte back = Screen.BackgroundColor;

			Screen.Column = 55;
			Screen.Row = 23;
			Screen.Color = Colors.Cyan;
			Screen.BackgroundColor = Colors.Black;

			counter++;
			Screen.Write(counter, 10, 7);
			Screen.Write(':');
			Screen.Write(interrupt, 16, 2);
			Screen.Write(':');
			Screen.Write(errorCode, 16, 2);

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
				Screen.Write('-');
				Screen.Write(counter, 10, 7);
				Screen.Write(':');
				Screen.Write(interrupt, 16, 2);
			}

			Screen.Column = c;
			Screen.Row = r;
			Screen.Color = col;
			Screen.BackgroundColor = back;
		}

	}
}
