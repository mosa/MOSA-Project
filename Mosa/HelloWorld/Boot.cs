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
		/// 
		/// </summary>
		private static uint counter = 0;

		/// <summary>
		/// Mains this instance.
		/// </summary>
		public static void Main()
		{
			Screen.Clear();

			Screen.Color = 0x0A;
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
			Screen.Color = 0x0A;
			Screen.Write('\'');
			Screen.NextLine();

			Screen.Color = 0x07;
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.Write('-');
			Screen.NextLine();

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
			Screen.Write('.');

			while (true) {
				Screen.Row = 0;
				Screen.Column = 27;
				Screen.Color = 0x0A;
				Screen.Write('-');
				Screen.Column = 27;
				Screen.Color = 0x0B;
				Screen.Write('\\');
				Screen.Column = 27;
				Screen.Color = 0x0C;
				Screen.Write('|');
				Screen.Column = 27;
				Screen.Color = 0x09;
				Screen.Write('/');
				DisplayCounter();
			}
		}

		/// <summary>
		/// Displays the counter.
		/// </summary>
		private static void DisplayCounter()
		{
			Screen.Row = 5;
			Screen.Column = 0;
			Screen.Color = 0x09;
			Screen.Write(counter++);
		}

	}
}
