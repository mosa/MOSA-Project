// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.MultibootSpecification;
using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.BootMemory;

public static class BootMemoryMap
{
	#region Private Members

	private static BootMemoryList List;

	private static Pointer AvailableMemory;

	#endregion Private Members

	#region Public API

	public static void Setup()
	{
		var entry = BootPageAllocator.AllocatePage();
		Page.ClearPage(entry);

		List = new BootMemoryList(entry)
		{
			Count = 0
		};

		ImportMultibootV1MemoryMap();
		ImportPlatformMemoryMap();
	}

	public static BootMemoryMapEntry SetMemoryMap(AddressRange range, BootMemoryType type)
	{
		return SetMemoryMap(range.Address, range.Size, type);
	}

	public static BootMemoryMapEntry SetMemoryMap(Pointer address, ulong size, BootMemoryType type)
	{
		var entry = List.GetBootMemoryMapEntry(List.Count);

		entry.StartAddress = address;
		entry.Size = size;
		entry.Type = type;

		List.Count++;

		return entry;
	}

	public static uint GetBootMemoryMapEntryCount() => List.Count;

	public static BootMemoryMapEntry GetBootMemoryMapEntry(uint index)
	{
		return List.GetBootMemoryMapEntry(index);
	}

	public static Pointer GetAvailableMemory()
	{
		return AvailableMemory;
	}

	#endregion Public API

	#region Private API

	private static void ImportMultibootV1MemoryMap()
	{
		Debug.WriteLine("BootMemoryMap::ImportMultibootV1MemoryMap()");

		if (!Multiboot.IsAvailable)
			return;

		if (Multiboot.MultibootV1.MemoryMapStart.IsNull)
			return;

		AvailableMemory = new Pointer(Multiboot.MultibootV1.MemoryUpper * 1024);

		Debug.WriteLine(" > Available Memory: ", AvailableMemory.ToInt64());

		var memoryMapEnd = Multiboot.MultibootV1.MemoryMapStart + Multiboot.MultibootV1.MemoryMapLength;

		var entry = new MultibootV1MemoryMapEntry(Multiboot.MultibootV1.MemoryMapStart);

		while (entry.Entry < memoryMapEnd)
		{
			SetMemoryMap(entry.BaseAddr, entry.Length, entry.Type == 1 ? BootMemoryType.Available : BootMemoryType.Reserved);

			entry = entry.GetNext();
		}
	}

	private static void ImportPlatformMemoryMap()
	{
		SetMemoryMap(Platform.GetBootReservedRegion(), BootMemoryType.Kernel);
		SetMemoryMap(Platform.GetInitialGCMemoryPool(), BootMemoryType.Kernel);
	}

	#endregion Private API

	#region Diagnostic

	public static void Dump()
	{
		Debug.WriteLine();
		Debug.WriteLine("BootMemoryMap - Dump:");
		Debug.WriteLine("=====================");
		Debug.Write("Entries: ");
		Debug.Write(List.Count);
		Debug.WriteLine();

		for (var slot = 0u; slot < List.Count; slot++)
		{
			var entry = GetBootMemoryMapEntry(slot);

			Debug.Write("Start: 0x");
			Debug.Write(new Hex8(entry.StartAddress));
			Debug.Write(" Size: 0x");
			Debug.Write(new Hex8(entry.Size));
			Debug.Write(" Type: ");
			Debug.Write((ulong)(byte)entry.Type);
			Debug.WriteLine();
		}

		Debug.WriteLine();

		#endregion Diagnostic
	}
}
