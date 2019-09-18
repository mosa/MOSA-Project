// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.MultibootSpecification;
using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.BootMemory
{
	public static class BootMemoryMap
	{
		private static BootMemoryMapTable Map;

		public static void Initialize()
		{
			var entry = BootPageAllocator.AllocatePage();
			Page.ClearPage(entry);

			Map = new BootMemoryMapTable(entry)
			{
				Count = 0
			};
		}

		public static void ImportMultibootV1MemoryMap()
		{
			if (!Multiboot.IsAvailable)
				return;

			if (Multiboot.MultibootV1.MemoryMapStart.IsNull)
				return;

			var memoryMapEnd = Multiboot.MultibootV1.MemoryMapStart + (int)Multiboot.MultibootV1.MemoryMapLength;

			var entry = new MultibootV1MemoryMapEntry(Multiboot.MultibootV1.MemoryMapStart);

			while (entry.IsAvailable)
			{
				SetMemoryMap(entry.BaseAddr, entry.Length, entry.Type == 1 ? BootMemoryMapType.Available : BootMemoryMapType.Reserved);

				entry = entry.GetNext(memoryMapEnd);
			}
		}

		public static BootMemoryMapEntry SetMemoryMap(Pointer address, ulong size, BootMemoryMapType type)
		{
			var entry = Map.GetBootMemoryMapEntry(Map.Count);

			entry.StartAddress = address;
			entry.Size = size;
			entry.Type = type;

			Map.Count += 1;

			return entry;
		}

		public static uint GetBootMemoryMapEntryCount()
		{
			return Map.Count;
		}

		public static BootMemoryMapEntry GetBootMemoryMapEntry(uint index)
		{
			return Map.GetBootMemoryMapEntry(index);
		}

		public static Pointer GetMaximumAddress()
		{
			var max = Pointer.Zero;
			var count = Map.Count;

			for (uint i = 0; i < count; i++)
			{
				var entry = GetBootMemoryMapEntry(i);

				var endAddress = entry.EndAddress;

				if (endAddress.GreaterThan(max))
					max = endAddress;
			}

			return max;
		}
	}
}
