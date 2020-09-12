// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.BootMemory
{
	public /*readonly*/ struct BootMemoryMapTable
	{
		private readonly Pointer Entry;

		public BootMemoryMapTable(Pointer entry)
		{
			Entry = entry;
		}

		public bool IsNull => Entry.IsNull;

		public uint Count
		{
			get { return Entry.Load32(); }
			set { Entry.Store32(value); }
		}

		public BootMemoryMapEntry GetBootMemoryMapEntry(uint index)
		{
			return new BootMemoryMapEntry(Entry + sizeof(uint) + (BootMemoryMapEntry.EntrySize * index));
		}
	}
}
