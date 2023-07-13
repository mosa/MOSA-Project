// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;
using Mosa.Runtime;

// Portions of this code are from Cosmos

//https://wiki.osdev.org/ACPI
//https://wiki.osdev.org/MADT

namespace Mosa.DeviceDriver.ISA.ACPI;

public struct GenericAddressStructure
{
	private readonly Pointer Entry;

	public readonly Pointer Pointer => Entry;

	public readonly uint Size = Offset.Size;

	internal static class Offset
	{
		public const int AddressSpace = 0;
		public const int BitWidth = AddressSpace + 1;
		public const int BitOffset = BitWidth + 1;
		public const int AccessSize = BitOffset + 1;
		public const int Address = AccessSize + 1;
		public const int Size = Address + 8;
	}

	public GenericAddressStructure(Pointer entry) => Entry = entry;

	public readonly byte AddressSpace => Entry.Load8(Offset.AddressSpace);

	public readonly byte BitWidth => Entry.Load8(Offset.BitWidth);

	public readonly byte BitOffset => Entry.Load8(Offset.BitOffset);

	public readonly byte AccessSize => Entry.Load8(Offset.AccessSize);

	public readonly ulong Address => Entry.Load32(Offset.Address);
}
