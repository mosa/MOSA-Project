// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.Kernel.BareMetal.BootMemory;
using Mosa.Kernel.BareMetal.GC;
using Mosa.Runtime;
using Mosa.Runtime.Plug;

namespace Mosa.Kernel.BareMetal;

public static class Boot
{
	[Plug("Mosa.Runtime.StartUp::PlatformInitialization")]
	public static void PlatformInitialization()
	{
		BootStatus.Initalize();

		Console.SetBackground(ConsoleColor.Black);
		Console.ClearScreen();

		Console.WriteLine(ConsoleColor.BrightYellow, "Initializing kernel...");
		Debug.Setup(true);

		Console.Write(ConsoleColor.BrightGreen, "> Initial garbage collection...");
		InitialGCMemory.Initialize();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Platform initialization...");
		Platform.EntryPoint();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");
	}

	[Plug("Mosa.Runtime.StartUp::GarbageCollectionInitialization")]
	public static void GarbageCollectionInitialization()
	{
	}

	[Plug("Mosa.Runtime.StartUp::KernelEntryPoint")]
	public static void EntryPoint()
	{
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
		PhysicalPageAllocator.Setup();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Page table...");
		PageTable.Setup();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Virtual memory allocator...");
		VirtualMemoryManager.Setup();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Garbage collection...");
		GCMemory.Initialize();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		//Scheduler.Setup();
		//IDT.SetInterruptHandler(ProcessInterrupt);

		Console.WriteLine();
		Console.WriteLine(ConsoleColor.BrightYellow, "Initializing services...");

		// Create Service manager and basic services
		Console.Write(ConsoleColor.BrightGreen, "> Service Manager...");
		var serviceManager = new ServiceManager();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		//	var DeviceService = new DeviceService();

		//	var diskDeviceService = new DiskDeviceService();
		//	var partitionService = new PartitionService();
		//	var pciControllerService = new PCIControllerService();
		//	var pciDeviceService = new PCIDeviceService();
		//	var pcService = new PCService();
	}

	[Plug("Mosa.Runtime.StartUp::StartApplication")]
	public static void StartApplication()
	{
		Console.WriteLine();
		Console.WriteLine(ConsoleColor.BrightYellow, "Executing Application...");
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
