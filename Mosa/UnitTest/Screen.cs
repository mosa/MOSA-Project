/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

namespace Mosa.UnitTest
{
	/// <summary>
	/// 
	/// </summary>
	public static class Screen
	{
		/// <summary>
		/// 
		/// </summary>
		public static int Column = 0;
		/// <summary>
		/// 
		/// </summary>
		public static int Row = 0;
		/// <summary>
		/// 
		/// </summary>
		public static byte Color = 0x0A;

		/// <summary>
		/// 
		/// </summary>
		public const int Columns = 80;

		/// <summary>
		/// 
		/// </summary>
		public const int Rows = 40;

		/// <summary>
		/// Gets the address.
		/// </summary>
		/// <returns></returns>
		private unsafe static byte* GetAddress()
		{
			return (byte*)(0xB8000 + ((Row * Columns + Column) * 2));
		}

		/// <summary>
		/// Nexts 
		/// </summary>
		private static void Next()
		{
			Column++;

			if (Column >= Columns) {
				Column = 0;
				Row++;
			}
		}

		/// <summary>
		/// Writes the character.
		/// </summary>
		/// <param name="chr">The character.</param>
		public unsafe static void Write(char chr)
		{
			byte* address = GetAddress();

			*address = (byte)chr;
			*(address + 1) = Color;

			Next();
		}

		/// <summary>
		/// Goto the top.
		/// </summary>
		public static void GotoTop()
		{
			Column = 0;
			Row = 0;
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public static void Clear()
		{
			GotoTop();

			byte c = Color;
			Color = 0x0A;

			for (int i = 0; i < Columns * Rows; i++)
				Write(' ');

			Color = c;
			GotoTop();
		}

	}
}
