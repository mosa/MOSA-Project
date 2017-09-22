// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.AppSystem;
using Mosa.ClassLib;
using Mosa.DeviceDriver.ScanCodeMap;
using Mosa.FileSystem.FAT;

//using Mosa.HardwareSystem;
using Mosa.Kernel.x86;
using Mosa.Runtime.x86;

namespace Mosa.CoolWorld.x86
{
	/// <summary>
	///
	/// </summary>
	public static class Boot
	{
		public static ConsoleSession Console;
		public static ConsoleSession Debug;

		/// <summary>
		/// Main
		/// </summary>
		unsafe public static void Main()
		{
			Kernel.x86.Kernel.Setup();

			Console = ConsoleManager.Controller.Boot;
			Debug = ConsoleManager.Controller.Boot;

			Console.Clear();

			Console.ScrollRow = 23;

			IDT.SetInterruptHandler(ProcessInterrupt);

			Console.Color = Color.White;
			Console.BackgroundColor = Color.Green;

			Console.Write("                   MOSA OS Version 1.5 - Compiler Version 1.5");
			FillLine();
			Console.Color = Color.White;
			Console.BackgroundColor = Color.Black;

			//Debug.Color = Color.White;
			//Debug.BackgroundColor = Color.Blue;
			//Debug.Clear();
			//Debug.WriteLine("Debug Information:");

			Console.WriteLine("> Initializing hardware abstraction layer...");
			var hardware = new HAL.Hardware();
			HardwareSystem.Setup.Initialize(hardware);

			Console.WriteLine("> Registering device drivers...");
			DeviceDriver.Setup.Register(HardwareSystem.Setup.DeviceDriverRegistry);

			Console.Write("> Probing for ISA devices...");
			HardwareSystem.Setup.StartISADevices();
			var isaDevices = HardwareSystem.Setup.DeviceManager.GetAllDevices();
			Console.WriteLine("[Completed: " + isaDevices.Count.ToString() + " found]");

			foreach (var device in isaDevices)
			{
				Console.Write("  ");
				Bullet(Color.Yellow);
				Console.Write(" ");
				InBrackets(device.Name, Color.White, Color.LightGreen);
				Console.WriteLine();
			}

			Console.Write("> Probing for PCI devices...");
			HardwareSystem.Setup.StartPCIDevices();
			var pciDevices = HardwareSystem.Setup.DeviceManager.GetDevices(new HardwareSystem.IsPCIDevice(), new HardwareSystem.IsAvailable());
			Console.WriteLine("[Completed: " + pciDevices.Count.ToString() + " found]");

			foreach (var device in pciDevices)
			{
				var pciDevice = device as HardwareSystem.PCI.IPCIDevice;

				Console.Write("  ");
				Bullet(Color.Yellow);
				Console.Write(" ");
				InBrackets(device.Name + ": " + pciDevice.VendorID.ToString("x") + ":" + pciDevice.DeviceID.ToString("x") + " " + pciDevice.SubSystemID.ToString("x") + ":" + pciDevice.SubVendorID.ToString("x") + " (" + pciDevice.Function.ToString("x") + ":" + pciDevice.ClassCode.ToString("x") + ":" + pciDevice.SubClassCode.ToString("x") + ":" + pciDevice.ProgIF.ToString("x") + ":" + pciDevice.RevisionID.ToString("x") + ")", Color.White, Color.LightGreen);
				Console.WriteLine();
			}

			Console.Write("> Probing for disk controllers...");
			var diskcontrollers = HardwareSystem.Setup.DeviceManager.GetDevices(new DeviceSystem.IsDiskControllerDevice());
			Console.WriteLine("[Completed: " + diskcontrollers.Count.ToString() + " found]");

			foreach (var device in diskcontrollers)
			{
				Console.Write("  ");
				Bullet(Color.Yellow);
				Console.Write(" ");
				InBrackets(device.Name, Color.White, Color.LightGreen);
				Console.WriteLine();
			}

			var diskcontroller = new DeviceSystem.DiskControllerManager(HardwareSystem.Setup.DeviceManager);
			diskcontroller.CreateDiskDevices();

			Console.Write("> Probing for disks...");
			var disks = HardwareSystem.Setup.DeviceManager.GetDevices(new DeviceSystem.IsDiskDevice());
			Console.WriteLine("[Completed: " + disks.Count.ToString() + " found]");
			foreach (var disk in disks)
			{
				Console.Write("  ");
				Bullet(Color.Yellow);
				Console.Write(" ");
				InBrackets(disk.Name, Color.White, Color.LightGreen);
				Console.Write(" " + (disk as DeviceSystem.IDiskDevice).TotalBlocks.ToString() + " blocks");
				Console.WriteLine();
			}

			var partitionManager = new Mosa.DeviceSystem.PartitionManager(HardwareSystem.Setup.DeviceManager);
			partitionManager.CreatePartitionDevices();

			Console.Write("> Finding partitions...");
			var partitions = HardwareSystem.Setup.DeviceManager.GetDevices(new DeviceSystem.IsPartitionDevice());
			Console.WriteLine("[Completed: " + partitions.Count.ToString() + " found]");
			foreach (var partition in partitions)
			{
				Console.Write("  ");
				Bullet(Color.Yellow);
				Console.Write(" ");
				InBrackets(partition.Name, Color.White, Color.LightGreen);
				Console.Write(" " + (partition as DeviceSystem.IPartitionDevice).BlockCount.ToString() + " blocks");
				Console.WriteLine();
			}

			Console.Write("> Finding file systems...");

			foreach (var partition in partitions)
			{
				var fat = new FatFileSystem(partition as DeviceSystem.IPartitionDevice);

				if (fat.IsValid)
				{
					Console.WriteLine("Found a FAT file system!");

					var filename = "TEST.TXT";

					var location = fat.FindEntry(filename);

					if (location.IsValid)
					{
						Console.WriteLine("Found: " + filename);

						var fatFileStream = new FatFileStream(fat, location);

						uint len = (uint)fatFileStream.Length;

						Console.WriteLine("Length: " + len.ToString());

						Console.WriteLine("Reading File:");

						for (;;)
						{
							int i = fatFileStream.ReadByte();

							if (i < 0)
								break;

							Console.Write((char)i);
						}
					}
				}
			}

			ForeverLoop();

			// Get StandardKeyboard
			var standardKeyboards = HardwareSystem.Setup.DeviceManager.GetDevices(new HardwareSystem.WithName("StandardKeyboard"));

			if (standardKeyboards.Count == 0)
			{
				Console.WriteLine("No Keyboard!");
				ForeverLoop();
			}

			var standardKeyboard = standardKeyboards[0] as DeviceSystem.IKeyboardDevice;

			Debug = ConsoleManager.Controller.Debug;

			// setup keymap
			var keymap = new US();

			// setup keyboard (state machine)
			var keyboard = new DeviceSystem.Keyboard(standardKeyboard, keymap);

			// setup app manager
			var manager = new AppManager(Console, keyboard);

			IDT.SetInterruptHandler(manager.ProcessInterrupt);

			manager.Start();
		}

