/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System;
using Mosa.HelloWorld.x86.Tests;
using Mosa.Kernel;
using Mosa.Kernel.x86;
using Mosa.Kernel.x86.Smbios;

namespace Mosa.HelloWorld.x86
{
	/// <summary>
	/// 
	/// </summary>
	public static class Boot
	{

		/// <summary>
		/// Mains this instance.
		/// </summary>
		public static void Main()
		{
			Mosa.Kernel.x86.Kernel.Setup();

			IDT.SetInterruptHandler(ProcessInterrupt);

			Screen.GotoTop();
			Screen.Color = Colors.Yellow;

			Screen.Write(@"MOSA OS Version 1.0 '");
			Screen.Color = Colors.Red;
			Screen.Write(@"Titan");
			Screen.Color = Colors.Yellow;
			Screen.Write(@"'                                Copyright 2008-2011");
			Screen.NextLine();

			Screen.Color = 0x0F;
			Screen.Write(new String((char)205, 60));
			Screen.Write((char)203);
			Screen.Write(new String((char)205, 19));
			Screen.NextLine();

			Screen.Goto(2, 0);
			Screen.Color = Colors.Green;
			Screen.Write(@"Multibootaddress: ");
			Screen.Color = Colors.Gray;
			Screen.Write(Multiboot.MultibootStructure, 16, 8);

			Screen.NextLine();
			Screen.Color = Colors.Green;
			Screen.Write(@"Multiboot-Flags:  ");
			Screen.Color = Colors.Gray;
			Screen.Write(Multiboot.Flags, 2, 32);
			Screen.NextLine();
			Screen.NextLine();

			Screen.Color = Colors.Green;
			Screen.Write(@"Size of Memory:   ");
			Screen.Color = Colors.Gray;
			Screen.Write((Multiboot.MemoryLower + Multiboot.MemoryUpper) / 1024, 10, -1);
			Screen.Write(@" MB (");
			Screen.Write(Multiboot.MemoryLower + Multiboot.MemoryUpper, 10, -1);
			Screen.Write(@" KB)");
			Screen.NextLine();

			Screen.Color = Colors.White;
			for (uint index = 0; index < 60; index++)
				Screen.Write((char)205);

			Screen.NextLine();

			/*Screen.Color = Colors.Green;
			Screen.Write(@"Memory-Map:");
			Screen.NextLine();

			for (uint index = 0; index < Multiboot.MemoryMapCount; index++)
			{
				Screen.Color = Colors.White;
				Screen.Write(Multiboot.GetMemoryMapBase(index), 16, 10);
				Screen.Write(@" - ");
				Screen.Write(Multiboot.GetMemoryMapBase(index) + Multiboot.GetMemoryMapLength(index) - 1, 16, 10);
				Screen.Write(@" (");
				Screen.Color = Colors.Gray;
				Screen.Write(Multiboot.GetMemoryMapLength(index), 16, 10);
				Screen.Color = Colors.White;
				Screen.Write(@") ");
				Screen.Color = Colors.Gray;
				Screen.Write(@"Type: ");
				Screen.Write(Multiboot.GetMemoryMapType(index), 16, 1);
				Screen.NextLine();
			}*/

			Screen.Color = Colors.Green;
			Screen.Write(@"Smbios Info: ");
			if (SmbiosManager.IsAvailable)
			{
				Screen.Color = Colors.White;
				Screen.Write(@"[");
				Screen.Color = Colors.Gray;
				Screen.Write(@"Version ");
				Screen.Write(SmbiosManager.MajorVersion, 10, -1);
				Screen.Write(@".");
				Screen.Write(SmbiosManager.MinorVersion, 10, -1);
				Screen.Color = Colors.White;
				Screen.Write(@"]");
				Screen.NextLine();

				Screen.Color = Colors.Yellow;
				Screen.Write(@"[Bios]");
				Screen.Color = Colors.White;
				Screen.NextLine();

				BiosInformationStructure biosInformation = new BiosInformationStructure();
				Screen.Color = Colors.White;
				Screen.Write(@"Vendor: ");
				Screen.Color = Colors.Gray;
				Screen.Write(biosInformation.BiosVendor);
				Screen.NextLine();
				Screen.Color = Colors.White;
				Screen.Write(@"Version: ");
				Screen.Color = Colors.Gray;
				Screen.Write(biosInformation.BiosVersion);
				Screen.NextLine();
				Screen.Color = Colors.White;
				Screen.Write(@"Date: ");
				Screen.Color = Colors.Gray;
				Screen.Write(biosInformation.BiosDate);

				Screen.Color = Colors.Yellow;
				Screen.Row = 8;
				Screen.Column = 35;
				Screen.Write(@"[Cpu]");
				Screen.Color = Colors.White;
				Screen.NextLine();
				Screen.Column = 35;

				CpuStructure cpuStructure = new CpuStructure();
				Screen.Color = Colors.White;
				Screen.Write(@"Vendor: ");
				Screen.Color = Colors.Gray;
				Screen.Write(cpuStructure.Vendor);
				Screen.NextLine();
				Screen.Column = 35;
				Screen.Color = Colors.White;
				Screen.Write(@"Version: ");
				Screen.Color = Colors.Gray;
				Screen.Write(cpuStructure.Version);
				Screen.NextLine();
				Screen.Column = 35;
				Screen.Color = Colors.White;
				Screen.Write(@"Socket: ");
				Screen.Color = Colors.Gray;
				Screen.Write(cpuStructure.Socket);
				Screen.Write(@" MHz");
				Screen.NextLine();
				Screen.Column = 35;
				Screen.Color = Colors.White;
				Screen.Write(@"Cur. Speed: ");
				Screen.Color = Colors.Gray;
				Screen.Write(cpuStructure.MaxSpeed, 10, -1);
				Screen.Write(@" MHz");
				Screen.NextLine();
				Screen.Column = 35;
			}
			else
			{
				Screen.Color = Colors.Red;
				Screen.Write(@"No SMBIOS available on this system!");
			}

			Screen.Goto(17, 0);

			Screen.Color = 0x0F;
			for (uint index = 0; index < 60; index++)
				Screen.Write((char)205);

			Screen.NextLine();

			CpuInfo cpuInfo = new CpuInfo();

			#region Vendor
			Screen.Color = Colors.Green;
			Screen.Write(@"Vendor:   ");
			Screen.Color = Colors.White;

			cpuInfo.PrintVendorString();

			Screen.NextLine();
			#endregion

			#region Brand
			Screen.Color = Colors.Green;
			Screen.Write(@"Brand:    ");
			Screen.Color = Colors.White;
			cpuInfo.PrintBrandString();
			Screen.NextLine();
			#endregion

			#region Stepping
			Screen.Color = Colors.Green;
			Screen.Write(@"Stepping: ");
			Screen.Color = Colors.White;
			Screen.Write(cpuInfo.Stepping, 16, 2);
			#endregion

			#region Model
			Screen.Color = Colors.Green;
			Screen.Write(@" Model: ");
			Screen.Color = Colors.White;
			Screen.Write(cpuInfo.Model, 16, 2);
			#endregion

			#region Family
			Screen.Color = Colors.Green;
			Screen.Write(@" Family: ");
			Screen.Color = Colors.White;
			Screen.Write(cpuInfo.Family, 16, 2);
			#endregion

			#region Type
			Screen.Color = Colors.Green;
			Screen.Write(@" Type: ");
			Screen.Color = Colors.White;

			Screen.Write(cpuInfo.Type, 16, 2);
			Screen.NextLine();
			Screen.Color = Colors.Green;
			Screen.Write(@"Cores:    ");
			Screen.Color = Colors.White;
			Screen.Write(cpuInfo.NumberOfCores, 16, 2);
			#endregion

			//Multiboot.Dump(4,53);

			Screen.Row = 22;
			for (uint index = 0; index < 80; index++)
			{
				Screen.Column = index;
				Screen.Write((char)205);
			}

			for (uint index = 2; index < 23; index++)
			{
				Screen.Column = 60;
				Screen.Row = index;

				Screen.Color = Colors.White;
				if (index == 6)
					Screen.Write((char)185);
				else if (index == 17)
					Screen.Write((char)185);
				else if (index == 22)
					Screen.Write((char)202);
				else
					Screen.Write((char)186);
			}

			Screen.Goto(24, 29);
			Screen.Color = Colors.Yellow;

			Screen.Write(@"www.mosa-project.org");

			CMOS cmos = new CMOS();

			KernelTest.RunTests();

			while (true)
			{
				cmos.Dump(2, 65);
				DisplayTime(cmos);
			}
		}

