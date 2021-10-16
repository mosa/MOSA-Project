// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Demo.VBEWorld.x86.HAL;
using Mosa.DeviceDriver;
using Mosa.DeviceDriver.ISA;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;
using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;
using Mosa.FileSystem.FAT;
using System.Drawing;

namespace Mosa.Demo.VBEWorld.x86
{
	/// <summary>
	/// Boot
	/// </summary>
	public static class Boot
	{
		public static ConsoleSession Console;
		public static DeviceService DeviceService;

		public static ACPI ACPI;

		private static Hardware _hal;
		private static StandardMouse _mouse;

		private static bool _hasFS;

		private static Image _wallpaper;

		private static uint _black, _gray;

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

			Serial.SetupPort(Serial.COM1);
			IDT.SetInterruptHandler(ProcessInterrupt);
			
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

			var acpi = DeviceService.GetDevices("ACPI");
			if (acpi.Count == 0)
				Log("No ACPI!");
			else ACPI = acpi[0].DeviceDriver as ACPI;

			partitionService.CreatePartitionDevices();
			var partitions = DeviceService.GetDevices<IPartitionDevice>();

			foreach (var partition in partitions)
			{
				var fat = new FatFileSystem(partition.DeviceDriver as IPartitionDevice);
				_hasFS = fat.IsValid;

				if (_hasFS)
				{
					var location = fat.FindEntry("WALLP.BMP");

					if (location.IsValid)
					{
						var fatFileStream = new FatFileStream(fat, location);
						var _wall = new byte[(uint)fatFileStream.Length];

						for (int k = 0; k < _wall.Length; k++)
							_wall[k] = (byte)(char)fatFileStream.ReadByte();

						_wallpaper = new Bitmap(_wall);
					}
				}
			}

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
			_black = (uint)Color.Black.ToArgb();
			_gray = (uint)Color.Gray.ToArgb();

			for (; ; )
			{
				if (_hasFS)
					VBEDisplay.Framebuffer.DrawImage(_wallpaper, 0, 0, false);
				else
					VBEDisplay.Framebuffer.ClearScreen(_gray);

				MosaLogo.Draw(VBEDisplay.Framebuffer, 10);
				DrawMouse();

				VBEDisplay.Framebuffer.SwapBuffers();
			}
		}

		private static void DrawMouse()
		{
			VBEDisplay.Framebuffer.SetPixel(_black, (uint)_mouse.X, (uint)_mouse.Y);
			VBEDisplay.Framebuffer.SetPixel(_black, (uint)_mouse.X + 1, (uint)_mouse.Y);
			VBEDisplay.Framebuffer.SetPixel(_black, (uint)_mouse.X, (uint)_mouse.Y + 1);
			VBEDisplay.Framebuffer.SetPixel(_black, (uint)_mouse.X + 1, (uint)_mouse.Y + 1);
		}

		public static void Log(string line)
		{
			Serial.Write(Serial.COM1, line + "\r\n");
			Console.WriteLine(line);
		}

		public static void ProcessInterrupt(uint interrupt, uint errorCode)
		{
			if (interrupt >= 0x20 && interrupt < 0x30)
				Mosa.DeviceSystem.HAL.ProcessInterrupt((byte)(interrupt - 0x20));
		}
	}
}
