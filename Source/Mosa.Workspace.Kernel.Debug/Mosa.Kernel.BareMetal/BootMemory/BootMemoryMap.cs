// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.MultibootSpecification;
using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.BootMemory
{
	public static class BootMemoryMap
	{
		private static BootMemoryMapTable Map;

		private static Pointer AvailableMemory;

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

			AvailableMemory = new Pointer((Multiboot.MultibootV1.MemoryUpper * 1024) + (1024 * 1024));  // assuming all of lower memory

			var memoryMapEnd = Multiboot.MultibootV1.MemoryMapStart + Multiboot.MultibootV1.MemoryMapLength;

			var entry = new MultibootV1MemoryMapEntry(Multiboot.MultibootV1.MemoryMapStart);

			while (entry.IsAvailable)
			{
				SetMemoryMap(entry.BaseAddr, entry.Length, entry.Type == 1 ? BootMemoryMapType.Available : BootMemoryMapType.Reserved);

				entry = entry.GetNext(memoryMapEnd);
			}
		}

		public static void ImportPlatformMemoryMap()
		{
			SetMemoryMap(Platform.GetBootReservedRegion(), BootMemoryMapType.Kernel);
			SetMemoryMap(Platform.GetInitialGCMemoryPool(), BootMemoryMapType.Kernel);

			for (int slot = 0; ; slot++)
			{
				var region = Platform.GetPlatformReservedMemory(slot);

				if (region.Size == 0)
					break;

				SetMemoryMap(region, BootMemoryMapType.Kernel);
			}
		}

		public static BootMemoryMapEntry SetMemoryMap(AddressRange range, BootMemoryMapType type)
		{
			return SetMemoryMap(range.Address, range.Size, type);
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

		public static Pointer GetAvailableMemory()
		{
			return AvailableMemory;
		}
	}
}
