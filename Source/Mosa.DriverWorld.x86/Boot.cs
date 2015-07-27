/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System.Collections.Generic;
using Mosa.DeviceSystem;
using Mosa.Kernel.x86;
using Mosa.Kernel.x86.Smbios;
using Mosa.Platform.Internal.x86;

namespace Mosa.DriverWorld.x86
{
	/// <summary>
	///
	/// </summary>
	public static class Boot
	{
		public static TextScreen console;

		public static TextScreen Console
		{
			get { return console; }
		}

		/// <summary>
		/// Main
		/// </summary>
		public static void Main()
		{
			Mosa.Kernel.x86.Kernel.Setup();

			IDT.SetInterruptHandler(ProcessInterrupt);

			Setup.Initialize();
			Setup.Start();

			var textDevice = (ITextDevice)Setup.DeviceManager.GetDevices(new FindDevice.IsTypeOf<ITextDevice>()).First.Value;
			console = new TextScreen(textDevice);

			Console.ClearScreen();
			Console.SetCursor(0, 0);

			Console.SetColor(TextColor.Blue, TextColor.Green);
			Console.WriteLine("                   MOSA OS Version 1.5 - Compiler Version 1.5                   ");
			Console.SetColor(TextColor.Black, TextColor.White);
			Console.WriteLine("> System ready");
			Console.SetCursor(0, 24);
			Console.Write("          Copyright (C) 2008-2015 [Managed Operating System Alliance]           ");

			Process();
		}

		public static void Process()
		{
			int lastSecond = -1;

			Console.SetCursor(0, 3);
			Console.Write("> ");

			Mosa.DeviceDriver.ScanCodeMap.US KBDMAP = new DeviceDriver.ScanCodeMap.US();

			while (true)
			{
				byte scancode = Setup.Keyboard.GetScanCode();

				if (scancode != 0)
				{
					//	Debug.Trace("Main.Main Key Scan Code: " + scancode.ToString());

					KeyEvent keyevent = KBDMAP.ConvertScanCode(scancode);

					//	Debug.Trace("Main.Main Key Character: " + keyevent.Character.ToString());

					if (keyevent.KeyPress == KeyEvent.Press.Make)
					{
						if (keyevent.Character != 0)
						{
							Console.Write(keyevent.Character);
						}

						if (keyevent.KeyType == KeyType.F1)
							ConsoleManager.Controller.Active = ConsoleManager.Controller.Boot;
						else if (keyevent.KeyType == KeyType.F2)
							ConsoleManager.Controller.Active = ConsoleManager.Controller.Debug;
					}
					//	Debug.Trace("Main.Main Key Character: " + ((uint)keyevent.Character).ToString());
				}

				if (Setup.CMOS.Second != lastSecond)
				{
					//DebugClient.SendAlive();
					lastSecond = Setup.CMOS.Second;
					//Debug.Trace("Main.Main Ping Alive");
				}

				//DebugClient.Process();
				Native.Hlt();
			}
		}

		public static void PrintDone()
		{
			InBrackets("Done", TextColor.White, TextColor.LightGreen);
			Console.WriteLine();
		}

		public static void BulletPoint()
		{
			Console.Write("  * ");
		}

		public static void InBrackets(string message, TextColor outerColor, TextColor innerColor)
		{
			Console.Write("[");
			Console.Write(message);
			Console.Write("]");
		}

		public static void ProcessInterrupt(uint interrupt, uint errorCode)
		{
			if (interrupt == 0x20)
			{
				// Timer Interrupt! Switch Tasks!
			}
			else if (interrupt >= 0x20 && interrupt < 0x30)
			{
				Mosa.DeviceSystem.HAL.ProcessInterrupt((byte)(interrupt - 0x20));

				//Debug.Trace("Returned from HAL.ProcessInterrupt");
			}
		}
	}
}
