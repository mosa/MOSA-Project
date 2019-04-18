// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.Extension;
using Mosa.Runtime.Extension;
using System;

namespace Mosa.Kernel.BareMetal.MultibootSpecification
{
	/// <summary>
	/// Multiboot V1 VBE Entry
	/// </summary>
	public readonly struct MultibootV1VBEEntry
	{
		private readonly IntPtr Entry;

		#region VBE Mode Info Offsets

		internal struct VBEModeInfoOffset
		{
			public const byte Attributes = 0;
			public const byte WindowA = 2;
			public const byte WindowB = 3;
			public const byte Granularity = 4;
			public const byte WindowSize = 6;
			public const byte SegmentA = 8;
			public const byte SegmentB = 10;
			public const byte WinFuncPtr = 12;
			public const byte Pitch = 16;
			public const byte ScreenWidth = 18;
			public const byte ScreenHeight = 20;
			public const byte WChar = 22;
			public const byte YChar = 23;
			public const byte Planes = 24;
			public const byte BitsPerPixel = 25;
			public const byte Banks = 26;
			public const byte MemoryModel = 27;
			public const byte BankSize = 28;
			public const byte ImagePages = 29;
			public const byte Reserved0 = 30;
			public const byte RedMask = 31;
			public const byte RedPosition = 32;
			public const byte GreenMask = 33;
			public const byte GreenPosition = 34;
			public const byte BlueMask = 35;
			public const byte BluePosition = 36;
			public const byte ReservedMask = 37;
			public const byte ReservedPosition = 38;
			public const byte DirectColorAttributes = 39;
			public const byte PhysBase = 40;
			public const byte OffScreenMemoryOff = 44;
			public const byte OffScreenMemorSize = 48;
			public const byte Reserved1 = 50;
		}

		#endregion VBE Mode Info Offsets

		/// <summary>
		/// Gets a value indicating whether VBE is available.
		/// </summary>
		public bool IsAvailable => !Entry.IsNull();

		/// <summary>
		/// Setup Multiboot V1 VBE Entry.
		/// </summary>
		public MultibootV1VBEEntry(IntPtr entry)
		{
			Entry = entry;
		}

		public ushort Attributes { get { return Entry.Load16(VBEModeInfoOffset.Attributes); } }

		public byte WindowA { get { return Entry.Load8(VBEModeInfoOffset.WindowA); } }

		public byte WindowB { get { return Entry.Load8(VBEModeInfoOffset.WindowB); } }

		public ushort Granularity { get { return Entry.Load16(VBEModeInfoOffset.Granularity); } }

		public ushort WindowSize { get { return Entry.Load16(VBEModeInfoOffset.WindowSize); } }

		public ushort SegmentA { get { return Entry.Load16(VBEModeInfoOffset.SegmentA); } }

		public ushort SegmentB { get { return Entry.Load16(VBEModeInfoOffset.SegmentB); } }

		public uint WinFuncPtr { get { return Entry.Load32(VBEModeInfoOffset.WinFuncPtr); } }

		public ushort Pitch { get { return Entry.Load16(VBEModeInfoOffset.Pitch); } }

		public ushort ScreenWidth { get { return Entry.Load16(VBEModeInfoOffset.ScreenWidth); } }

		public ushort ScreenHeight { get { return Entry.Load16(VBEModeInfoOffset.ScreenHeight); } }

		public byte WChar { get { return Entry.Load8(VBEModeInfoOffset.WChar); } }

		public byte YChar { get { return Entry.Load8(VBEModeInfoOffset.YChar); } }

		public byte Planes { get { return Entry.Load8(VBEModeInfoOffset.Planes); } }

		public byte BitsPerPixel { get { return Entry.Load8(VBEModeInfoOffset.BitsPerPixel); } }

		public byte Banks { get { return Entry.Load8(VBEModeInfoOffset.Banks); } }

		public byte MemoryModel { get { return Entry.Load8(VBEModeInfoOffset.MemoryModel); } }

		public byte BankSize { get { return Entry.Load8(VBEModeInfoOffset.BankSize); } }

		public byte ImagePages { get { return Entry.Load8(VBEModeInfoOffset.ImagePages); } }

		public byte Reserved0 { get { return Entry.Load8(VBEModeInfoOffset.Reserved0); } }

		public byte RedMask { get { return Entry.Load8(VBEModeInfoOffset.RedMask); } }

		public byte RedPosition { get { return Entry.Load8(VBEModeInfoOffset.RedPosition); } }

		public byte GreenMask { get { return Entry.Load8(VBEModeInfoOffset.GreenMask); } }

		public byte GreenPosition { get { return Entry.Load8(VBEModeInfoOffset.GreenPosition); } }

		public byte BlueMask { get { return Entry.Load8(VBEModeInfoOffset.BlueMask); } }

		public byte BluePosition { get { return Entry.Load8(VBEModeInfoOffset.BluePosition); } }

		public byte ReservedMask { get { return Entry.Load8(VBEModeInfoOffset.ReservedMask); } }

		public byte ReservedPosition { get { return Entry.Load8(VBEModeInfoOffset.ReservedPosition); } }

		public byte DirectColorAttributes { get { return Entry.Load8(VBEModeInfoOffset.DirectColorAttributes); } }

		public IntPtr MemoryPhysicalLocation { get { return Entry.LoadPointer(VBEModeInfoOffset.PhysBase); } }

		public uint OffScreenMemoryOff { get { return Entry.Load32(VBEModeInfoOffset.OffScreenMemoryOff); } }

		public ushort OffScreenMemorSize { get { return Entry.Load16(VBEModeInfoOffset.OffScreenMemorSize); } }
	}
}
