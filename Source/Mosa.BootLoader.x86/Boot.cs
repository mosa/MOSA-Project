// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;

namespace Mosa.BootLoader.x86
{
	/// <summary>
	///
	/// </summary>
	public static class Boot
	{
		[Method("Mosa.Runtime.StartUp.SetInitialMemory")]
		public static void SetInitialMemory()
		{
			KernelMemory.SetInitialMemory(Address.GCInitialMemory_BootLoader, 0x01000000);
		}

		private static uint counter = 0;

		/// <summary>
		/// Main
		/// </summary>
		public static void Main()
		{
			IDT.SetInterruptHandler(null);
			Panic.Setup();
			Debugger.Setup(Serial.COM1);

			// Initialize interrupts
			PIC.Setup();
			IDT.Setup();
			GDT.Setup();

			Runtime.Internal.Setup();

			IDT.SetInterruptHandler(ProcessInterrupt);

			EnterTestReadyLoop();
		}

		public static void EnterTestReadyLoop()
		{
			Screen.Color = 0x0;
			Screen.Clear();
			Screen.GotoTop();
			Screen.Color = 0x0E;
			Screen.Write("MOSA OS Version 1.6 - Bootloader");
			Screen.NextLine();
			Screen.NextLine();

			UnitTestQueue.Setup();
			UnitTestRunner.Setup();

			UnitTestRunner.EnterTestReadyLoop();
		}

		public static void ProcessInterrupt(uint interrupt, uint errorCode)
		{
			counter++;

			uint c = Screen.Column;
			uint r = Screen.Row;
			byte col = Screen.Color;
			byte back = Screen.BackgroundColor;

			Screen.Column = 60;
			Screen.Row = 24;
			Screen.Color = Colors.Cyan;
			Screen.BackgroundColor = Colors.Black;

			Screen.Write(counter, 10, 7);
			Screen.Write(':');
			Screen.Write(interrupt, 16, 2);
			Screen.Write(':');
			Screen.Write(errorCode, 16, 2);

			Screen.Column = c;
			Screen.Row = r;
			Screen.Color = col;
			Screen.BackgroundColor = back;
		}
	}
}
