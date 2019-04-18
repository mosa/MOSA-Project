// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.BootMemory;
using Mosa.Kernel.BareMetal.MultibootSpecification;
using Mosa.Runtime.Extension;

namespace Mosa.Kernel.BareMetal
{
	public static class Boot
	{
		public static void EntryPoint()
		{
			Platform.EntryPoint();

			BootMemoryMap.Initialize();

			Platform.UpdateBootMemoryMap();

			ImportMultibootMemoryMap();
		}

		private static void ImportMultibootMemoryMap()
		{
			if (!Multiboot.IsAvailable)
				return;

			if (Multiboot.MultibootV1.MemoryMapStart.IsNull())
				return;

			var memoryMapEnd = Multiboot.MultibootV1.MemoryMapStart + (int)Multiboot.MultibootV1.MemoryMapLength;

			var entry = new MultibootV1MemoryMapEntry(Multiboot.MultibootV1.MemoryMapStart);

			while (entry.IsAvailable)
			{
				BootMemoryMap.SetMemoryMap(entry.BaseAddr, entry.Length, entry.Type == 1 ? BootMemoryMapType.Available : BootMemoryMapType.Reserved);

				entry = entry.GetNext(memoryMapEnd);
			}
		}
	}
}
