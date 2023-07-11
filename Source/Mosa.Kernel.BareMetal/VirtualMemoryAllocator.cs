// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.Kernel.BareMetal.VirtualMemory;

namespace Mosa.Kernel.BareMetal;

public static class VirtualMemoryAllocator
{
	private const uint DefaultHeapSize = 0x1000000u; // 16MB

	private static MemoryHeapList HeapList;
	private static MemoryHeap CurrentHeap;

	#region Public API

	public static void Setup()
	{
		Debug.WriteLine("VirtualMemoryAllocator:Setup()");

		var entry = BootPageAllocator.AllocatePage();
		Page.ClearPage(entry);

		HeapList = new MemoryHeapList(entry)
		{
			Count = 0
		};

		CurrentHeap = AllocateHeap();

		Debug.WriteLine("VirtualMemoryAllocator:Setup() [Exit]");
	}

	public static ConstrainedPointer AllocateMemory(uint size)
	{
		var heapStart = CurrentHeap.Address;
		var heapSize = CurrentHeap.Size;
		var heapUsed = CurrentHeap.Used;

		var headerSize = 4u; // size of 32bit value

		if (heapStart.IsNull || heapSize - heapUsed < size + headerSize)
		{
			CurrentHeap = AllocateHeap();
			heapStart = CurrentHeap.Address;
			heapUsed = 0;
		}

		var at = heapStart + heapUsed;

		at.Store32(0, size); // store size before allocation

		at += headerSize;

		Debug.WriteLine("+ Allocating Memory: size = ", size, " @ ", new Hex(at));

		CurrentHeap.Used = heapUsed + size + headerSize;

		return new ConstrainedPointer(at, size);
	}

	#endregion Public API

	private static MemoryHeap AllocateHeap()
	{
		var heap = HeapList.GetHeapEntry(HeapList.Count);

		var size = DefaultHeapSize;

		heap.Address = VirtualPageAllocator.ReservePages(DefaultHeapSize / Page.Size);
		heap.Size = size;
		heap.Used = 0;

		Debug.WriteLine("+ Allocated New Heap: size = ", new Hex(size), " @ ", new Hex(heap.Address));

		return heap;
	}
}
