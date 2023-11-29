// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.MultibootSpecification;

public struct MultibootV2
{
	public const uint Magic = 0x36D76289;

	private readonly Pointer Pointer;

	public readonly bool IsAvailable => !Pointer.IsNull;

	public Pointer CommandLine => GetEntryValuePointer(1, 12);

	public Pointer BootloaderName => GetEntryValuePointer(2, 8);

	public uint MemoryLower => GetEntryValue32(4, 8);

	public uint MemoryUpper => GetEntryValue32(4, 12);

	public uint EntrySize => GetEntryValue32(6, 8);

	public uint EntryVersion => GetEntryValue32(6, 12);

	public uint Entries
	{
		get
		{
			var size = GetEntryValue32(6, 4);

			if (size == 0)
				return 0;

			return (size - 16) / EntrySize;
		}
	}

	public MultibootV2MemoryMapEntry FirstEntry => new(GetEntryValuePointer(6, 16));

	public Pointer FrameBuffer => GetEntryValuePointer(8, 8);

	public uint FrameBufferPitch => GetEntryValue32(8, 12);

	public uint FrameBufferWidth => GetEntryValue32(8, 16);

	public uint FrameBufferHeight => GetEntryValue32(8, 20);

	public byte FrameBufferBitPerPixel => GetEntryValue8(8, 24);

	public byte FrameBufferType => GetEntryValue8(8, 25);

	public Pointer RSDPv1 => GetEntryValuePointer(14, 8);

	public Pointer RSDPv2 => GetEntryValuePointer(15, 8);

	public MultibootV2(Pointer entry)
	{
		Pointer = entry;
	}

	private Pointer GetStructurePointer(int type)
	{
		for (var at = Pointer + 8; ;)
		{
			var entryType = at.Load32();

			if (entryType == 0)
				return Pointer.Zero;

			if (entryType == type)
				return at;

			var size = at.Load32(4);

			at += (size + 7) & ~7;
		}
	}

	private Pointer GetStructureEntryPointer(int type, int offset)
	{
		var entry = GetStructurePointer(type);

		if (entry.IsNull)
			return Pointer.Zero;

		return entry + offset;
	}

	private uint GetEntryValue32(int type, int offset)
	{
		var entry = GetStructureEntryPointer(type, offset);

		if (entry.IsNull)
			return 0;

		return entry.Load32();
	}

	private byte GetEntryValue8(int type, int offset)
	{
		var entry = GetStructureEntryPointer(type, offset);

		if (entry.IsNull)
			return 0;

		return entry.Load8();
	}

	private Pointer GetEntryValuePointer(int type, int offset)
	{
		var entry = GetStructureEntryPointer(type, offset);

		if (entry.IsNull)
			return Pointer.Zero;

		return entry.LoadPointer();
	}
}
