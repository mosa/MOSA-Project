// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

public struct AddressRange
{
	public Pointer Address { get; }

	public ulong Size { get; }

	public AddressRange(Pointer address, uint size)
	{
		Address = address;
		Size = size;
	}

	public AddressRange(uint address, uint size)
	{
		Address = new Pointer(address);
		Size = size;
	}

	public AddressRange(ulong address, uint size)
	{
		Address = new Pointer((long)address);
		Size = size;
	}

	public AddressRange(ulong address, ulong size)
	{
		Address = new Pointer((long)address);
		Size = size;
	}
}
