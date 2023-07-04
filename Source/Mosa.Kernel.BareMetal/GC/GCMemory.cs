// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;
using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.GC;

public static class GCMemory
{
	private static GCHeapList HeapList;
	private static GCHeap CurrentHeap;

	public static void Initialize()
	{
		//Debug.WriteLine("GCMemory:Setup()");

		var entry = BootPageAllocator.AllocatePage();
		Page.ClearPage(entry);

		HeapList = new GCHeapList(entry)
		{
			Count = 0
		};

		var region = Platform.GetInitialGCMemoryPool();

		CurrentHeap = HeapList.GetGCHeapEntry(HeapList.Count);

		CurrentHeap.Address = region.Address;
		CurrentHeap.Size = (uint)region.Size;

		//Debug.WriteLineHex("  > Initial Size: ", region.Size);

		BootStatus.IsGCEnabled = true;
	}

	public static Pointer AllocateMemory(uint size)
	{
		var heapStart = CurrentHeap.Address;
		var heapSize = CurrentHeap.Size;
		var heapUsed = CurrentHeap.Used;

		Debug.WriteLineHex("+ Allocating Memory: ", size);

		if (heapStart.IsNull || heapSize - heapUsed < size)
		{
			//Debug.WriteLineHex("+ Allocated Memory: ", size);

			// TODO - allocate memory for new heap
		}

		Debug.Kill();

		var at = heapStart + heapUsed;
		CurrentHeap.Used = heapUsed + size;
		return at;
	}
}
