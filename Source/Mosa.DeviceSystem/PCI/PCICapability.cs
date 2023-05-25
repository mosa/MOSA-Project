// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.PCI;

public class PCICapability
{
	public readonly byte Capability;
	public readonly byte Offset;

	public PCICapability(byte capability, byte offset)
	{
		Capability = capability;
		Offset = offset;
	}
}
