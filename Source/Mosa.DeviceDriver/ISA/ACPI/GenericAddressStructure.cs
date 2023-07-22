// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

// Portions of this code are from Cosmos

//https://wiki.osdev.org/ACPI
//https://wiki.osdev.org/MADT

namespace Mosa.DeviceDriver.ISA.ACPI;

public struct GenericAddressStructure
{
	public readonly Pointer Pointer;

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

	public GenericAddressStructure(Pointer entry) => Pointer = entry;

	public readonly byte AddressSpace => Pointer.Load8(Offset.AddressSpace);

	public readonly byte BitWidth => Pointer.Load8(Offset.BitWidth);

	public readonly byte BitOffset => Pointer.Load8(Offset.BitOffset);

	public readonly byte AccessSize => Pointer.Load8(Offset.AccessSize);

	public readonly ulong Address => Pointer.Load32(Offset.Address);
}
