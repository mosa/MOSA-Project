// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.BootMemory;
using Mosa.Kernel.BareMetal.MultibootSpecification;
using Mosa.Runtime.Extension;
using Mosa.Runtime.Plug;

namespace Mosa.Kernel.BareMetal
{
	public static class Boot
	{
		[Plug("Mosa.Runtime.StartUp::InitalizeKernal")]
		public static void EntryPoint()
		{
			Platform.EntryPoint();

			BootPageAllocator.Setup();

			BootMemoryMap.Initialize();

			Platform.UpdateBootMemoryMap();

			BootMemoryMap.ImportMultibootV1MemoryMap();

			PhysicalPageAllocator.Setup();

			// TODO: SinglePageAllocator --- allocates single pages only

			PageTable.Setup();
		}
	}
}
