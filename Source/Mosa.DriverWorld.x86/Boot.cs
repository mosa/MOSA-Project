/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

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
		public static TextScreen Console = null;

		/// <summary>
		/// Main
		/// </summary>
		public static void Main()
		{
			DebugClient.Setup(Serial.COM1);
			IDT.SetInterruptHandler(null);
			SSE.Setup();
			Multiboot.Setup();
			PIC.Setup();
			GDT.Setup();
			IDT.Setup();
			PageFrameAllocator.Setup();
			PageTable.Setup();
			VirtualPageAllocator.Setup();
			ProcessManager.Setup();
			Runtime.Setup();
			TaskManager.Setup();
			SmbiosManager.Setup();
			IDT.SetInterruptHandler(ProcessInterrupt);
			Screen.RawWrite(3, 0, 'A', 0x0A);
			Setup.Initialize();
			Screen.RawWrite(4, 0, 'A', 0x0A);
			Setup.Start();
			Screen.RawWrite(5, 0, 'A', 0x0A);
			var textDevice = (ITextDevice)Setup.DeviceManager.GetDevices(new FindDevice.WithName("VGAText")).First.Value;
			Console = new TextScreen(textDevice);
			Screen.RawWrite(6, 0, 'A', 0x0A);
			Console.ClearScreen();

			Console.Write("          Copyright (C) 2008-2015 [Managed Operating System Alliance]");
			Console.WriteLine();
			Console.WriteLine("> System ready");
			Console.WriteLine();
			Screen.RawWrite(7, 0, 'A', 0x0A);
			Process();
		}

		public static void Process()
		{
			int lastSecond = -1;

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
			InBrackets("Done");
			Console.WriteLine();
		}

		public static void BulletPoint()
		{
			Console.Write("  * ");
		}

		public static void InBrackets(string message)
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
				Console.WriteLine("1");
				Mosa.DeviceSystem.HAL.ProcessInterrupt((byte)(interrupt - 0x20));

				//Debug.Trace("Returned from HAL.ProcessInterrupt");
			}
		}
	}
}
