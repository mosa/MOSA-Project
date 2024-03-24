// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ACPI;

/// <summary>
/// The ACPI Generic Address Structure. It describes in which address space a register is located (e.g. MMIO, Port IO, PCI configuration
/// space, etc...)
/// </summary>
public readonly struct GenericAddressStructure
{
	public readonly Pointer Pointer;

	public const uint Size = Offset.Size;

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

	public byte AddressSpace => Pointer.Load8(Offset.AddressSpace);

	public byte BitWidth => Pointer.Load8(Offset.BitWidth);

	public byte BitOffset => Pointer.Load8(Offset.BitOffset);

	public byte AccessSize => Pointer.Load8(Offset.AccessSize);

	public ulong Address => Pointer.Load32(Offset.Address);
}
