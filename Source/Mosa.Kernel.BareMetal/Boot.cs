// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.BootMemory;
using Mosa.Kernel.BareMetal.GC;
using Mosa.Runtime;
using Mosa.Runtime.Plug;

namespace Mosa.Kernel.BareMetal
{
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
			Console.Write("Booting...");

			BootPageAllocator.Setup();

			Console.Write("1...");
			BootMemoryMap.Initialize();

			Console.Write("3...");
			BootMemoryMap.ImportPlatformMemoryMap();

			Console.Write("2...");
			BootMemoryMap.ImportMultibootV1MemoryMap();

			//BootMemoryMap.Dump();

			Console.Write("4...");
			PhysicalPageAllocator.Setup();

			Console.WriteLine("5...");
			PageTable.Setup();

			Console.WriteLine("6...");

			while (true) { }
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
}
