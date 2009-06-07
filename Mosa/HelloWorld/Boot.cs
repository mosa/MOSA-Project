/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

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
			unsafe {

				// Clear the screen
				for (byte* i = (byte*)0xB8000; i < (byte*)0xB8FA0; i = i + 2)
					WriteChar(' ', 0x0A, i);

				byte* index = (byte*)(0xB8000);
				WriteChar('M', 0x0A, index); index = index + 2;
				WriteChar('O', 0x0A, index); index = index + 2;
				WriteChar('S', 0x0A, index); index = index + 2;
				WriteChar('A', 0x0A, index); index = index + 2;
				WriteChar(' ', 0x0A, index); index = index + 2;
				WriteChar('O', 0x0A, index); index = index + 2;
				WriteChar('S', 0x0A, index); index = index + 2;
				WriteChar(' ', 0x0A, index); index = index + 2;
				WriteChar('V', 0x0A, index); index = index + 2;
				WriteChar('e', 0x0A, index); index = index + 2;
				WriteChar('r', 0x0A, index); index = index + 2;
				WriteChar('s', 0x0A, index); index = index + 2;
				WriteChar('i', 0x0A, index); index = index + 2;
				WriteChar('o', 0x0A, index); index = index + 2;
				WriteChar('n', 0x0A, index); index = index + 2;
				WriteChar(' ', 0x0A, index); index = index + 2;
				WriteChar('0', 0x0A, index); index = index + 2;
				WriteChar('.', 0x0A, index); index = index + 2;
				WriteChar('1', 0x0A, index); index = index + 2;
				WriteChar(' ', 0x0A, index); index = index + 2;
				WriteChar('\'', 0x0A, index); index = index + 2;
				WriteChar('W', 0x0C, index); index = index + 2;
				WriteChar('a', 0x0C, index); index = index + 2;
				WriteChar('k', 0x0C, index); index = index + 2;
				WriteChar('e', 0x0C, index); index = index + 2;
				WriteChar('\'', 0x0A, index); index = index + 2;
				WriteChar(' ', 0x0A, index);

				index = (byte*)(0xB80A0);
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index); index = index + 2;
				WriteChar('-', 0x07, index);

				index = (byte*)(0xB8140);
				WriteChar('C', 0x0E, index); index = index + 2;
				WriteChar('o', 0x0E, index); index = index + 2;
				WriteChar('p', 0x0E, index); index = index + 2;
				WriteChar('y', 0x0E, index); index = index + 2;
				WriteChar('r', 0x0E, index); index = index + 2;
				WriteChar('i', 0x0E, index); index = index + 2;
				WriteChar('g', 0x0E, index); index = index + 2;
				WriteChar('h', 0x0E, index); index = index + 2;
				WriteChar('t', 0x0E, index); index = index + 2;
				WriteChar(' ', 0x0E, index); index = index + 2;
				WriteChar('2', 0x0A, index); index = index + 2;
				WriteChar('0', 0x0A, index); index = index + 2;
				WriteChar('0', 0x0A, index); index = index + 2;
				WriteChar('8', 0x0A, index); index = index + 2;
				WriteChar('-', 0x0A, index); index = index + 2;
				WriteChar('2', 0x0A, index); index = index + 2;
				WriteChar('0', 0x0A, index); index = index + 2;
				WriteChar('0', 0x0A, index); index = index + 2;
				WriteChar('9', 0x0A, index); index = index + 2;
				WriteChar('.', 0x0A, index);

				uint cnt = 0;

				index = (byte*)(0xB8036);
				while (true) {
					WriteChar('-', 0x0A, index);
					WriteChar('\\', 0x0B, index);
					WriteChar('|', 0x0C, index);
					WriteChar('/', 0x09, index);
					Write(cnt++, 0x09, (byte*)(0xB8140 + 0x140));
				}
			}
		}

		/// <summary>
		/// Writes the character.
		/// </summary>
		/// <param name="chr">The character.</param>
		/// <param name="color">The color.</param>
		/// <param name="address">The address.</param>
		private unsafe static void WriteChar(char chr, byte color, byte* address)
		{
			*address = (byte)chr;
			*(address + 1) = color;
		}

		/// <summary>
		/// Writes the specified value.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <param name="color">The color.</param>
		/// <param name="address">The address.</param>
		private unsafe static void Write(uint val, byte color, byte* address)
		{
			int count = 0;
			uint temp = val;

			do {
				temp /= 10;
				count++;
			}
			while (temp != 0);

			for (int i = 0; i < count; i++) {
				WriteChar((char)('0' + (val % 10)), color, address + (2 * (count - 1 - i)));
				val /= 10;
			}
		}
	}
}