		/// <summary>
		/// Displays the seconds.
		/// </summary>
		private static void DisplayTime(CMOS cmos)
		{
			Screen.Goto(24, 52);
			Screen.Color = Colors.Green;
			Screen.Write(@"Time: ");

			byte bcd = 10;

			if (cmos.BCD)
				bcd = 16;

			Screen.Color = Colors.White;
			Screen.Write(cmos.Hour, bcd, 2);
			Screen.Color = Colors.Gray;
			Screen.Write(':');
			Screen.Color = Colors.White;
			Screen.Write(cmos.Minute, bcd, 2);
			Screen.Color = Colors.Gray;
			Screen.Write(':');
			Screen.Color = Colors.White;
			Screen.Write(cmos.Second, bcd, 2);
			Screen.Write(' ');
			Screen.Color = Colors.Gray;
			Screen.Write('(');
			Screen.Color = Colors.White;
			Screen.Write(cmos.Month, bcd, 2);
			Screen.Color = Colors.Gray;
			Screen.Write('/');
			Screen.Color = Colors.White;
			Screen.Write(cmos.Day, bcd, 2);
			Screen.Color = Colors.Gray;
			Screen.Write('/');
			Screen.Color = Colors.White;
			Screen.Write('2');
			Screen.Write('0');
			Screen.Write(cmos.Year, bcd, 2);
			Screen.Color = Colors.Gray;
			Screen.Write(')');
		}

		private static uint counter = 0;

		public static void ProcessInterrupt(byte interrupt, byte errorCode)
		{
			uint c = Screen.Column;
			uint r = Screen.Row;
			byte col = Screen.Color;
			byte back = Screen.BackgroundColor;

			Screen.Column = 31;
			Screen.Row = 0;
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

				if (interrupt == 0x21)
				{
					byte scancode = Keyboard.ReadScanCode();
					Screen.Write('-');
					Screen.Write(scancode, 16, 2);
				}
			}

			Screen.Column = c;
			Screen.Row = r;
			Screen.Color = col;
			Screen.BackgroundColor = back;
		}
	}
}
