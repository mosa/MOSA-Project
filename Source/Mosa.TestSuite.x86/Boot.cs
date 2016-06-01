// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;
using Mosa.Runtime.x86;

namespace Mosa.TestSuite.x86
{
	/// <summary>
	///
	/// </summary>
	public static class Boot
	{
		private static uint counter = 0;

		/// <summary>
		/// Main
		/// </summary>
		public static void Main()
		{
			Kernel.x86.Kernel.Setup();

			IDT.SetInterruptHandler(ProcessInterrupt);

			EnterUnitTestMode();
		}

		public static void EnterUnitTestMode()
		{
			Screen.Color = 0x0;
			Screen.Clear();
			Screen.GotoTop();
			Screen.Color = 0x0E;
			Screen.Write("MOSA OS Version 1.4 - Unit Test Mode Kernel");
			Screen.NextLine();
			Screen.NextLine();

			UnitTestRunner.Setup();

			DebugClient.Setup(Serial.COM1);

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

		private static void ForceTestCollection()
		{
			Mosa.Test.Collection.OptimizationTest.OptimizationTest1();
		}
	}
}
