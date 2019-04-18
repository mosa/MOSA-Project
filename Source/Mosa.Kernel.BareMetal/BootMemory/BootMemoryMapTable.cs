// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.Extension;
using System;

namespace Mosa.Kernel.BareMetal.BootMemory
{
	public readonly struct BootMemoryMapTable
	{
		private readonly IntPtr Entry;

		public BootMemoryMapTable(IntPtr entry)
		{
			Entry = entry;
		}

		public bool IsNull => Entry == IntPtr.Zero;

		public uint Count
		{
			get { return Entry.Load32(IntPtr.Size); }
			set { Entry.Store32(IntPtr.Size, value); }
		}

		public BootMemoryMapEntry GetBootMemoryMapEntry(uint index)
		{
			var offset = sizeof(int) + (BootMemoryMapEntry.EntrySize * index);
			return new BootMemoryMapEntry(Entry + (int)offset);
		}
	}
}
