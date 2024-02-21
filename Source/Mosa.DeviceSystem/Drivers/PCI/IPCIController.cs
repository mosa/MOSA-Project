// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceSystem.Drivers.PCI;

/// <summary>
/// An interface used for reading/writing to the PCI configuration space of a PCI device.
/// </summary>
public interface IPCIController
{
	uint ReadConfig32(PCIDevice pciDevice, byte register);

	ushort ReadConfig16(PCIDevice pciDevice, byte register);

	byte ReadConfig8(PCIDevice pciDevice, byte register);

	void WriteConfig32(PCIDevice pciDevice, byte register, uint value);

	void WriteConfig16(PCIDevice pciDevice, byte register, ushort value);

	void WriteConfig8(PCIDevice pciDevice, byte register, byte value);
}
