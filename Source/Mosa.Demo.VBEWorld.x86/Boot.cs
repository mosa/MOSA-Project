// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Demo.VBEWorld.x86.Apps;
using Mosa.Demo.VBEWorld.x86.Components;
using Mosa.Demo.VBEWorld.x86.HAL;
using Mosa.DeviceDriver;
using Mosa.DeviceDriver.ISA;
using Mosa.DeviceSystem;
using Mosa.FileSystem.FAT;
using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Mosa.Demo.VBEWorld.x86
{
	/// <summary>
	/// Boot
	/// </summary>
	public static class Boot
	{
		public static DeviceService DeviceService;
		public static ServiceManager ServiceManager;

		public static StandardKeyboard Keyboard;

		public static Taskbar Taskbar;
		public static Random Random;

		private static Hardware hal;

		private static bool hasFS;

		private static Image wallpaper;

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

			Console.Clear();

			Serial.SetupPort(Serial.COM1);
			IDT.SetInterruptHandler(ProcessInterrupt);

			hal = new Hardware();
			Random = new Random();

			// Create Service manager and basic services
			ServiceManager = new ServiceManager();
			DeviceService = new DeviceService();

			var diskDeviceService = new DiskDeviceService();
			var partitionService = new PartitionService();
			var pciControllerService = new PCIControllerService();
			var pciDeviceService = new PCIDeviceService();

			ServiceManager.AddService(DeviceService);
			ServiceManager.AddService(diskDeviceService);
			ServiceManager.AddService(partitionService);
			ServiceManager.AddService(pciControllerService);
			ServiceManager.AddService(pciDeviceService);

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

						wallpaper = Bitmap.CreateImage(_wall);
					}
				}
			}

			Keyboard = DeviceService.GetFirstDevice<StandardKeyboard>().DeviceDriver as StandardKeyboard;
			if (Keyboard == null)
				hal.Abort("Catastrophic failure, keyboard not found.");

			Utils.Mouse = DeviceService.GetFirstDevice<StandardMouse>().DeviceDriver as StandardMouse;
			if (Utils.Mouse == null)
				hal.Abort("Catastrophic failure, mouse not found.");

			if (Display.Initialize())
			{
				Log("Graphics setup OK!");
				Log("<SELFTEST:PASSED>");

				DoGraphics();
			}
			else Log("Graphics setup ERROR!");
		}

		private static void DoGraphics()
		{
			Utils.Mouse.SetScreenResolution(Display.Width, Display.Height);

			Mouse.Initialize();
			WindowManager.Initialize();

			Taskbar = new Taskbar();
			Taskbar.Buttons.Add(new TaskbarButton(Taskbar, "Shutdown", Color.Blue, Color.White, Color.Navy,
				() => { (ServiceManager.GetFirstService<PCService>() as PCService).Shutdown(); return null; }));
			Taskbar.Buttons.Add(new TaskbarButton(Taskbar, "Paint", Color.Blue, Color.White, Color.Navy,
				() => { WindowManager.Open(new Paint(70, 90, 400, 200, Color.MediumPurple, Color.Purple, Color.White)); return null; }));

			List<Label> labels;

			for (; ; )
			{
				if (hasFS)
					Display.DrawImage(0, 0, wallpaper, false);
				else
					Display.Clear(Color.Gray);

				// Draw MOSA logo
				Display.DrawMosaLogo(10);

				// Initialize background labels
				labels = new List<Label>
					{
						new Label("Current resolution is " + Display.Width + "x" + Display.Height, Display.DefaultFont.Name, 10, 10, Color.OrangeRed),
						new Label("FPS is " + FPSMeter.FPS, Display.DefaultFont.Name, 10, 26, Color.Lime)
					};

				// Draw all labels
				foreach (Label label in labels)
					label.Draw();

				// Draw and update all windows
				WindowManager.Update();

				// Draw taskbar on top of everything else (except cursor) and update it
				Taskbar.Draw();
				Taskbar.Update();

				// Draw cursor
				Mouse.Draw();

				// Update graphics (necessary if double buffering) and FPS meter
				Display.Update();
				FPSMeter.Update();
			}
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

		public static Func<object> BuildMethod<T>(Func<T> func)
		{
			return () => func();
		}

		public static Color Invert(Color color)
		{
			return Color.FromArgb(color.A, (byte)(255 - color.R), (byte)(255 - color.G), (byte)(255 - color.B));
		}
	}
}
