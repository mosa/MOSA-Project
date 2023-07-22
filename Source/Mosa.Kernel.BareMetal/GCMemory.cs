// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.GC;
using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

public static class GCMemory
{
	private static uint DefaultHeapSize = 0x1000000u; // 16MB

	private static GCHeapList HeapList;
	private static GCHeap CurrentHeap;

	public static void Setup()
	{
		Debug.WriteLine("GCMemory:Setup()");

		var entry = BootPageAllocator.AllocatePage();
		Page.ClearPage(entry);

		HeapList = new GCHeapList(entry)
		{
			Count = 0
		};

		CurrentHeap = AllocateHeap();

		BootStatus.IsGCEnabled = true;

		Debug.WriteLine("GCMemory:Setup() [Exit]");
	}

	public static Pointer AllocateMemory(uint size)
	{
		var heapStart = CurrentHeap.Address;
		var heapSize = CurrentHeap.Size;
		var heapUsed = CurrentHeap.Used;

		if (heapStart.IsNull || heapSize - heapUsed < size)
		{
			CurrentHeap = AllocateHeap();
			CurrentHeap.Used = size;

			Debug.WriteLine("+ Allocating Object: size = ", size, " @ ", new Hex(CurrentHeap.Address));

			return CurrentHeap.Address;
		}

		var at = heapStart + heapUsed;

		Debug.WriteLine("+ Allocating Object: size = ", size, " @ ", new Hex(at));

		CurrentHeap.Used = heapUsed + size;
		return at;
	}

	private static GCHeap AllocateHeap()
	{
		var heap = HeapList.GetHeapEntry(HeapList.Count);

		var size = DefaultHeapSize;

		heap.Address = VirtualPageAllocator.ReservePages(size / Page.Size);
		heap.Size = size;
		heap.Used = 0;

		Debug.WriteLine("+ Allocated New Heap: size = ", new Hex(size), " @ ", new Hex(heap.Address));

		return heap;
	}
}
