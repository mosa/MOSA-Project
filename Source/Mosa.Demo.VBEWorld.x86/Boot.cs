// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Demo.VBEWorld.x86.HAL;
using Mosa.DeviceDriver;
using Mosa.DeviceDriver.ISA;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;
using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;

namespace Mosa.Demo.VBEWorld.x86
{
	/// <summary>
	/// Boot
	/// </summary>
	public static class Boot
	{
		public static ConsoleSession Console;
		public static DeviceService DeviceService;

		private static Hardware _hal;
		private static StandardMouse _mouse;

		[Plug("Mosa.Runtime.StartUp::SetInitialMemory")]
		public static void SetInitialMemory()
		{
			KernelMemory.SetInitialMemory(Address.GCInitialMemory, 0x01000000);
		}

		/// <summary>
		/// Main
		/// </summary>
		public static void Main()
		{
			Kernel.x86.Kernel.Setup();

			Console = ConsoleManager.Controller.Boot;
			Console.Clear();

			IDT.SetInterruptHandler(ProcessInterrupt);

			Serial.SetupPort(Serial.COM1);

			_hal = new Hardware();

			// Create Service manager and basic services
			var serviceManager = new ServiceManager();

			DeviceService = new DeviceService();

			var diskDeviceService = new DiskDeviceService();
			var partitionService = new PartitionService();
			var pciControllerService = new PCIControllerService();
			var pciDeviceService = new PCIDeviceService();

			serviceManager.AddService(DeviceService);
			serviceManager.AddService(diskDeviceService);
			serviceManager.AddService(partitionService);
			serviceManager.AddService(pciControllerService);
			serviceManager.AddService(pciDeviceService);

			DeviceSystem.Setup.Initialize(_hal, DeviceService.ProcessInterrupt);

			DeviceService.RegisterDeviceDriver(DeviceDriver.Setup.GetDeviceDriverRegistryEntries());
			DeviceService.Initialize(new X86System(), null);

			var standardMice = DeviceService.GetDevices("StandardMouse");
			if (standardMice.Count == 0)
			{
				_hal.Pause();
				_hal.Abort("Catastrophic failure, mouse and/or PIT not found.");
			}

			_mouse = standardMice[0].DeviceDriver as StandardMouse;
			_mouse.SetScreenResolution(VBE.ScreenWidth, VBE.ScreenHeight);

			if (VBEDisplay.InitVBE(_hal))
			{
				Log("VBE setup OK!");
				DoGraphics();
			} else Log("VBE setup ERROR!");
		}

		private static void DoGraphics()
		{
			for (; ; )
			{
				VBEDisplay.Framebuffer.ClearScreen(0x00555555);
				VBEDisplay.Framebuffer.FillRectangle(0x0, 50, 50, 130, 80);

				MosaLogo.Draw(VBEDisplay.Framebuffer, 10);
				DrawMouse();

				VBEDisplay.Framebuffer.SwapBuffers();
			}
		}

		private static void DrawMouse()
		{
			VBEDisplay.Framebuffer.SetPixel(0x0, (uint)_mouse.X, (uint)_mouse.Y);
			VBEDisplay.Framebuffer.SetPixel(0x0, (uint)_mouse.X + 1, (uint)_mouse.Y);
			VBEDisplay.Framebuffer.SetPixel(0x0, (uint)_mouse.X, (uint)_mouse.Y + 1);
			VBEDisplay.Framebuffer.SetPixel(0x0, (uint)_mouse.X + 1, (uint)_mouse.Y + 1);
		}

		public static void Log(string line)
		{
			Serial.Write(Serial.COM1, line + "\r\n");
			Console.WriteLine(line);
		}

		public static void ProcessInterrupt(uint interrupt, uint errorCode)
		{
			if (interrupt >= 0x20 && interrupt < 0x30)
				_hal.ProcessInterrupt((byte)(interrupt - 0x20));
		}
	}
}
