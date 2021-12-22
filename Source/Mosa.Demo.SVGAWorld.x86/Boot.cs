// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Demo.VBEWorld.x86.Apps;
using Mosa.Demo.VBEWorld.x86.Components;
using Mosa.Demo.VBEWorld.x86.HAL;
using Mosa.DeviceDriver;
using Mosa.DeviceDriver.ISA;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.Service;
using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;
using System;
using System.Collections.Generic;
using System.Drawing;
using Mosa.DeviceDriver.ScanCodeMap;
using Mosa.FileSystem.FAT;

namespace Mosa.Demo.VBEWorld.x86
{
	/// <summary>
	/// Boot
	/// </summary>
	public static class Boot
	{
		public static DeviceService DeviceService;

		public static DeviceSystem.Keyboard Keyboard;

		public static Taskbar Taskbar;
		public static Random Random;

		private static Hardware HAL;

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

			Serial.SetupPort(Serial.COM1);
			IDT.SetInterruptHandler(ProcessInterrupt);

			HAL = new Hardware();
			Random = new Random();
			DeviceService = new DeviceService();

			// Create service manager and basic services
			var serviceManager = new ServiceManager();
			var diskDeviceService = new DiskDeviceService();
            var partitionService = new PartitionService();

			serviceManager.AddService(DeviceService);
			serviceManager.AddService(diskDeviceService);
			serviceManager.AddService(partitionService);
			serviceManager.AddService(new PCIControllerService());
			serviceManager.AddService(new PCIDeviceService());

			DeviceSystem.Setup.Initialize(HAL, DeviceService.ProcessInterrupt);

			DeviceService.RegisterDeviceDriver(DeviceDriver.Setup.GetDeviceDriverRegistryEntries());
			DeviceService.Initialize(new X86System(), null);

			PowerService.Initialize();

			partitionService.CreatePartitionDevices();

			foreach (var partition in DeviceService.GetDevices<IPartitionDevice>())
			{
				var fat = new FatFileSystem(partition.DeviceDriver as IPartitionDevice);

				if (!fat.IsValid)
					break;

				var serif = fat.FindEntry("FONT.BIN");
				var script = fat.FindEntry("FONT2.BIN");

				if (!serif.IsValid || !script.IsValid)
					HAL.Abort("Could not find required fonts.");

				var stream = new FatFileStream(fat, serif);
				var font = new byte[stream.Length];

				stream.Read(font, 0, font.Length);

				Display.DefaultFont = Utils.Load(font);

				var stream2 = new FatFileStream(fat, script);
				var font2 = new byte[stream2.Length];

				stream2.Read(font2, 0, font2.Length);

				Utils.Fonts = new List<ISimpleFont> { Display.DefaultFont, Utils.Load(font2) };
			}

			if (!Display.Initialize())
			{
				Log("An error occured when initializing the graphics driver.");
				for (;;) ;
			}

			var keyboard = DeviceService.GetFirstDevice<StandardKeyboard>().DeviceDriver as StandardKeyboard;
			if (keyboard == null)
				HAL.Abort("Catastrophic failure, keyboard not found.");

			// Setup keyboard (state machine)
			Keyboard = new DeviceSystem.Keyboard(keyboard, new US());

			Utils.Mouse = DeviceService.GetFirstDevice<StandardMouse>().DeviceDriver as StandardMouse;
			if (Utils.Mouse == null)
				HAL.Abort("Catastrophic failure, mouse not found.");

			Log("<SELFTEST:PASSED>");
			DoGraphics();
		}

		private static void DoGraphics()
		{
			Utils.BackColor = Color.Indigo;
			Utils.Mouse.SetScreenResolution(Display.Width, Display.Height);

			Mouse.Initialize();
			WindowManager.Initialize();

			Taskbar = new Taskbar();
			Taskbar.Buttons.Add(new TaskbarButton(Taskbar, "Shutdown", Color.Blue, Color.White, Color.Navy,
				() => { PowerService.Shutdown(); return null; }));
			Taskbar.Buttons.Add(new TaskbarButton(Taskbar, "Reset", Color.Blue, Color.White, Color.Navy,
				() => { PowerService.Reset(); return null; }));
			Taskbar.Buttons.Add(new TaskbarButton(Taskbar, "Paint", Color.Coral, Color.White, Color.Red,
				() => { WindowManager.Open(new Paint(70, 90, 400, 200, Color.MediumPurple, Color.Purple, Color.White)); return null; }));
			Taskbar.Buttons.Add(new TaskbarButton(Taskbar, "Notepad", Color.Coral, Color.White, Color.Red,
				() => { WindowManager.Open(new Notepad(70, 90, 400, 200, Color.MediumPurple, Color.Purple, Color.White)); return null; }));
			Taskbar.Buttons.Add(new TaskbarButton(Taskbar, "Settings", Color.Coral, Color.White, Color.Red,
				() => { WindowManager.Open(new Settings(70, 90, 400, 200, Color.MediumPurple, Color.Purple, Color.White)); return null; }));

			List<Label> labels;

			for (;;)
			{
				// Clear screen
				Display.Clear(Utils.BackColor);

				// Draw MOSA logo
				Display.DrawMosaLogo(10);

				// Initialize background labels
				labels = new List<Label>
				{
					new Label("Current resolution is " + Display.Width + "x" + Display.Height, Display.DefaultFont, 10, 10, Color.OrangeRed),
					new Label("FPS is " + FPSMeter.FPS, Display.DefaultFont, 10, 26, Color.Lime),
					new Label("Current driver is " + Display.CurrentDriver, Display.DefaultFont, 10, 42, Color.MidnightBlue),
					new Label("Current font is " + Display.DefaultFont.Name, Display.DefaultFont, 10, 58, Color.LightSeaGreen)
				};

				// Draw all labels
				foreach (var label in labels)
					label.Draw();

				// Draw and update all windows
				WindowManager.Update();

				// Draw taskbar on top of everything else (except cursor) and update it
				Taskbar.Draw();
				Taskbar.Update();

				// Draw cursor
				Mouse.Draw();

				// Update graphics and FPS meter
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
	}
}