		public static void ForeverLoop()
		{
			while (true)
			{
				Native.Hlt();
			}
		}

		public static void FillLine()
		{
			for (uint c = 80 - Console.Column; c != 0; c--)
			{
				Console.Write(' ');
			}
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

		private static uint tick = 0;

		public static void ProcessInterrupt(uint interrupt, uint errorCode)
		{
			uint c = Console.Column;
			uint r = Console.Row;
			byte col = Console.Color;
			byte back = Console.BackgroundColor;
			uint sr = Console.ScrollRow;

			Console.Color = Color.Cyan;
			Console.BackgroundColor = Color.Black;
			Console.Row = 24;
			Console.Column = 0;
			Console.ScrollRow = Console.Rows;

			tick++;
			Console.Write("Booting - ");
			Console.Write("Tick: ");
			Console.Write(tick, 10, 7);
			Console.Write(" Free Memory: ");
			Console.Write((PageFrameAllocator.TotalPages - PageFrameAllocator.TotalPagesInUse) * PageFrameAllocator.PageSize / (1024 * 1024));
			Console.Write(" MB         ");

			if (interrupt >= 0x20 && interrupt < 0x30)
			{
				Mosa.HardwareSystem.HAL.ProcessInterrupt((byte)(interrupt - 0x20));
			}

			Console.Column = c;
			Console.Row = r;
			Console.Color = col;
			Console.BackgroundColor = back;
			Console.ScrollRow = sr;
		}
	}
}
