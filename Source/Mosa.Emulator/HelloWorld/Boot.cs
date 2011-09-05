/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using Mosa.Kernel.x86;
using Mosa.Platform.x86;

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
			Screen.Color = 0x0E;
			Screen.Write('M');
			Screen.Write('O');
			Screen.Write('S');
			Screen.Write('A');
			Screen.Write(' ');
			Screen.Write('O');
			Screen.Write('S');
			Screen.Write(' ');
			Screen.Write('V');
			Screen.Write('e');
			Screen.Write('r');
			Screen.Write('s');
			Screen.Write('i');
			Screen.Write('o');
			Screen.Write('n');
			Screen.Write(' ');
			Screen.Write('0');
			Screen.Write('.');
			Screen.Write('6');
			Screen.Write(' ');
			Screen.Write('\'');
			Screen.Color = 0x0C;
			Screen.Write('T');
			Screen.Write('a');
			Screen.Write('n');
			Screen.Write('i');
			Screen.Write('g');
			Screen.Write('a');
			Screen.Write('w');
			Screen.Write('a');
			Screen.Color = 0x0E;
			Screen.Write('\'');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Color = 0x0E;
			Screen.Write('C');
			Screen.Write('o');
			Screen.Write('p');
			Screen.Write('y');
			Screen.Write('r');
			Screen.Write('i');
			Screen.Write('g');
			Screen.Write('h');
			Screen.Write('t');
			Screen.Write(' ');
			Screen.Write('2');
			Screen.Write('0');
			Screen.Write('0');
			Screen.Write('8');
			Screen.Write('-');
			Screen.Write('2');
			Screen.Write('0');
			Screen.Write('1');
			Screen.Write('0');
			Screen.NextLine();

			Screen.Color = 0x0F;
			for (uint index = 0; index < 80; index++)
			{
				if (index == 60)
					Screen.Write((char)203);
				else
					Screen.Write((char)205);
			}
			Screen.NextLine();

			Screen.SetCursor(2, 0);
			Screen.Color = 0x0A;
			Screen.Write('M');
			Screen.Write('u');
			Screen.Write('l');
			Screen.Write('t');
			Screen.Write('i');
			Screen.Write('b');
			Screen.Write('o');
			Screen.Write('o');
			Screen.Write('t');
			Screen.Write('a');
			Screen.Write('d');
			Screen.Write('d');
			Screen.Write('r');
			Screen.Write('e');
			Screen.Write('s');
			Screen.Write('s');
			Screen.Write(':');
			Screen.Write(' ');
			Screen.Color = 0x07;
			Screen.Write(Native.Get32(0x200004), 16, 8);

			Screen.NextLine();
			Screen.Color = 0x0A;
			Screen.Write('M');
			Screen.Write('a');
			Screen.Write('g');
			Screen.Write('i');
			Screen.Write('c');
			Screen.Write(' ');
			Screen.Write('n');
			Screen.Write('u');
			Screen.Write('m');
			Screen.Write('b');
			Screen.Write('e');
			Screen.Write('r');
			Screen.Write(':');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Color = 0x07;
			Screen.Write(Native.Get32(0x200000), 16, 8);

			Screen.NextLine();
			Screen.Color = 0x0A;
			Screen.Write('M');
			Screen.Write('u');
			Screen.Write('l');
			Screen.Write('t');
			Screen.Write('i');
			Screen.Write('b');
			Screen.Write('o');
			Screen.Write('o');
			Screen.Write('t');
			Screen.Write('-');
			Screen.Write('F');
			Screen.Write('l');
			Screen.Write('a');
			Screen.Write('g');
			Screen.Write('s');
			Screen.Write(':');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Color = 0x07;
			Screen.Write(Multiboot.Flags, 2, 32);
			Screen.NextLine();
			Screen.NextLine();

			Screen.Color = 0x0A;
			Screen.Write('S');
			Screen.Write('i');
			Screen.Write('z');
			Screen.Write('e');
			Screen.Write(' ');
			Screen.Write('o');
			Screen.Write('f');
			Screen.Write(' ');
			Screen.Write('M');
			Screen.Write('e');
			Screen.Write('m');
			Screen.Write('o');
			Screen.Write('r');
			Screen.Write('y');
			Screen.Write(':');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Color = 0x07;
			Screen.Write((Multiboot.MemoryLower + Multiboot.MemoryUpper) / 1024, 10, -1);
			Screen.Write(' ');
			Screen.Write('M');
			Screen.Write('B');
			Screen.Write(' ');
			Screen.Write('(');
			Screen.Write(Multiboot.MemoryLower + Multiboot.MemoryUpper, 10, -1);
			Screen.Write(' ');
			Screen.Write('K');
			Screen.Write('B');
			Screen.Write(')');
			Screen.NextLine();

			Screen.Color = 0x0F;
			for (uint index = 0; index < 60; index++)
				Screen.Write((char)205);

			Screen.NextLine();

			Screen.Color = 0x0A;
			Screen.Write('M');
			Screen.Write('e');
			Screen.Write('m');
			Screen.Write('o');
			Screen.Write('r');
			Screen.Write('y');
			Screen.Write('-');
			Screen.Write('M');
			Screen.Write('a');
			Screen.Write('p');
			Screen.Write(':');
			Screen.NextLine();

			for (uint index = 0; index < Multiboot.MemoryMapCount; index++)
			{
				Screen.Color = 0x0F;
				Screen.Write(Multiboot.GetMemoryMapBase(index), 16, 10);
				Screen.Write(' ');
				Screen.Write('-');
				Screen.Write(' ');
				Screen.Write(Multiboot.GetMemoryMapBase(index) + Multiboot.GetMemoryMapLength(index) - 1, 16, 10);
				Screen.Write(' ');
				Screen.Write('(');
				Screen.Color = 0x07;
				Screen.Write(Multiboot.GetMemoryMapLength(index), 16, 10);
				Screen.Color = 0x0F;
				Screen.Write(')');
				Screen.Write(' ');
				Screen.Color = 0x07;
				Screen.Write('T');
				Screen.Write('y');
				Screen.Write('p');
				Screen.Write('e');
				Screen.Write(':');
				Screen.Write(' ');
				Screen.Write(Multiboot.GetMemoryMapType(index), 16, 1);
				Screen.NextLine();
			}

			Screen.SetCursor(18, 0);

			Screen.Color = 0x0F;
			for (uint index = 0; index < 60; index++)
				Screen.Write((char)205);

			Screen.NextLine();

			#region Vendor
			Screen.Color = 0x0A;
			Screen.Write('V');
			Screen.Write('e');
			Screen.Write('n');
			Screen.Write('d');
			Screen.Write('o');
			Screen.Write('r');
			Screen.Write(':');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Color = 0x0F;

			int identifier = Platform.x86.Native.CpuIdEbx(0);
			for (int i = 0; i < 4; ++i)
				Screen.Write((char)((identifier >> (i * 8)) & 0xFF));

			identifier = Platform.x86.Native.CpuIdEdx(0);
			for (int i = 0; i < 4; ++i)
				Screen.Write((char)((identifier >> (i * 8)) & 0xFF));

			identifier = Platform.x86.Native.CpuIdEcx(0);
			for (int i = 0; i < 4; ++i)
				Screen.Write((char)((identifier >> (i * 8)) & 0xFF));

			Screen.NextLine();
			#endregion

			#region Brand
			Screen.Color = 0x0A;
			Screen.Write('B');
			Screen.Write('r');
			Screen.Write('a');
			Screen.Write('n');
			Screen.Write('d');
			Screen.Write(':');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Color = 0x0F;

			PrintBrand((uint)2147483650);
			PrintBrand((uint)2147483651);
			PrintBrand((uint)2147483652);
			Screen.NextLine();
			#endregion

			int info = Platform.x86.Native.CpuIdEax(1);
			#region Stepping
			Screen.Color = 0x0A;
			Screen.Write('S');
			Screen.Write('t');
			Screen.Write('e');
			Screen.Write('p');
			Screen.Write('p');
			Screen.Write('i');
			Screen.Write('n');
			Screen.Write('g');
			Screen.Write(':');
			Screen.Write(' ');
			Screen.Color = 0x0F;
			Screen.Write((ulong)(info & 0xF), 16, 2);
			#endregion

			#region Model
			Screen.Color = 0x0A;
			Screen.Write(' ');
			Screen.Write('M');
			Screen.Write('o');
			Screen.Write('d');
			Screen.Write('e');
			Screen.Write('l');
			Screen.Write(':');
			Screen.Write(' ');
			Screen.Color = 0x0F;
			Screen.Write((ulong)((info & 0xF0) >> 4), 16, 2);
			#endregion

			#region Family
			Screen.Color = 0x0A;
			Screen.Write(' ');
			Screen.Write('F');
			Screen.Write('a');
			Screen.Write('m');
			Screen.Write('i');
			Screen.Write('l');
			Screen.Write('y');
			Screen.Write(':');
			Screen.Write(' ');
			Screen.Color = 0x0F;
			Screen.Write((ulong)((info & 0xF00) >> 8), 16, 2);
			#endregion

			#region Type
			Screen.Color = 0x0A;
			Screen.Write(' ');
			Screen.Write('T');
			Screen.Write('y');
			Screen.Write('p');
			Screen.Write('e');
			Screen.Write(':');
			Screen.Write(' ');
			Screen.Color = 0x0F;

			Screen.Write((ulong)((info & 0x3000) >> 12), 16, 2);
			Screen.NextLine();
			Screen.Color = 0x0A;
			Screen.Write('C');
			Screen.Write('o');
			Screen.Write('r');
			Screen.Write('e');
			Screen.Write('s');
			Screen.Write(':');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Write(' ');
			Screen.Color = 0x0F;

			info = Platform.x86.Native.CpuIdEax(4);
			Screen.Write((ulong)((info >> 26) + 1), 16, 2);
			#endregion

			//Multiboot.Dump(4,53);

			Screen.Row = 23;
			for (uint index = 0; index < 80; index++)
			{
				Screen.Column = index;
				Screen.Write((char)205);
			}

			for (uint index = 2; index < 24; index++)
			{
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
			Screen.Write('w');
			Screen.Write('w');
			Screen.Write('w');
			Screen.Write('.');
			Screen.Write('m');
			Screen.Write('o');
			Screen.Write('s');
			Screen.Write('a');
			Screen.Write('-');
			Screen.Write('p');
			Screen.Write('r');
			Screen.Write('o');
			Screen.Write('j');
			Screen.Write('e');
			Screen.Write('c');
			Screen.Write('t');
			Screen.Write('.');
			Screen.Write('o');
			Screen.Write('r');
			Screen.Write('g');

			while (true)
			{
				CMOS.Instance.Dump(2, 65);
				DisplayTime();
			}
		}

		/// <summary>
		/// Displays the seconds.
		/// </summary>
		private unsafe static void DisplayTime()
		{
			Screen.SetCursor(24, 52);
			Screen.Color = 0x0A;
			Screen.Write('T');
			Screen.Write('i');
			Screen.Write('m');
			Screen.Write('e');
			Screen.Write(':');
			Screen.Write(' ');

			byte bcd = 10;

			if (CMOS.Instance.BCD)
				bcd = 16;

			Screen.Color = 0x0F;
			Screen.Write(CMOS.Instance.Hour, bcd, 2);
			Screen.Color = 0x07;
			Screen.Write(':');
			Screen.Color = 0x0F;
			Screen.Write(CMOS.Instance.Minute, bcd, 2);
			Screen.Color = 0x07;
			Screen.Write(':');
			Screen.Color = 0x0F;
			Screen.Write(CMOS.Instance.Second, bcd, 2);
			Screen.Write(' ');
			Screen.Color = 0x07;
			Screen.Write('(');
			Screen.Color = 0x0F;
			Screen.Write(CMOS.Instance.Month, bcd, 2);
			Screen.Color = 0x07;
			Screen.Write('/');
			Screen.Color = 0x0F;
			Screen.Write(CMOS.Instance.Day, bcd, 2);
			Screen.Color = 0x07;
			Screen.Write('/');
			Screen.Color = 0x0F;
			Screen.Write('2');
			Screen.Write('0');
			Screen.Write(CMOS.Instance.Year, bcd, 2);
			Screen.Color = 0x07;
			Screen.Write(')');
		}

		/// <summary>
		/// Prints the brand.
		/// </summary>
		/// <param name="param">The param.</param>
		private static void PrintBrand(uint param)
		{
			int identifier = Platform.x86.Native.CpuIdEax(param);
			if (identifier != 0x20202020)
				for (int i = 0; i < 4; ++i)
					Screen.Write((char)((identifier >> (i * 8)) & 0xFF));

			identifier = Platform.x86.Native.CpuIdEbx(param);
			if (identifier != 0x20202020)
				for (int i = 0; i < 4; ++i)
					Screen.Write((char)((identifier >> (i * 8)) & 0xFF));

			identifier = Platform.x86.Native.CpuIdEcx(param);
			if (identifier != 0x20202020)
				for (int i = 0; i < 4; ++i)
					Screen.Write((char)((identifier >> (i * 8)) & 0xFF));

			identifier = Platform.x86.Native.CpuIdEdx(param);
			if (identifier != 0x20202020)
				for (int i = 0; i < 4; ++i)
					Screen.Write((char)((identifier >> (i * 8)) & 0xFF));
		}

	}
}
