// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.GC
{
	public /*readonly*/ struct GCHeap
	{
		private readonly Pointer Entry;

		public GCHeap(Pointer entry)
		{
			Entry = entry;
		}

		public Pointer StartAddress
		{
			get { return Entry.LoadPointer(); }
			set { Entry.StorePointer(value); }
		}

		public ulong Size
		{
			get { return Entry.Load64(Pointer.Size); }
			set { Entry.Store64(Pointer.Size, value); }
		}

		public Pointer EndAddress
		{
			get { return StartAddress + Size; }
		}
	}
}
