// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Demo.AppSystem;
using Mosa.DeviceDriver;
using Mosa.DeviceDriver.ISA;
using Mosa.DeviceDriver.ScanCodeMap;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;
using Mosa.DeviceSystem.Service;
using Mosa.FileSystem.FAT;
using Mosa.Kernel.x86;
using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;

namespace Mosa.Demo.CoolWorld.x86;

/// <summary>
/// Boot
/// </summary>
public static class Boot
{
	public static ConsoleSession Console, Debug;

	public static DeviceService DeviceService;

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
		Console.Goto(0, 0);
		Console.ScrollRow = 23;
		Console.Color = ScreenColor.White;
		Console.BackgroundColor = ScreenColor.Green;

		Debug = ConsoleManager.Controller.Debug;

		Console.Write("                   MOSA OS Version 2.2 - Compiler Version 2.2");
		FillLine();
		Console.Color = ScreenColor.White;
		Console.BackgroundColor = ScreenColor.Black;

		Console.WriteLine("> Initializing services...");

		// Create Service manager and basic services
		var serviceManager = new ServiceManager();

		DeviceService = new DeviceService();

		var diskDeviceService = new DiskDeviceService();
		var partitionService = new PartitionService();
		var pciControllerService = new PCIControllerService();
		var pciDeviceService = new PCIDeviceService();
		var pcService = new PCService();

		Console.WriteLine("> Initializing hardware abstraction layer...");

		// Set device driver system with the hardware HAL
		var hardware = new HAL.Hardware();

		DeviceSystem.Setup.Initialize(hardware, DeviceService.ProcessInterrupt);

		serviceManager.AddService(DeviceService);
		serviceManager.AddService(diskDeviceService);
		serviceManager.AddService(partitionService);
		serviceManager.AddService(pciControllerService);
		serviceManager.AddService(pciDeviceService);
		serviceManager.AddService(pcService);

		Console.WriteLine("> Registering device drivers...");

		DeviceService.RegisterDeviceDriver(DeviceDriver.Setup.GetDeviceDriverRegistryEntries());

		Console.WriteLine("> Starting devices...");

		DeviceService.Initialize(new X86System(), null);

		var acpi = DeviceService.GetFirstDevice<IACPI>().DeviceDriver as IACPI;

		// TODO:
		// Initialize APIC when ACPI is initialized
		// Re-register all IRQs to use with the APIC

		// Setup APIC
		/*var localApic = DeviceSystem.HAL.GetPhysicalMemory((Pointer)acpi.LocalApicAddress, 0xFFFF).Address;
		var ioApic = DeviceSystem.HAL.GetPhysicalMemory((Pointer)acpi.IOApicAddress, 0xFFFF).Address;

		PIC.Disable();

		LocalAPIC.Setup(localApic);
		IOAPIC.Setup(ioApic);*/

		Console.Write("> Probing for ISA devices...");
		var isaDevices = DeviceService.GetChildrenOf(DeviceService.GetFirstDevice<ISABus>());
		Console.WriteLine("[Completed: " + isaDevices.Count + " found]");

		foreach (var device in isaDevices)
		{
			Console.Write("  ");
			Bullet(ScreenColor.Yellow);
			Console.Write(" ");
			InBrackets(device.Name, ScreenColor.White, ScreenColor.Green);
			Console.WriteLine();
		}

		Console.Write("> Probing for PCI devices...");
		var devices = DeviceService.GetDevices<PCIDevice>();
		Console.WriteLine("[Completed: " + devices.Count + " found]");

		foreach (var device in devices)
		{
			Console.Write("  ");
			Bullet(ScreenColor.Yellow);
			Console.Write(" ");

			var pciDevice = device.DeviceDriver as PCIDevice;
			InBrackets(device.Name + ": " + pciDevice.VendorID.ToString("x") + ":" + pciDevice.DeviceID.ToString("x") + " " + pciDevice.SubSystemID.ToString("x") + ":" + pciDevice.SubSystemVendorID.ToString("x") + " (" + pciDevice.ClassCode.ToString("x") + ":" + pciDevice.SubClassCode.ToString("x") + ":" + pciDevice.ProgIF.ToString("x") + ":" + pciDevice.RevisionID.ToString("x") + ")", ScreenColor.White, ScreenColor.Green);

			var children = DeviceService.GetChildrenOf(device);

			if (children.Count != 0)
			{
				var child = children[0];

				Console.WriteLine();
				Console.Write("    ");

				var pciDevice2 = child.DeviceDriver as PCIDevice;
				InBrackets(child.Name, ScreenColor.White, ScreenColor.Green);
			}

			Console.WriteLine();
		}

