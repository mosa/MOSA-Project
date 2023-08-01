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

		ScreenConsole.SetBackground(ScreenColor.Black);
		ScreenConsole.ClearScreen();

		ScreenConsole.WriteLine(ScreenColor.BrightYellow, "MOSA BareMetal v0.1");

		ScreenConsole.WriteLine();

		ScreenConsole.WriteLine(ScreenColor.BrightYellow, "Initializing kernel...");
		Debug.Setup(true);

		ScreenConsole.Write(ScreenColor.BrightGreen, "> Initial garbage collection...");
		InitialGCMemory.Initialize();
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");

		ScreenConsole.Write(ScreenColor.BrightGreen, "> Platform initialization...");
		Platform.EntryPoint();
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");
	}

	[Plug("Mosa.Runtime.StartUp::KernelEntryPoint")]
	public static void EntryPoint()
	{
		Debug.WriteLine("[Kernel Entry Point]");

		ScreenConsole.Write(ScreenColor.BrightGreen, "> Enabling debug logging...");
		Debug.Setup(true);
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");

		ScreenConsole.Write(ScreenColor.BrightGreen, "> Boot page allocator...");
		BootPageAllocator.Setup();
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");

		ScreenConsole.Write(ScreenColor.BrightGreen, "> Memory map...");
		BootMemoryMap.Setup();
		BootMemoryMap.Dump();
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");

		ScreenConsole.Write(ScreenColor.BrightGreen, "> Physical page allocator...");
		PageFrameAllocator.Setup();
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");

		ScreenConsole.Write(ScreenColor.BrightGreen, "> Page table...");
		PageTable.Setup();
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");

		ScreenConsole.Write(ScreenColor.BrightGreen, "> Virtual page allocator...");
		VirtualPageAllocator.Setup();
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");

		ScreenConsole.Write(ScreenColor.BrightGreen, "> Interrupt Manager...");
		InterreuptManager.Setup();
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");

		ScreenConsole.Write(ScreenColor.BrightGreen, "> Garbage collection...");
		GCMemory.Setup();
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");
		ScreenConsole.Write(ScreenColor.BrightGreen, "> Interrupt Handler...");
		InterreuptManager.SetHandler(null);
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");

		ScreenConsole.Write(ScreenColor.BrightGreen, "> Virtual memory allocator...");
		VirtualMemoryAllocator.Setup();
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");

		//Console.Write(ConsoleColor.BrightGreen, "> Scheduler...");
		//Scheduler.Setup();
		//Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		ScreenConsole.Write(ScreenColor.BrightGreen, "> Hardware abstraction layer...");
		var hardware = new HardwareAbstractionLayer();
		var deviceService = new DeviceService();
		DeviceSystem.Setup.Initialize(hardware, deviceService.ProcessInterrupt);
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");

		ScreenConsole.Write(ScreenColor.BrightGreen, "> Registering device drivers...");
		deviceService.RegisterDeviceDriver(DeviceDriver.Setup.GetDeviceDriverRegistryEntries());
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");

		ScreenConsole.WriteLine();
		ScreenConsole.WriteLine(ScreenColor.BrightYellow, "Initializing services...");

		ScreenConsole.Write(ScreenColor.BrightGreen, "> Service Manager...");
		var serviceManager = new ServiceManager();
		Kernel.ServiceManger = serviceManager;
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");

		ScreenConsole.Write(ScreenColor.BrightGreen, "> Device Service...");
		serviceManager.AddService(deviceService);
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");

		ScreenConsole.Write(ScreenColor.BrightGreen, "> Disk Device Service...");
		serviceManager.AddService(new DiskDeviceService());
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");

		ScreenConsole.Write(ScreenColor.BrightGreen, "> Partition Service...");
		serviceManager.AddService(new PartitionService());
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");

		ScreenConsole.Write(ScreenColor.BrightGreen, "> PCI Controller Service...");
		serviceManager.AddService(new PCIControllerService());
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");

		ScreenConsole.Write(ScreenColor.BrightGreen, "> PCI Device Service...");
		serviceManager.AddService(new PCIDeviceService());
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");

		ScreenConsole.Write(ScreenColor.BrightGreen, "> PC Service...");
		serviceManager.AddService(new PCService());
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");

		ScreenConsole.Write(ScreenColor.BrightGreen, "> X86System...");
		deviceService.Initialize(new X86System(), null);
		ScreenConsole.WriteLine(ScreenColor.BrightBlack, " [Completed]");
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
