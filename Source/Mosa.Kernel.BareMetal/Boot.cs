// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
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

		Console.Write(ConsoleColor.BrightGreen, "> Boot page allocator...");
		BootPageAllocator.Setup();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Memory map...");
		BootMemoryMap.Initialize();
		BootMemoryMap.ImportPlatformMemoryMap();
		BootMemoryMap.ImportMultibootV1MemoryMap();
		//BootMemoryMap.Dump();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Physical page allocator...");
		PhysicalPageAllocator.Setup();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.Write(ConsoleColor.BrightGreen, "> Page table...");
		PageTable.Setup();
		Console.WriteLine(ConsoleColor.BrightBlack, " [Completed]");

		Console.WriteLine();
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
