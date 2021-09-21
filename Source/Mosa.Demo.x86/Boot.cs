// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Demo.AppSystem;
using Mosa.DeviceDriver;
using Mosa.DeviceDriver.ISA;
using Mosa.DeviceDriver.ScanCodeMap;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;
using Mosa.FileSystem.FAT;
using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;
using Mosa.Demo.x86.HAL;

namespace Mosa.Demo.x86
{
	/// <summary>
	/// Boot
	/// </summary>
	public static class Boot
	{
		public static ConsoleSession Console, Debug;
		public static Hardware Hardware;

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
			// You can write messages to serial ports too!
			//Serial.SetupPort(Serial.COM1);
			Logger.Log("<SELFTEST:PASSED>");

			Console = ConsoleManager.Controller.Boot;

			Console.Clear();
			Console.Goto(0, 0);
			Console.ScrollRow = 23;
			Console.Color = ScreenColor.White;
			Console.BackgroundColor = ScreenColor.Green;

			Debug = ConsoleManager.Controller.Boot;

			Console.Write("                   MOSA OS Version 2.2 - Compiler Version 2.2");
			FillLine();
			Console.Color = ScreenColor.White;
			Console.BackgroundColor = ScreenColor.Black;

			Console.WriteLine("> Initializing services...");

			// Create Service manager and basic services
			var serviceManager = new ServiceManager();

			var deviceService = new DeviceService();

			var diskDeviceService = new DiskDeviceService();
			var partitionService = new PartitionService();
			var pciControllerService = new PCIControllerService();
			var pciDeviceService = new PCIDeviceService();

			serviceManager.AddService(deviceService);
			serviceManager.AddService(diskDeviceService);
			serviceManager.AddService(partitionService);
			serviceManager.AddService(pciControllerService);
			serviceManager.AddService(pciDeviceService);

			Console.WriteLine("> Initializing hardware abstraction layer...");

			// Set device driver system with the hardware HAL
			Hardware = new Hardware();

			DeviceSystem.Setup.Initialize(Hardware, deviceService.ProcessInterrupt);

			Console.WriteLine("> Registering device drivers...");
			deviceService.RegisterDeviceDriver(DeviceDriver.Setup.GetDeviceDriverRegistryEntries());

			Console.WriteLine("> Starting devices...");

			deviceService.Initialize(new X86System(), null);

			// Get StandardKeyboard
			var standardKeyboards = deviceService.GetDevices("StandardKeyboard");

			if (standardKeyboards.Count == 0)
			{
				Console.WriteLine("No keyboard found!");
				ForeverLoop();
			}

			// Setup keyboard (state machine) with US keymap
			var keyboard = new DeviceSystem.Keyboard(standardKeyboards[0].DeviceDriver as IKeyboardDevice, new US());

			if (VBEDisplay.InitVBE(Hardware))
			{
				var standardMice = deviceService.GetDevices("StandardMouse");
				if (standardMice.Count == 0)
				{
					Console.WriteLine("No mouse detected!");
					ForeverLoop();
				}

				var mouse = standardMice[0].DeviceDriver as StandardMouse;
				mouse.SetScreenResolution(VBE.ScreenWidth, VBE.ScreenHeight);

				IDT.SetInterruptHandler(ProcessInterrupt);

				Console.WriteLine("Successfully initialized VBE!");

				for (; ; )
				{
					// Clear screen
					VBEDisplay.Framebuffer.Clear(0x00555555);

					// Draw MOSA logo
					MosaLogo.Draw(VBEDisplay.Framebuffer, 10);

					// Draw mouse
					VBEDisplay.Framebuffer.FillRectangle(0x0, (uint)mouse.X, (uint)mouse.Y, 10, 10);

					// Swap display buffers (since we're using double buffering)
					VBEDisplay.Framebuffer.Update();
				}
			}
			else Console.WriteLine("This computer doesn't support VBE!");

			Console.Write("> Probing for ISA devices...");
			var isaDevices = deviceService.GetChildrenOf(deviceService.GetFirstDevice<ISABus>());
			Console.WriteLine("[Completed: " + isaDevices.Count.ToString() + " found]");

			Console.Write("> Probing for PCI devices...");
			var devices = deviceService.GetDevices<PCIDevice>();
			Console.WriteLine("[Completed: " + devices.Count.ToString() + " found]");

			Console.Write("> Probing for disk controllers...");
			var diskcontrollers = deviceService.GetDevices<IDiskControllerDevice>();
			Console.WriteLine("[Completed: " + diskcontrollers.Count.ToString() + " found]");

			Console.Write("> Probing for disks...");
			var disks = deviceService.GetDevices<IDiskDevice>();
			Console.WriteLine("[Completed: " + disks.Count.ToString() + " found]");

			partitionService.CreatePartitionDevices();

			Console.Write("> Finding partitions...");
			var partitions = deviceService.GetDevices<IPartitionDevice>();
			Console.WriteLine("[Completed: " + partitions.Count.ToString() + " found]");

			Console.Write("> Finding file systems...");
			foreach (var partition in partitions)
			{
				var fat = new FatFileSystem(partition.DeviceDriver as IPartitionDevice);

				if (fat.IsValid)
				{
					Console.WriteLine("Found a FAT file system!");

					const string filename = "TEST.TXT";

					var location = fat.FindEntry(filename);

					if (location.IsValid)
					{
						Console.Write("Found: " + filename);

						var fatFileStream = new FatFileStream(fat, location);

						uint len = (uint)fatFileStream.Length;

						Console.WriteLine(" - Length: " + len.ToString());

						Console.Write("Reading File: ");

						for (; ; )
						{
							int i = fatFileStream.ReadByte();

							if (i < 0)
								break;

							Console.Write((char)i);
						}

						Console.WriteLine();
					}
				}
			}

			Debug = ConsoleManager.Controller.Debug;

			// Setup app manager
			var manager = new AppManager(Console, keyboard, serviceManager);

			IDT.SetInterruptHandler(manager.ProcessInterrupt);

			manager.Start();
		}

		public static void ProcessInterrupt(uint interrupt, uint errorCode)
		{
			if (interrupt >= 0x20 && interrupt < 0x30)
				DeviceSystem.HAL.ProcessInterrupt((byte)(interrupt - 0x20));
		}

		public static void Pause()
		{
			DeviceSystem.HAL.Pause();
		}

		public static void ForeverLoop()
		{
			for (; ; )
				Hardware.HaltCPU();
		}

		public static void FillLine()
		{
			for (uint c = 80 - Console.Column; c != 0; c--)
				Console.Write(' ');
		}

		public static void InBrackets(string message, byte outerColor, byte innerColor)
		{
			var restore = Console.Color;
			Console.Color = outerColor;
			Console.Write("[");
			Console.Color = innerColor;
			Console.Write(message);
			Console.Color = outerColor;
			Console.Write("]");
			Console.Color = restore;
		}

		public static void Bullet(byte color)
		{
			var restore = Console.Color;
			Console.Color = color;
			Console.Write("*");
			Console.Color = restore;
		}
	}
}
