/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */
using Mosa.Kernel.Memory.X86;
using Mosa.Platforms.x86;

namespace Mosa.HelloWorld
{
	/// <summary>
	/// 
	/// </summary>
	public static class Boot
	{

		/// <summary>
		/// 
		/// </summary>
		private static uint counter = 0;

		/// <summary>
		/// Mains this instance.
		/// </summary>
		public static void Main()
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
			Screen.Write('1');
			Screen.Write(' ');
			Screen.Write('\'');
			Screen.Color = 0x0C;
			Screen.Write('W');
			Screen.Write('a');
			Screen.Write('k');
			Screen.Write('e');
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
			Screen.Write('0');
			Screen.Write('9');
			Screen.NextLine();

			Screen.Color = 0x0F;
			for (uint index = 0; index < 80; index++) {
				if (index == 51)
					Screen.Write((char)203);
				else
					Screen.Write((char)205);
			}
			Screen.NextLine();

			Screen.SetCursor(0, 2);
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
			Screen.Write(Memory.Get32(0x100000), 16, 8);
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
			Screen.Write(Memory.Get32(0x100004), 16, 8);

			Multiboot.SetMultibootLocation(Memory.Get32(0x100004), Memory.Get32(0x100000));
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
			for (uint index = 0; index < 51; index++) {
				Screen.Write((char)205);
			}
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

			for (uint index = 0; index < Multiboot.MemoryMapCount; index++) {
				Screen.Color = 0x0F;
				Screen.Write(Multiboot.GetMemoryMapBaseLow(index), 16, 10);
				Screen.Write(' ');
				Screen.Write('-');
				Screen.Write(' ');
				Screen.Write(Multiboot.GetMemoryMapBaseLow(index) + Multiboot.GetMemoryMapLengthLow(index), 16, 10);
				Screen.Write(' ');
				Screen.Write('(');
				Screen.Color = 0x07;
				Screen.Write(Multiboot.GetMemoryMapLengthLow(index), 16, 10);
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

			Screen.Column = 53;
			Screen.Row = 2;
			Screen.Color = 0x0A;
			Screen.Write('M');
			Screen.Write('e');
			Screen.Write('m');
			Screen.Write('o');
			Screen.Write('r');
			Screen.Write('y');
			Screen.Write('d');
			Screen.Write('u');
			Screen.Write('m');
			Screen.Write('p');
			Screen.Color = 0x0F;

			Screen.Row = 23;
			for (int index = 0; index < 80; index++) {
				Screen.Column = index;
				Screen.Write((char)205);
			}


			for (int index = 2; index < 24; index++) {
				Screen.Column = 51;
				Screen.Row = index;

				Screen.Color = 0x0F;
				if (index == 7)
					Screen.Write((char)185);
				else if (index == 23)
					Screen.Write((char)202);
				else
					Screen.Write((char)186);
			}

			Multiboot.Dump();
			//Multiboot.Dump2();

			Screen.SetCursor(29, 24);
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

			CMOS.InitializeClock();

			while (true) 
				DisplayTime();
		}

		/// <summary>
		/// Displays the seconds.
		/// </summary>
		private unsafe static void DisplayTime()
		{
			//while (0x80 == CMOS.Get(0x0A)) ;
			
			Screen.SetCursor(52, 24);
			Screen.Color = 0x0A;
			Screen.Write('T');
			Screen.Write('i');
			Screen.Write('m');
			Screen.Write('e');
			Screen.Write(':');
			Screen.Write(' ');

			Screen.Color = 0x0F;
			Screen.Write(CMOS.Hour, 10, 2);
			Screen.Color = 0x07;
			Screen.Write(':');
			Screen.Color = 0x0F;
			Screen.Write(CMOS.Minute, 10, 2);
			Screen.Color = 0x07;
			Screen.Write(':');
			Screen.Color = 0x0F;
			Screen.Write(CMOS.Second, 10, 2);
			Screen.Write(' ');
			Screen.Color = 0x07;
			Screen.Write('(');
			Screen.Color = 0x0F;
			Screen.Write(CMOS.Month, 10, 2);
			Screen.Color = 0x07;
			Screen.Write('/');
			Screen.Color = 0x0F;
			Screen.Write(CMOS.Day, 10, 2);
			Screen.Color = 0x07;
			Screen.Write('/');
			Screen.Color = 0x0F;
			Screen.Write('2');
			Screen.Write('0');
			Screen.Write(CMOS.Year, 10, 2);
			Screen.Color = 0x07;
			Screen.Write(')');
		}

		/// <summary>
		/// Displays the counter.
		/// </summary>
		private static void DisplayCounter()
		{
			Screen.SetCursor(0, 5);
			Screen.Color = 0x09;
			Screen.Write(counter++);
		}

	}
}
