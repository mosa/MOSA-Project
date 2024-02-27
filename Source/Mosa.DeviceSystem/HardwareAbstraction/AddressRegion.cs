// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceSystem.HardwareAbstraction;

/// <summary>
/// Describes a sized region in memory.
/// </summary>
public struct AddressRegion
{
	public Pointer Address { get; }

	public uint Size { get; }

	public AddressRegion(Pointer address, uint size)
	{
		Address = address;
		Size = size;
	}

	public bool Contains(Pointer address) => address >= Address && address < Address + Size;
}
