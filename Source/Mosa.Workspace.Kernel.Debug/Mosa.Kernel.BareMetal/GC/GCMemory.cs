// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.GC
{
	public static class GCMemory
	{
		private static GCHeapList HeapList;
		private static GCHeap CurrentHeap;

		public static void Initialize()
		{
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
		}

		public static Pointer AllocateMemory(uint size)
		{
			var heapStart = CurrentHeap.Address;
			var heapSize = CurrentHeap.Size;
			var heapUsed = CurrentHeap.Used;

			if (heapStart.IsNull || (heapSize - heapUsed) < size)
			{
				// TODO - allocate memory for new heap
			}

			var at = heapStart + heapUsed;
			CurrentHeap.Used = heapUsed + size;
			return at;
		}
	}
}
