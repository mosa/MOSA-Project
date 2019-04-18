// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.Extension;
using System;

namespace Mosa.Kernel.BareMetal.BootMemory
{
	public readonly struct BootMemoryMapEntry
	{
		private readonly IntPtr Entry;

		public BootMemoryMapEntry(IntPtr entry)
		{
			Entry = entry;
		}

		public bool IsNull => Entry == IntPtr.Zero;

		public IntPtr Address
		{
			get { return Entry.LoadPointer(); }
			set { Entry.StorePointer(value); }
		}

		public ulong Size
		{
			get { return Entry.Load64(IntPtr.Size); }
			set { Entry.Store64(IntPtr.Size, value); }
		}

		public BootMemoryMapType Type
		{
			get { return (BootMemoryMapType)Entry.Load8(IntPtr.Size + sizeof(ulong)); }
			set { Entry.Store8(IntPtr.Size, (byte)value); }
		}

		public bool Valid
		{
			get { return Entry.Load8(IntPtr.Size + sizeof(ulong)) == 1; }
			set { Entry.Store8(IntPtr.Size, (byte)(value ? 1 : 0)); }
		}

		public static uint EntrySize = (uint)IntPtr.Size + sizeof(ulong) + (sizeof(byte) * 2);
	}
}
