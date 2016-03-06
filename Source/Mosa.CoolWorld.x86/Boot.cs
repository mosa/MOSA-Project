// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.AppSystem;
using Mosa.DeviceDriver.ScanCodeMap;
using Mosa.Kernel.x86;
using Mosa.Runtime.x86;

namespace Mosa.CoolWorld.x86
{
	/// <summary>
	///
	/// </summary>
	public static class Boot
	{
		public static ConsoleSession Console;
		public static ConsoleSession Debug;

		/// <summary>
		/// Main
		/// </summary>
		unsafe public static void Main()
		{
			Kernel.x86.Kernel.Setup();

			Console = ConsoleManager.Controller.Boot;
			Debug = ConsoleManager.Controller.Boot;

			Console.Clear();

			Console.ScrollRow = 23;

			IDT.SetInterruptHandler(ProcessInterrupt);

			Console.Color = Colors.White;
			Console.BackgroundColor = Colors.Green;

			Console.Write(@"                   MOSA OS Version 1.4 - Compiler Version 1.4");
			FillLine();
			Console.Color = Colors.White;
			Console.BackgroundColor = Colors.Black;

			Console.WriteLine("> Initializing hardware abstraction layer...");
			Setup.Initialize();

			Console.WriteLine("> Adding hardware devices...");
			Setup.Start();

			Console.Color = Colors.White;
			Console.WriteLine();

			Debug = ConsoleManager.Controller.Debug;

			// setup keymap
			var keymap = new US();

			// setup keyboard (state machine)
			var keyboard = new Mosa.DeviceSystem.Keyboard(Setup.StandardKeyboard, keymap);

			// setup app manager
			var manager = new AppManager(Console, keyboard);

			IDT.SetInterruptHandler(manager.ProcessInterrupt);

			manager.Start();
		}

		public static void WaitForKey()
		{
			// wait for key press

			while (true)
			{
				byte scancode = Setup.StandardKeyboard.GetScanCode();

				if (scancode != 0)
				{
					break;
				}
				Native.Hlt();
			}
		}

		public static void ForeverLoop()
		{
			while (true)
			{
				Native.Hlt();
			}
		}

		public static void FillLine()
		{
			for (uint c = 80 - Console.Column; c != 0; c--)
			{
				Console.Write(' ');
			}
		}

		public static void InBrackets(string message, byte outerColor, byte innerColor)
		{
			Console.Color = outerColor;
			Console.Write("[");
			Console.Color = innerColor;
			Console.Write(message);
			Console.Color = outerColor;
			Console.Write("]");
		}

		private static uint tick = 0;

		public static void ProcessInterrupt(uint interrupt, uint errorCode)
		{
			uint c = Console.Column;
			uint r = Console.Row;
			byte col = Console.Color;
			byte back = Console.BackgroundColor;
			uint sr = Console.ScrollRow;

			Console.Color = Colors.Cyan;
			Console.BackgroundColor = Colors.Black;
			Console.Row = 24;
			Console.Column = 0;
			Console.ScrollRow = Console.Rows;

			tick++;
			Console.Write("Booting - ");
			Console.Write("Tick: ");
			Console.Write(tick, 10, 7);
			Console.Write(" Free Memory: ");
			Console.Write((PageFrameAllocator.TotalPages - PageFrameAllocator.TotalPagesInUse) * PageFrameAllocator.PageSize / (1024 * 1024));
			Console.Write(" MB         ");

			if (interrupt >= 0x20 && interrupt < 0x30)
			{
				Mosa.HardwareSystem.HAL.ProcessInterrupt((byte)(interrupt - 0x20));
			}

			Console.Column = c;
			Console.Row = r;
			Console.Color = col;
			Console.BackgroundColor = back;
			Console.ScrollRow = sr;
		}
	}
}
