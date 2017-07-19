// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.AppSystem;
using Mosa.DeviceDriver.ScanCodeMap;
using Mosa.HardwareSystem;
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

			Console.Color = Color.White;
			Console.BackgroundColor = Color.Green;

			Console.Write(@"                   MOSA OS Version 1.4 - Compiler Version 1.4");
			FillLine();
			Console.Color = Color.White;
			Console.BackgroundColor = Color.Black;

			// Setup hardware abstraction interface
			Console.WriteLine("> Initializing hardware abstraction layer...");

			var hardware = new HAL.Hardware();
			HardwareSystem.Setup.Initialize(hardware);

			Console.WriteLine("> Adding hardware devices...");
			HardwareSystem.Setup.Start();

			Console.Color = Color.White;
			Console.WriteLine();

			// Get StandardKeyboard
			var standardKeyboards = HardwareSystem.Setup.DeviceManager.GetDevices(new WithName("StandardKeyboard"));

			if (standardKeyboards == null)
			{
				Console.WriteLine("No Keyboards");
				ForeverLoop();
			}

			if (standardKeyboards.Count == 0)
			{
				Console.WriteLine("No Keyboards");
				ForeverLoop();
			}

			var standardKeyboard = standardKeyboards[0] as DeviceSystem.IKeyboardDevice;

			Debug = ConsoleManager.Controller.Debug;

			// setup keymap
			var keymap = new US();

			// setup keyboard (state machine)
			var keyboard = new Mosa.DeviceSystem.Keyboard(standardKeyboard, keymap);

			// setup app manager
			var manager = new AppManager(Console, keyboard);

			IDT.SetInterruptHandler(manager.ProcessInterrupt);

			manager.Start();
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

			Console.Color = Color.Cyan;
			Console.BackgroundColor = Color.Black;
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
