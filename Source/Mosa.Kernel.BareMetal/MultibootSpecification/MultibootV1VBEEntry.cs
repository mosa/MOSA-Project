// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.MultibootSpecification;

/// <summary>
/// Multiboot V1 VBE Entry
/// </summary>
public struct MultibootV1VBEEntry
{
	private readonly Pointer Entry;

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
	public readonly bool IsAvailable => !Entry.IsNull;

	/// <summary>
	/// Setup Multiboot V1 VBE Entry.
	/// </summary>
	public MultibootV1VBEEntry(Pointer entry) => Entry = entry;

	public ushort Attributes => Entry.Load16(VBEModeInfoOffset.Attributes);

	public byte WindowA => Entry.Load8(VBEModeInfoOffset.WindowA);

	public byte WindowB => Entry.Load8(VBEModeInfoOffset.WindowB);

	public ushort Granularity => Entry.Load16(VBEModeInfoOffset.Granularity);

	public ushort WindowSize => Entry.Load16(VBEModeInfoOffset.WindowSize);

	public ushort SegmentA => Entry.Load16(VBEModeInfoOffset.SegmentA);

	public ushort SegmentB => Entry.Load16(VBEModeInfoOffset.SegmentB);

	public uint WinFuncPtr => Entry.Load32(VBEModeInfoOffset.WinFuncPtr);

	public ushort Pitch => Entry.Load16(VBEModeInfoOffset.Pitch);

	public ushort ScreenWidth => Entry.Load16(VBEModeInfoOffset.ScreenWidth);

	public ushort ScreenHeight => Entry.Load16(VBEModeInfoOffset.ScreenHeight);

	public byte WChar => Entry.Load8(VBEModeInfoOffset.WChar);

	public byte YChar => Entry.Load8(VBEModeInfoOffset.YChar);

	public byte Planes => Entry.Load8(VBEModeInfoOffset.Planes);

	public byte BitsPerPixel => Entry.Load8(VBEModeInfoOffset.BitsPerPixel);

	public byte Banks => Entry.Load8(VBEModeInfoOffset.Banks);

	public byte MemoryModel => Entry.Load8(VBEModeInfoOffset.MemoryModel);

	public byte BankSize => Entry.Load8(VBEModeInfoOffset.BankSize);

	public byte ImagePages => Entry.Load8(VBEModeInfoOffset.ImagePages);

	public byte Reserved0 => Entry.Load8(VBEModeInfoOffset.Reserved0);

	public byte RedMask => Entry.Load8(VBEModeInfoOffset.RedMask);

	public byte RedPosition => Entry.Load8(VBEModeInfoOffset.RedPosition);

	public byte GreenMask => Entry.Load8(VBEModeInfoOffset.GreenMask);

	public byte GreenPosition => Entry.Load8(VBEModeInfoOffset.GreenPosition);

	public byte BlueMask => Entry.Load8(VBEModeInfoOffset.BlueMask);

	public byte BluePosition => Entry.Load8(VBEModeInfoOffset.BluePosition);

	public byte ReservedMask => Entry.Load8(VBEModeInfoOffset.ReservedMask);

	public byte ReservedPosition => Entry.Load8(VBEModeInfoOffset.ReservedPosition);

	public byte DirectColorAttributes => Entry.Load8(VBEModeInfoOffset.DirectColorAttributes);

	public Pointer MemoryPhysicalLocation => Entry.LoadPointer(VBEModeInfoOffset.PhysBase);

	public uint OffScreenMemoryOff => Entry.Load32(VBEModeInfoOffset.OffScreenMemoryOff);

	public ushort OffScreenMemorSize => Entry.Load16(VBEModeInfoOffset.OffScreenMemorSize);
}
