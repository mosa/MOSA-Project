// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.Kernel.x86;
using Mosa.Runtime.x86;

namespace Mosa.VBEWorld.x86
{
	/// <summary>
	/// 
	/// </summary>
	public static class Boot
	{
		public static ConsoleSession Console;

		public static VBEFrameBuffer32bpp _framebuffer;

		/// <summary>
		/// Main
		/// </summary>
		public static void Main()
		{
			Kernel.x86.Kernel.Setup();

			Console = ConsoleManager.Controller.Boot;
			Console.Clear();

			IDT.SetInterruptHandler(ProcessInterrupt);

			Serial.SetupPort(Serial.COM1); //For debugging

			if (SetupVBE())
			{
				Log("VBE setup OK!");

				DoGraphics();
			}
			else
			{
				Log("VBE setup ERROR!");
			}

			ForeverLoop();
		}

		private static void DoGraphics()
		{
			_framebuffer.FillRectangle(0x00555555, 0, 0, _framebuffer.Width, _framebuffer.Height);

			MosaLogo.Draw(_framebuffer, 10);
		}

		private static bool SetupVBE()
		{
			if (!Multiboot.IsMultibootEnabled)
			{
				Log("Multiboot not enabled.");
				return false;
			}

			if (!Multiboot.VBEPresent)
			{
				Log("VBE is not enabled.");
				return false;
			}

			VBEMode vbeInfo = Multiboot.VBEModeInfoStructure;
			Log("VBE mode: " + vbeInfo.ScreenWidth.ToString() + "x" + vbeInfo.ScreenHeight.ToString() + "x" + vbeInfo.BitsPerPixel);

			_framebuffer = new VBEFrameBuffer32bpp(vbeInfo);

			return true;
		}

		private static void Log(string line)
		{
			Serial.Write(Serial.COM1, line + "\r\n");
			Console.WriteLine(line);
		}

		private static void ForeverLoop()
		{
			while (true)
			{
				Native.Hlt();
			}
		}

		public static void ProcessInterrupt(uint interrupt, uint errorCode)
		{
		}
	}
}