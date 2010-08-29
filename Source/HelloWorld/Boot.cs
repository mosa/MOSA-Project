/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using Mosa.Platforms.x86;
using Mosa.Kernel;
using Mosa.Kernel.X86;
using Mosa.Kernel.X86.Smbios;
using System;

namespace Mosa.HelloWorld
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
			Kernel.Setup();
			Screen.GotoTop();
			Screen.Color = Colors.Yellow;

            Screen.Write(@"MOSA OS Version 0.7 '");
			Screen.Color = Colors.Red;
			Screen.Write(@"Malibu");
			Screen.Color = Colors.Yellow;
			Screen.Write(@"'                                Copyright 2008-2010");
			Screen.NextLine();

			Screen.Color = 0x0F;
			Screen.Write(new String((char)205, 60));
			Screen.Write((char)203);
			Screen.Write(new String((char)205, 19));
			Screen.NextLine();

			Screen.SetCursor(2, 0);
			Screen.Color = Colors.Green;
			Screen.Write(@"Multibootaddress: ");
			Screen.Color = Colors.Gray;
			Screen.Write(Native.Get32(0x200004), 16, 8);

			Screen.NextLine();
			Screen.Color = Colors.Green;
			Screen.Write(@"Magic number:     ");
			Screen.Color = Colors.Gray;
			Screen.Color = 0x07;
			Screen.Write(Native.Get32(0x200000), 16, 8);

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
			Screen.Write (@"Smbios Info: ");
			if (SmbiosManager.IsAvailable)
			{
				Screen.Color = Colors.White;
				Screen.Write (@"[");
				Screen.Color = Colors.Gray;
				Screen.Write (@"Version ");
				Screen.Write (SmbiosManager.MajorVersion, 10, -1);
				Screen.Write (@".");
				Screen.Write (SmbiosManager.MinorVersion, 10, -1);
				Screen.Color = Colors.White;
				Screen.Write (@"]");
				Screen.NextLine ();
				
				Screen.Color = Colors.Yellow;
				Screen.Write (@"[Bios]");
				Screen.Color = Colors.White;
				Screen.NextLine ();
				
				BiosInformationStructure biosInformation = new BiosInformationStructure ();
				Screen.Color = Colors.White;
				Screen.Write (@"Vendor: ");
				Screen.Color = Colors.Gray;
				Screen.Write (biosInformation.BiosVendor);
				Screen.NextLine ();
				
				Screen.Color = Colors.Yellow;
				Screen.Row = 9;
				Screen.Column = 25;
				Screen.Write (@"[Cpu]");
				Screen.Color = Colors.White;
				Screen.NextLine ();
				Screen.Column = 25;
				
				CpuStructure cpuStructure = new CpuStructure ();
				Screen.Color = Colors.White;
				Screen.Write (@"Vendor: ");
				Screen.Color = Colors.Gray;
				Screen.Write (cpuStructure.Vendor);
				Screen.NextLine ();
				Screen.Column = 25;
				Screen.Color = Colors.White;
				Screen.Write (@"Clock Freq.: ");
				Screen.Color = Colors.Gray;
				Screen.Write (cpuStructure.ClockFrequency, 10, -1);
				Screen.Write (@" MHz");
				Screen.NextLine ();
				Screen.Column = 25;
				Screen.Color = Colors.White;
				Screen.Write (@"Max. Speed: ");
				Screen.Color = Colors.Gray;
				Screen.Write (cpuStructure.MaxSpeed, 10, -1);
				Screen.Write (@" MHz");
				Screen.NextLine ();
				Screen.Column = 25;
			}
			else
			{
				Screen.Color = Colors.Red;
				Screen.Write (@"No SMBIOS available on this system!");
			}

			Screen.SetCursor(17, 0);

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
			for (uint index = 0; index < 80; index++) {
				Screen.Column = index;
				Screen.Write((char)205);
			}

			for (uint index = 2; index < 23; index++) {
				Screen.Column = 60;
				Screen.Row = index;

				Screen.Color = Colors.White;
				if (index == 7)
					Screen.Write((char)185);
				else if (index == 17)
					Screen.Write((char)185);
				else if (index == 22)
					Screen.Write((char)202);
				else
					Screen.Write((char)186);
			}

			Screen.SetCursor(24, 29);
			Screen.Color = Colors.Yellow;

			Screen.Write(@"www.mosa-project.org");

			CMOS cmos = new CMOS();

			Mosa.HelloWorld.Tests.StringTest.Test();

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
			Screen.SetCursor(24, 52);
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
	}
}
