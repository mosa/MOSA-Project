// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.BootMemory;
using Mosa.Kernel.BareMetal.MultibootSpecification;
using Mosa.Runtime.Extension;
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
			Console.WriteLine("Booting...");

			BootPageAllocator.Setup();

			BootMemoryMap.Initialize();

			Platform.UpdateBootMemoryMap();

			BootMemoryMap.ImportMultibootV1MemoryMap();

			//PhysicalPageAllocator.Setup();

			// TODO: PageFrameAllocator --- allocates single pages only

			PageTable.Setup();

			while (true)
			{
			}
		}
	}
}
