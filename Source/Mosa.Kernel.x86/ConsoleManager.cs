/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

namespace Mosa.Kernel.x86
{

	/// <summary>
	/// 
	/// </summary>
	public class ConsoleManager
	{
		public static ConsoleManager Controller;

		protected ConsoleSession active;

		public ConsoleSession Boot;
		public ConsoleSession Debug;

		public ConsoleSession Active { get { return active; } set { Change(value); } }

		public static void Setup()
		{
			Controller = new ConsoleManager();
		}

		public ConsoleManager()
		{
			Boot = new ConsoleSession(this);
			Debug = new ConsoleSession(this);
			active = Boot;
		}

		public void Change(ConsoleSession console)
		{
			if (console == active)
				return;

			for (byte row = 0; row < 40; row++)
			{
				for (byte column = 0; column < 80; column++)
				{
					char chr = console.GetText(column, row);
					byte color = console.GetColor(column, row);

					Mosa.Kernel.x86.Screen.RawWrite(row, column, chr, color);
				}
			}

			Mosa.Kernel.x86.Screen.Goto(console.Row, console.Column);

			active = console;
		}

		public void RawWrite(ConsoleSession console, uint row, uint column, char chr, byte color)
		{
			if (console != active)
				return;

			Mosa.Kernel.x86.Screen.RawWrite(row, column, chr, color);
		}

	}
}
