// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.PCI;

/// <summary>
/// An interface used for reading/writing to the PCI configuration space of a PCI device.
/// </summary>
public interface IPCIController
{
	uint ReadConfig32(PCIDeviceConfiguration configuration, byte register);

	ushort ReadConfig16(PCIDeviceConfiguration configuration, byte register);

	byte ReadConfig8(PCIDeviceConfiguration configuration, byte register);

	void WriteConfig32(PCIDeviceConfiguration configuration, byte register, uint value);

	void WriteConfig16(PCIDeviceConfiguration configuration, byte register, ushort value);

	void WriteConfig8(PCIDeviceConfiguration configuration, byte register, byte value);
}
