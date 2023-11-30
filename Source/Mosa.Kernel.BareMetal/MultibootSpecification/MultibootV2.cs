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

	private uint MemoryMapSize => GetEntryValue32(6, 4);

	public uint MemoryMapEntrySize => GetEntryValue32(6, 8);

	public uint MemoryMapEntryVersion => GetEntryValue32(6, 12);

	public MultibootV2MemoryMapEntry MemoryMapStart => new(GetStructureEntryPointer(6, 16));

	public uint MemoryMapEntries
	{
		get
		{
			if (MemoryMapEntrySize == 0)
				return 0;

			return (MemoryMapSize - 16) / MemoryMapEntrySize;
		}
	}

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

	private readonly Pointer GetStructurePointer(int type)
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

	private readonly Pointer GetStructureEntryPointer(int type, int offset)
	{
		var entry = GetStructurePointer(type);

		if (entry.IsNull)
			return Pointer.Zero;

		return entry + offset;
	}

	private readonly uint GetEntryValue32(int type, int offset)
	{
		var entry = GetStructureEntryPointer(type, offset);

		if (entry.IsNull)
			return 0;

		return entry.Load32();
	}

	private readonly byte GetEntryValue8(int type, int offset)
	{
		var entry = GetStructureEntryPointer(type, offset);

		if (entry.IsNull)
			return 0;

		return entry.Load8();
	}

	private readonly Pointer GetEntryValuePointer(int type, int offset)
	{
		var entry = GetStructureEntryPointer(type, offset);

		if (entry.IsNull)
			return Pointer.Zero;

		return entry;
	}
}
