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
		Console.SetBackground(ConsoleColor.Blue);
		Console.SetForground(ConsoleColor.White);
		Console.ClearScreen();

		Console.WriteLine("Initializing boot page allocator...");
		BootPageAllocator.Setup();

		Console.WriteLine("Initializing memory map...");
		BootMemoryMap.Initialize();
		BootMemoryMap.ImportPlatformMemoryMap();
		BootMemoryMap.ImportMultibootV1MemoryMap();
		BootMemoryMap.Dump();

		Console.WriteLine("Initializing physical page allocator...");
		PhysicalPageAllocator.Setup();

		Console.WriteLine("Initializing page table...");
		PageTable.Setup();
	}

	[Plug("Mosa.Runtime.StartUp::GarbageCollectionInitialization")]
	public static void GarbageCollectionInitialization()
	{
		GCMemory.Initialize();
	}

	[Plug("Mosa.Runtime.GC::AllocateMemory")]
	private static Pointer AllocateMemory(uint size)
	{
		return GCMemory.AllocateMemory(size);
	}
}
