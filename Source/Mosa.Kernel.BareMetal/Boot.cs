// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
		Platform.EntryPoint();
	}

	[Plug("Mosa.Runtime.StartUp::KernelEntryPoint")]
	public static void EntryPoint()
	{
		Console.SetBackground(ConsoleColor.Black);
		Console.ClearScreen();

		Console.WriteLine(ConsoleColor.BrightYellow, "Initializing kernel...");

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

		Console.WriteLine();

		//VirtualPageAllocator.Setup();
		//GC.Setup();
		//Scheduler.Setup();
		//IDT.SetInterruptHandler(ProcessInterrupt);

		Console.WriteLine(ConsoleColor.BrightYellow, "Initializing services...");

		// Create Service manager and basic services
		//var serviceManager = new ServiceManager();

		//	var DeviceService = new DeviceService();

		//	var diskDeviceService = new DiskDeviceService();
		//	var partitionService = new PartitionService();
		//	var pciControllerService = new PCIControllerService();
		//	var pciDeviceService = new PCIDeviceService();
		//	var pcService = new PCService();
	}

	[Plug("Mosa.Runtime.StartUp::GarbageCollectionInitialization")]
	public static void GarbageCollectionInitialization()
	{
		Console.WriteLine("> Garbage Collection...");
		GCMemory.Initialize();
	}

	[Plug("Mosa.Runtime.GC::AllocateMemory")]
	private static Pointer AllocateMemory(uint size)
	{
		return GCMemory.AllocateMemory(size);
	}
}
