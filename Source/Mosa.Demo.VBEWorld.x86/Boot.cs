﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

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

		private static Hardware hal;
		private static StandardMouse mouse;

		private static bool hasFS;

		private static Image wallpaper;

		private static uint black, gray;

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
			
			hal = new Hardware();

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

			DeviceSystem.Setup.Initialize(hal, DeviceService.ProcessInterrupt);

			DeviceService.RegisterDeviceDriver(DeviceDriver.Setup.GetDeviceDriverRegistryEntries());
			DeviceService.Initialize(new X86System(), null);

			partitionService.CreatePartitionDevices();
			var partitions = DeviceService.GetDevices<IPartitionDevice>();

			foreach (var partition in partitions)
			{
				var fat = new FatFileSystem(partition.DeviceDriver as IPartitionDevice);
				hasFS = fat.IsValid;

				if (hasFS)
				{
					var location = fat.FindEntry("WALLP.BMP");

					if (location.IsValid)
					{
						var fatFileStream = new FatFileStream(fat, location);
						var _wall = new byte[(uint)fatFileStream.Length];

						for (int k = 0; k < _wall.Length; k++)
							_wall[k] = (byte)(char)fatFileStream.ReadByte();

						wallpaper = new Bitmap(_wall);
					}
				}
			}

			var standardMice = DeviceService.GetDevices("StandardMouse");
			if (standardMice.Count == 0)
			{
				hal.Pause();
				hal.Abort("Catastrophic failure, mouse and/or PIT not found.");
			}

			mouse = standardMice[0].DeviceDriver as StandardMouse;
			mouse.SetScreenResolution(VBE.ScreenWidth, VBE.ScreenHeight);

			if (VBEDisplay.InitVBE(hal))
			{
				Log("VBE setup OK!");
				DoGraphics();
			} else Log("VBE setup ERROR!");
		}

		private static void DoGraphics()
		{
			black = (uint)Color.Black.ToArgb();
			gray = (uint)Color.Gray.ToArgb();

			for (; ; )
			{
				if (hasFS)
					VBEDisplay.Framebuffer.DrawImage(wallpaper, 0, 0, false);
				else
					VBEDisplay.Framebuffer.ClearScreen(gray);

				MosaLogo.Draw(VBEDisplay.Framebuffer, 10);
				DrawMouse();

				VBEDisplay.Framebuffer.SwapBuffers();
			}
		}

		private static void DrawMouse()
		{
			VBEDisplay.Framebuffer.SetPixel(black, (uint)mouse.X, (uint)mouse.Y);
			VBEDisplay.Framebuffer.SetPixel(black, (uint)mouse.X + 1, (uint)mouse.Y);
			VBEDisplay.Framebuffer.SetPixel(black, (uint)mouse.X, (uint)mouse.Y + 1);
			VBEDisplay.Framebuffer.SetPixel(black, (uint)mouse.X + 1, (uint)mouse.Y + 1);
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
