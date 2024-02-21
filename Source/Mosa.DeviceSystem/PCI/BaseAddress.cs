// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceSystem.PCI;

/// <summary>
/// Describes a PCI BAR.
/// </summary>
public class BaseAddress
{
	public AddressType Region { get; }

	public Pointer Address { get; }

	public uint Size { get; }

	public bool Prefetchable { get; }

	public BaseAddress(AddressType region, Pointer address, uint size, bool prefetchable)
	{
		Region = region;
		Address = address;
		Size = size;
		Prefetchable = prefetchable;
	}

	public override string ToString()
	{
		switch (Region)
		{
			case AddressType.IO: return "I/O Port at 0x" + Address.ToUInt32().ToString("X") + " [size=" + Size + "]";
			case AddressType.Memory:
				{
					if (Prefetchable) return "Memory at 0x" + Address.ToUInt32().ToString("X") + " [size=" + Size + "] (prefetchable)";
					return "Memory at 0x" + Address.ToUInt32().ToString("X") + " [size=" + Size + "] (non-prefetchable)";
				}
			case AddressType.Undefined:
			default: return string.Empty;
		}
	}
}
