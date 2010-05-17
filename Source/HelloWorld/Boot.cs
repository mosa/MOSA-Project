/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using Mosa.Platforms.x86;
using Mosa.Kernel;
using Mosa.Kernel.X86;

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

            Native.BochsDebug();
            Screen.Write(@"MOSA OS Version 0.6 '");
			Screen.Color = Colors.Red;
            Screen.Write(@"Mammoth");
			Screen.Color = Colors.Yellow;
			Screen.Write(@"'                               Copyright 2008-2010");
			Screen.NextLine();

			Screen.Color = 0x0F;
			for (uint index = 0; index < 80; index++) {
				if (index == 60)
					Screen.Write((char)203);
				else
					Screen.Write((char)205);
			}
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

			Screen.Color = 0x0A;
			Screen.Write(@"Size of Memory:   ");
			Screen.Color = 0x07;
			Screen.Write((Multiboot.MemoryLower + Multiboot.MemoryUpper) / 1024, 10, -1);
			Screen.Write(@" MB (");
			Screen.Write(Multiboot.MemoryLower + Multiboot.MemoryUpper, 10, -1);
			Screen.Write(@" KB)");
			Screen.NextLine();

			Screen.Color = 0x0F;
			for (uint index = 0; index < 60; index++)
				Screen.Write((char)205);

			Screen.NextLine();

			Screen.Color = 0x0A;
			Screen.Write(@"Memory-Map:");
			Screen.NextLine();

			for (uint index = 0; index < Multiboot.MemoryMapCount; index++) {
				Screen.Color = 0x0F;
				Screen.Write(Multiboot.GetMemoryMapBase(index), 16, 10);
				Screen.Write(@" - ");
				Screen.Write(Multiboot.GetMemoryMapBase(index) + Multiboot.GetMemoryMapLength(index) - 1, 16, 10);
				Screen.Write(@" (");
				Screen.Color = 0x07;
				Screen.Write(Multiboot.GetMemoryMapLength(index), 16, 10);
				Screen.Color = 0x0F;
				Screen.Write(@") ");
				Screen.Color = 0x07;
				Screen.Write(@"Type: ");
				Screen.Write(Multiboot.GetMemoryMapType(index), 16, 1);
				Screen.NextLine();
			}

			Screen.SetCursor(18, 0);

			Screen.Color = 0x0F;
			for (uint index = 0; index < 60; index++)
				Screen.Write((char)205);

			Screen.NextLine();
			
			CpuInfo cpuInfo = new CpuInfo();
			#region Vendor
			Screen.Color = 0x0A;
			Screen.Write(@"Vendor:   ");
			Screen.Color = 0x0F;

			cpuInfo.PrintVendorString();

			Screen.NextLine();
			#endregion

			#region Brand
			Screen.Color = 0x0A;
			Screen.Write(@"Brand:    ");
			Screen.Color = 0x0F;
			cpuInfo.PrintBrandString();
			Screen.NextLine();
			#endregion

			#region Stepping
			Screen.Color = 0x0A;
			Screen.Write(@"Stepping: ");
			Screen.Color = 0x0F;
			Screen.Write(cpuInfo.Stepping, 16, 2);
			#endregion

			#region Model
			Screen.Color = 0x0A;
			Screen.Write(@" Model: ");
			Screen.Color = 0x0F;
			Screen.Write(cpuInfo.Model, 16, 2);
			#endregion

			#region Family
			Screen.Color = 0x0A;
			Screen.Write(@" Family: ");
			Screen.Color = 0x0F;
			Screen.Write(cpuInfo.Family, 16, 2);
			#endregion

			#region Type
			Screen.Color = 0x0A;
			Screen.Write(@" Type: ");
			Screen.Color = 0x0F;

			Screen.Write(cpuInfo.Type, 16, 2);
			Screen.NextLine();
            Screen.Color = 0x0A;
			Screen.Write(@"Cores:    ");
            Screen.Color = 0x0F;
            Screen.Write(cpuInfo.NumberOfCores, 16, 2);
			#endregion

			//Multiboot.Dump(4,53);

			Screen.Row = 23;
			for (uint index = 0; index < 80; index++) {
				Screen.Column = index;
				Screen.Write((char)205);
			}

			for (uint index = 2; index < 24; index++) {
				Screen.Column = 60;
				Screen.Row = index;

				Screen.Color = 0x0F;
				if (index == 7)
					Screen.Write((char)185);
				else if (index == 18)
					Screen.Write((char)185);
				else if (index == 23)
					Screen.Write((char)202);
				else
					Screen.Write((char)186);
			}

			Screen.SetCursor(24, 29);
			Screen.Color = 0x0E;
			Screen.Write(@"www.mosa-project.org");

			CMOS cmos = new CMOS();

			while (true) {
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
			Screen.Color = 0x0A;
			Screen.Write(@"Time: ");

			byte bcd = 10;

            if (cmos.BCD)
				bcd = 16;

			Screen.Color = 0x0F;
            Screen.Write(cmos.Hour, bcd, 2);
			Screen.Color = 0x07;
			Screen.Write(':');
			Screen.Color = 0x0F;
            Screen.Write(cmos.Minute, bcd, 2);
			Screen.Color = 0x07;
			Screen.Write(':');
			Screen.Color = 0x0F;
            Screen.Write(cmos.Second, bcd, 2);
			Screen.Write(' ');
			Screen.Color = 0x07;
			Screen.Write('(');
			Screen.Color = 0x0F;
            Screen.Write(cmos.Month, bcd, 2);
			Screen.Color = 0x07;
			Screen.Write('/');
			Screen.Color = 0x0F;
            Screen.Write(cmos.Day, bcd, 2);
			Screen.Color = 0x07;
			Screen.Write('/');
			Screen.Color = 0x0F;
			Screen.Write('2');
			Screen.Write('0');
            Screen.Write(cmos.Year, bcd, 2);
			Screen.Color = 0x07;
			Screen.Write(')');
		}

		/// <summary>
		/// Prints the brand.
		/// </summary>
		/// <param name="param">The param.</param>
		private static void PrintBrand(uint param)
		{
			int identifier = Platforms.x86.Native.CpuIdEax(param);
			if (identifier != 0x20202020)
				for (int i = 0; i < 4; ++i)
					Screen.Write((char)((identifier >> (i * 8)) & 0xFF));

			identifier = Platforms.x86.Native.CpuIdEbx(param);
			if (identifier != 0x20202020)
				for (int i = 0; i < 4; ++i)
					Screen.Write((char)((identifier >> (i * 8)) & 0xFF));

			identifier = Platforms.x86.Native.CpuIdEcx(param);
			if (identifier != 0x20202020)
				for (int i = 0; i < 4; ++i)
					Screen.Write((char)((identifier >> (i * 8)) & 0xFF));

			identifier = Platforms.x86.Native.CpuIdEdx(param);
			if (identifier != 0x20202020)
				for (int i = 0; i < 4; ++i)
					Screen.Write((char)((identifier >> (i * 8)) & 0xFF));
		}

	}
}