		Console.Write("> Probing for disk controllers...");
		var diskcontrollers = DeviceService.GetDevices<IDiskControllerDevice>();
		Console.WriteLine("[Completed: " + diskcontrollers.Count + " found]");

		foreach (var device in diskcontrollers)
		{
			Console.Write("  ");
			Bullet(ScreenColor.Yellow);
			Console.Write(" ");
			InBrackets(device.Name, ScreenColor.White, ScreenColor.Green);
			Console.WriteLine();
		}

		Console.Write("> Probing for disks...");
		var disks = DeviceService.GetDevices<IDiskDevice>();
		Console.WriteLine("[Completed: " + disks.Count + " found]");

		foreach (var disk in disks)
		{
			Console.Write("  ");
			Bullet(ScreenColor.Yellow);
			Console.Write(" ");
			InBrackets(disk.Name, ScreenColor.White, ScreenColor.Green);
			Console.Write(" " + (disk.DeviceDriver as IDiskDevice).TotalBlocks + " blocks");
			Console.WriteLine();
		}

		partitionService.CreatePartitionDevices();

		Console.Write("> Finding partitions...");
		var partitions = DeviceService.GetDevices<IPartitionDevice>();
		Console.WriteLine("[Completed: " + partitions.Count + " found]");

		//foreach (var partition in partitions)
		//{
		//	Console.Write("  ");
		//	Bullet(ScreenColor.Yellow);
		//	Console.Write(" ");
		//	InBrackets(partition.Name, ScreenColor.White, ScreenColor.Green);
		//	Console.Write(" " + (partition.DeviceDriver as IPartitionDevice).BlockCount.ToString() + " blocks");
		//	Console.WriteLine();
		//}

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

					Console.WriteLine(" - Length: " + len + " bytes");

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

				const string bmpname = "WALLP.BMP";

				var bmploc = fat.FindEntry(bmpname);

				if (bmploc.IsValid)
				{
					Console.Write("Found: " + bmpname);

					var fatFileStream = new FatFileStream(fat, bmploc);

					uint len = (uint)fatFileStream.Length;

					Console.WriteLine(" - Length: " + len + " bytes");
					Console.WriteLine();
				}
			}
		}

		// Get StandardKeyboard
		var keyboards = DeviceService.GetDevices("StandardKeyboard");
		if (keyboards.Count == 0)
		{
			Console.WriteLine("No Keyboard!");
			ForeverLoop();
		}

		var stdKeyboard = keyboards[0].DeviceDriver as IKeyboardDevice;

		// setup keymap
		var keymap = new US();

		// setup keyboard (state machine)
		var keyboard = new DeviceSystem.Keyboard(stdKeyboard, keymap);

		// setup app manager
		var manager = new AppManager(Console, keyboard, serviceManager);

		IDT.SetInterruptHandler(manager.ProcessInterrupt);

		Logger.Log("##PASS##");

		manager.Start();
	}

	public static void Pause()
	{
		DeviceSystem.HAL.Yield();
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

	private static uint counter;

	public static void ProcessInterrupt(uint interrupt, uint errorCode)
	{
		counter++;

		uint c = Console.Column;
		uint r = Console.Row;
		var col = Console.Color;
		var back = Console.BackgroundColor;

		Console.Column = 31;
		Console.Row = 0;
		Console.Color = ScreenColor.Cyan;
		Console.BackgroundColor = ScreenColor.Black;

		Console.Write(counter, 10, 7);
		Console.Write(':');
		Console.Write(interrupt, 16, 2);
		Console.Write(':');
		Console.Write(errorCode, 16, 2);

		Console.Column = c;
		Console.Row = r;
		Console.Color = col;
		Console.BackgroundColor = back;
	}
}
