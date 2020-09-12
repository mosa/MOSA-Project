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
		}
	}
}
