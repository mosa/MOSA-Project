// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceDriver;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.Service;
using Mosa.Kernel.BareMetal.BootMemory;
using Mosa.Runtime;
using Mosa.Runtime.Plug;

namespace Mosa.Kernel.BareMetal;

public static class Boot
{
	[Plug("Mosa.Runtime.StartUp::PlatformInitialization")]
	public static void PlatformInitialization()
	{
		Debug.WriteLine("[Platform Initialization]");

		BootStatus.Initalize();

		Console.SetBackground(ConsoleColor.Black);
		Console.ClearScreen();

		Console.WriteLine(ConsoleColor.BrightYellow, "MOSA BareMetal v0.1");

		Console.WriteLine();

		Console.WriteLine(ConsoleColor.BrightYellow, "Initializing kernel...");
		Debug.Setup(true);

		Console.Write(ConsoleColor.BrightGreen, "> Initial garbage collection...");
		InitialGCMemory.Initialize();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Platform initialization...");
		Platform.EntryPoint();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");
	}

	[Plug("Mosa.Runtime.StartUp::KernelEntryPoint")]
	public static void EntryPoint()
	{
		Debug.WriteLine("[Entry Point]");

		Console.Write(ConsoleColor.BrightGreen, "> Enabling debug logging...");
		Debug.Setup(true);
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Boot page allocator...");
		BootPageAllocator.Setup();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Memory map...");
		BootMemoryMap.Setup();
		BootMemoryMap.Dump();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Physical page allocator...");
		PageFrameAllocator.Setup();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Page table...");
		PageTable.Setup();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Virtual page allocator...");
		VirtualPageAllocator.Setup();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Interrupt Manager...");
		InterreuptManager.Setup();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Garbage collection...");
		GCMemory.Setup();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");
		Console.Write(ConsoleColor.BrightGreen, "> Interrupt Handler...");
		InterreuptManager.SetHandler(null);
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Virtual memory allocator...");
		VirtualMemoryAllocator.Setup();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		//Console.Write(ConsoleColor.BrightGreen, "> Scheduler...");
		//Scheduler.Setup();
		//Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.WriteLine();
		Console.WriteLine(ConsoleColor.BrightYellow, "Initializing services...");

		Console.Write(ConsoleColor.BrightGreen, "> Service Manager...");
		var serviceManager = new ServiceManager();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Disk Device Service...");
		var diskDeviceService = new DiskDeviceService();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Partition Service...");
		var partitionService = new PartitionService();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> PCI Controller Service...");
		var pciControllerService = new PCIControllerService();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> PCI Device Service...");
		var pciDeviceService = new PCIDeviceService();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> PC Service...");
		var pcService = new PCService();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Device Manager...");
		var deviceService = new DeviceService();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Initializing hardware abstraction layer...");
		var hardware = new HAL.Hardware();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Intalizing device system...");
		DeviceSystem.Setup.Initialize(hardware, deviceService.ProcessInterrupt);
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Registering device drivers...");
		deviceService.RegisterDeviceDriver(DeviceDriver.Setup.GetDeviceDriverRegistryEntries());
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Debug.WriteLine("[System]");

		var system = new X86System();

		Debug.WriteLine("Name: ");

		Console.WriteLine(system.Device.Name);

		Console.Write(ConsoleColor.BrightGreen, "> X86System...");
		deviceService.Initialize(system, null);
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Debug.WriteLine("Done");
		Debug.Kill();
	}

	[Plug("Mosa.Runtime.GC::AllocateMemory")]
	private static Pointer AllocateMemory(uint size)
	{
		if (BootStatus.IsGCEnabled)
			return GCMemory.AllocateMemory(size);
		else
			return InitialGCMemory.AllocateMemory(size);
	}
}
