// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.Extension;
using Mosa.Runtime.Extension;
using System;

namespace Mosa.Kernel.BareMetal.MultibootSpecification
{
	/// <summary>
	/// Multiboot V1 Memory Map
	/// </summary>
	public readonly struct MultibootV1MemoryMapEntry
	{
		private readonly IntPtr Entry;

		#region Multiboot Memory Map Offsets

		private struct MultiBootMemoryMapOffset
		{
			public const byte Size = 0;
			public const byte BaseAddr = 4;
			public const byte Length = 12;
			public const byte Type = 20;
			public const byte Next = 24;
		}

		#endregion Multiboot Memory Map Offsets

		/// <summary>
		/// Gets a value indicating whether Multiboot V1 Memory Map is available.
		/// </summary>
		public bool IsAvailable => !Entry.IsNull();

		/// <summary>
		/// Setup Multiboot V1 Memory Map Entry.
		/// </summary>
		public MultibootV1MemoryMapEntry(IntPtr entry)
		{
			Entry = entry;
		}

		public uint Size { get { return Entry.Load32(MultiBootMemoryMapOffset.Size); } }

		public IntPtr BaseAddr { get { return Entry.LoadPointer(MultiBootMemoryMapOffset.BaseAddr); } }

		public uint Length { get { return Entry.Load32(MultiBootMemoryMapOffset.Length); } }

		public byte Type { get { return Entry.Load8(MultiBootMemoryMapOffset.Type); } }

		public byte Next { get { return Entry.Load8(MultiBootMemoryMapOffset.Next); } }

		public MultibootV1MemoryMapEntry GetNext(IntPtr memoryMapEnd)
		{
			var next = Entry + Next + sizeof(int);

			if (!next.LessThan(memoryMapEnd))
			{
				next = IntPtr.Zero;
			}

			return new MultibootV1MemoryMapEntry(next);
		}
	}
}
