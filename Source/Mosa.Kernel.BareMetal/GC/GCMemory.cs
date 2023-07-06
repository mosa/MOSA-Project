// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;
using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.GC;

public static class GCMemory
{
	private static GCHeapList HeapList;
	private static GCHeap CurrentHeap;

	public static void Setup()
	{
		//Debug.WriteLine("GCMemory:Setup()");

		var entry = BootPageAllocator.AllocatePage();
		Page.ClearPage(entry);

		HeapList = new GCHeapList(entry)
		{
			Count = 0
		};

		CurrentHeap = AllocateHeap();

		BootStatus.IsGCEnabled = true;
	}

	public static Pointer AllocateMemory(uint size)
	{
		var heapStart = CurrentHeap.Address;
		var heapSize = CurrentHeap.Size;
		var heapUsed = CurrentHeap.Used;

		Debug.WriteLine("+ Allocating Memory: ", size);

		if (heapStart.IsNull || heapSize - heapUsed < size)
		{
			CurrentHeap = AllocateHeap();
			CurrentHeap.Used = size;
			return CurrentHeap.Address;
		}

		var at = heapStart + heapUsed;
		CurrentHeap.Used = heapUsed + size;
		return at;
	}

	private static GCHeap AllocateHeap()
	{
		var heap = HeapList.GetGCHeapEntry(HeapList.Count);

		var size = 0x1000000u; // 16MB

		heap.Address = VirtualPageAllocator.ReservePages(size / Page.Size);
		heap.Size = size;
		heap.Used = 0;

		return heap;
	}
}
