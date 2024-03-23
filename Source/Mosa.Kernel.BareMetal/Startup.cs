// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceDriver;
using Mosa.DeviceDriver.ISA;
using Mosa.DeviceDriver.ScanCodeMap;
using Mosa.DeviceSystem.Disks;
using Mosa.DeviceSystem.HardwareAbstraction;
using Mosa.DeviceSystem.Keyboard;
using Mosa.DeviceSystem.Services;
using Mosa.FileSystem.FAT;
using Mosa.Kernel.BareMetal.BootMemory;
using Mosa.Runtime;
using Mosa.Runtime.Plug;

namespace Mosa.Kernel.BareMetal;

public static class Startup
{
	[Plug("Mosa.Runtime.StartUp::InitializePlatform")]
	public static void Initialize(Pointer stackFrame)
	{
		Platform.Interrupt.Disable();
		Platform.Setup(stackFrame);
		BootOptions.Setup();
		Debug.Setup();
		BootStatus.Initalize();

		Debug.WriteLine("Startup.Initialize()");
		Debug.WriteLine(" > Initial Stack @ ", new Hex(stackFrame));

		Console.BackgroundColor = ConsoleColor.Black;
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.Clear();

		Console.WriteLine("MOSA BareMetal v2.4");

		Console.WriteLine();

		Console.WriteLine("Initializing kernel...");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Initial garbage collection...");
		InitialGCMemory.Initialize();
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Platform initialization...");
		Platform.Initialize();
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Debug.WriteLine("Startup.Initialize() [Exit]");
	}

	[Plug("Mosa.Runtime.StartUp::KernelEntryPoint")]
	public static void EntryPoint()
	{
		Debug.WriteLine("Startup.EntryPoint()");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Enabling debug logging...");
		Debug.Setup();
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Boot page allocator...");
		BootPageAllocator.Setup();
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Memory map...");
		BootMemoryMap.Setup();
		BootMemoryMap.Dump();
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Physical page allocator...");
		PageFrameAllocator.Setup();
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Page table...");
		PageTable.Setup();
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Virtual page allocator...");
		VirtualPageAllocator.Setup();
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Interrupt Manager...");
		InterruptManager.Setup();
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Garbage collection...");
		GCMemory.Setup();
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");
		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Initializing interrupt handler...");
		InterruptManager.SetHandler(null);
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Virtual memory allocator...");
		VirtualMemoryAllocator.Setup();
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Scheduler...");
		Scheduler.Setup();
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Hardware abstraction layer...");
		var hardware = new HardwareAbstractionLayer();
		var deviceService = new DeviceService();
		HAL.Set(hardware);
		HAL.SetInterruptHandler(deviceService.ProcessInterrupt);
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Registering device drivers...");
		deviceService.RegisterDeviceDriver(Setup.GetDeviceDriverRegistryEntries());
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine();
		Console.WriteLine("Initializing services...");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Service Manager...");
		var serviceManager = new ServiceManager();
		var diskDeviceService = new DiskDeviceService();
		var partitionService = new PartitionService();
		var pciControllerService = new PCIControllerService();
		var pciDeviceService = new PCIDeviceService();
		var pcService = new PCService();
		Kernel.ServiceManager = serviceManager;
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Device Service...");
		serviceManager.AddService(deviceService);
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Disk Device Service...");
		serviceManager.AddService(diskDeviceService);
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Partition Service...");
		serviceManager.AddService(partitionService);
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> PCI Controller Service...");
		serviceManager.AddService(pciControllerService);
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> PCI Device Service...");
		serviceManager.AddService(pciDeviceService);
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> PC Service...");
		serviceManager.AddService(pcService);
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		// Future: We shouldn't be directly initializing ISA devices on non-x86 platforms (even if it doesn't do anything on those)
		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> ISABus...");
		deviceService.Initialize(new ISABus(), null);
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Creating partition devices...");
		partitionService.CreatePartitionDevices();
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Registering partition devices...");
		{
			foreach (var partition in deviceService.GetDevices<IPartitionDevice>())
				FileManager.Register(new FatFileSystem(partition.DeviceDriver as IPartitionDevice));
		}
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> StandardKeyboard...");
		{
			var stdKeyboard = deviceService.GetFirstDevice<StandardKeyboard>().DeviceDriver as IKeyboardDevice;
			if (stdKeyboard == null)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(" [FAIL]");
				Console.WriteLine("No keyboard detected!");

				for (;;)
					HAL.Yield();
			}

			Kernel.Keyboard = new Keyboard(stdKeyboard, new US());
		}
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Setting interrupt handler...");
		InterruptManager.SetHandler(ProcessInterrupt);
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Console.ForegroundColor = ConsoleColor.LightGreen;
		Console.Write("> Enabling interrupts...");
		Platform.Interrupt.Enable();
		Console.ForegroundColor = ConsoleColor.DarkGray;
		Console.WriteLine(" [Completed]");

		Debug.WriteLine("Startup.EntryPoint() [Exit]");
	}

	[Plug("Mosa.Runtime.GC::AllocateMemory")]
	private static Pointer AllocateMemory(uint size)
	{
		return BootStatus.IsGCEnabled
			? GCMemory.AllocateMemory(size)
			: InitialGCMemory.AllocateMemory(size);
	}

	private static void ProcessInterrupt(uint interrupt, uint errorCode)
	{
		if (interrupt is >= 0x20 and < 0x30)        // FIXME: X86 specific
		{
			HAL.ProcessInterrupt((byte)(interrupt - 0x20));
		}
	}
}
