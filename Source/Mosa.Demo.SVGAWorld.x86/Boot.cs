// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System;
using System.Collections.Generic;
using System.Drawing;
using Mosa.Demo.SVGAWorld.x86.Apps;
using Mosa.Demo.SVGAWorld.x86.Components;
using Mosa.Demo.SVGAWorld.x86.HAL;
using Mosa.Demo.SVGAWorld.x86.Utils;
using Mosa.DeviceDriver;
using Mosa.DeviceDriver.ISA;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.Service;
using Mosa.FileSystem.FAT;
using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;
using RTC = Mosa.Kernel.x86.RTC;

namespace Mosa.Demo.SVGAWorld.x86
{
	/// <summary>
	/// Boot
	/// </summary>
	public static class Boot
	{
		public static DeviceService DeviceService;

		public static Taskbar Taskbar;
		public static Random Random;

		private static Hardware HAL;

		private static PCService PCService;

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

			Log("<SELFTEST:PASSED>");

			HAL = new Hardware();
			Random = new Random();
			DeviceService = new DeviceService();

			// Create service manager and basic services
			var serviceManager = new ServiceManager();
			var partitionService = new PartitionService();

			serviceManager.AddService(DeviceService);
			serviceManager.AddService(new DiskDeviceService());
			serviceManager.AddService(partitionService);
			serviceManager.AddService(new PCService());
			serviceManager.AddService(new PCIControllerService());
			serviceManager.AddService(new PCIDeviceService());

			DeviceSystem.Setup.Initialize(HAL, DeviceService.ProcessInterrupt);

			DeviceService.RegisterDeviceDriver(DeviceDriver.Setup.GetDeviceDriverRegistryEntries());
			DeviceService.Initialize(new X86System(), null);

			PCService = serviceManager.GetFirstService<PCService>();

			partitionService.CreatePartitionDevices();

			foreach (var partition in DeviceService.GetDevices<IPartitionDevice>())
				FileManager.Register(new FatFileSystem(partition.DeviceDriver as IPartitionDevice));

			Display.DefaultFont = GeneralUtils.Load(FileManager.ReadAllBytes("font.bin"));

			GeneralUtils.Fonts = new List<ISimpleFont>
			{
				Display.DefaultFont,
				GeneralUtils.Load(FileManager.ReadAllBytes("font2.bin"))
			};

			if (!Display.Initialize())
			{
				Log("An error occured when initializing the graphics driver.");
				for (; ; );
			}

			GeneralUtils.Mouse = DeviceService.GetFirstDevice<StandardMouse>().DeviceDriver as StandardMouse;
			if (GeneralUtils.Mouse == null)
				HAL.Abort("Mouse not found.");

			DoGraphics();

			/*var keyboardDevice = (IKeyboardDevice)DeviceService.GetFirstDevice<IKeyboardDevice>().DeviceDriver;
			var graphicsDevice = (IGraphicsDevice)DeviceService.GetFirstDevice<IGraphicsDevice>().DeviceDriver;

			graphicsDevice.SetMode(640, 480);

			var font = new ASC16Font();
			var textDevice = new GraphicalTextDevice(640, 480, font, graphicsDevice.FrameBuffer);
			var keyboard = new DeviceSystem.Keyboard(keyboardDevice, new US());
			var textScreen = new TextScreen(textDevice, keyboard);

			textScreen.WriteLine("Hello, World!");
			textScreen.WriteLine("Type something to get it echoed back at you.");

			for (;;)
			{
				textScreen.Write("> ");
				var line = textScreen.ReadLine();
				textScreen.WriteLine("You typed: " + line);
			}*/
		}

		private static void DoGraphics()
		{
			GeneralUtils.BackColor = Color.Indigo;
			GeneralUtils.Mouse.SetScreenResolution(Display.Width, Display.Height);

			Mouse.Initialize();
			WindowManager.Initialize();

			Taskbar = new Taskbar();
			Taskbar.Buttons.Add(new TaskbarButton(Taskbar, "Shutdown", Color.Blue, Color.White, Color.Navy,
				() => { PCService.Shutdown(); return null; }));
			Taskbar.Buttons.Add(new TaskbarButton(Taskbar, "Reset", Color.Blue, Color.White, Color.Navy,
				() => { PCService.Reset(); return null; }));
			Taskbar.Buttons.Add(new TaskbarButton(Taskbar, "Paint", Color.Coral, Color.White, Color.Red,
				() => { WindowManager.Open(new Paint(70, 90, 400, 200, Color.MediumPurple, Color.Purple, Color.White)); return null; }));
			Taskbar.Buttons.Add(new TaskbarButton(Taskbar, "Settings", Color.Coral, Color.White, Color.Red,
				() => { WindowManager.Open(new Settings(70, 90, 400, 200, Color.MediumPurple, Color.Purple, Color.White)); return null; }));

			for (; ; )
			{
				// Clear screen
				Display.Clear(GeneralUtils.BackColor);

				// Draw MOSA logo
				Display.DrawMosaLogo(10);

				// Initialize background labels
				var labels = new List<Label>
				{
					new Label("Current resolution is " + Display.Width + "x" + Display.Height, Display.DefaultFont, 10, 10, Color.OrangeRed),
					new Label("FPS is " + FPSMeter.FPS, Display.DefaultFont, 10, 26, Color.Lime),
					new Label("Current driver is " + Display.CurrentDriver, Display.DefaultFont, 10, 42, Color.MidnightBlue),
					new Label("Current font is " + Display.DefaultFont.Name, Display.DefaultFont, 10, 58, Color.LightSeaGreen),
					new Label((RTC.Hour < 10 ? "0" : string.Empty) + RTC.Hour + ":" + (RTC.Minute < 10 ? "0" : string.Empty) + RTC.Minute, Display.DefaultFont, 10, 74, Color.DeepPink)
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
			if (interrupt is >= 0x20 and < 0x30)
				DeviceSystem.HAL.ProcessInterrupt((byte)(interrupt - 0x20));
		}
	}
}
