// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.BootMemory;
using Mosa.Runtime.Plug;

namespace Mosa.Kernel.BareMetal
{
	public static class Boot
	{
		[Plug("Mosa.Runtime.StartUp::KernelInitialization")]
		public static void EntryPoint()
		{
			Platform.EntryPoint();

			Console.SetBackground(ConsoleColor.Blue);
			Console.SetForground(ConsoleColor.White);
			Console.ClearScreen();
			Console.Write("Booting...");

			BootPageAllocator.Setup();

			Console.Write("1...");
			BootMemoryMap.Initialize();

			Console.Write("2...");
			BootMemoryMap.ImportMultibootV1MemoryMap();

			Console.Write("3...");
			BootMemoryMap.ImportPlatformMemoryMap();
			BootMemoryMap.Dump();

			Console.Write("4...");
			PhysicalPageAllocator.Setup();

			Console.Write("5...");
			PageTable.Setup();

			Console.Write("6...");

			while (true)
			{
			}
		}
	}
}
