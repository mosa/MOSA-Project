// Copyright (c) MOSA Project. Licensed under the New BSD License.

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

		public ConsoleSession Active { get { return active; } set { Switch(value); } }

		public static void Setup()
		{
			Controller = new ConsoleManager();
		}

		public ConsoleManager()
		{
			Boot = CreateSeason();
			Debug = CreateSeason();
			active = Boot;
		}

		public ConsoleSession CreateSeason()
		{
			return new ConsoleSession(this);
		}

		public void Switch(ConsoleSession console)
		{
			if (console == active)
				return;

			for (byte row = 0; row < 40; row++)
			{
				for (byte column = 0; column < 80; column++)
				{
					char chr = console.GetText(column, row);
					byte color = console.GetColor(column, row);

					Screen.RawWrite(row, column, chr, color);
				}
			}

			Screen.Goto(console.Row, console.Column);
			UpdateCursor(console);

			active = console;
		}

		public void UpdateCursor(ConsoleSession console)
		{
			Screen.SetCursor(console.Row, console.Column);
		}

		public void RawWrite(ConsoleSession console, uint row, uint column, char chr, byte color)
		{
			if (console != active)
				return;

			Screen.RawWrite(row, column, chr, color);
		}
	}
}
