// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.DeviceSystem.Misc;

namespace Mosa.DeviceSystem.HardwareAbstraction;

/// <summary>
/// Contains the IRQ and a list of port-mapped I/O and memory regions of a device. It is used by the device driver implementation for
/// retrieving e.g. BARs (for PCI devices) and I/O ports (for ISA devices).
/// </summary>
public sealed class HardwareResources
{
	public List<AddressRegion> AddressRegions { get; }

	public List<IOPortRegion> IOPortRegions { get; }

	public byte IRQ { get; }

	public HardwareResources(List<IOPortRegion> ioPortRegions, List<AddressRegion> addressRegions, byte irq = 0)
	{
		IOPortRegions = ioPortRegions;
		AddressRegions = addressRegions;
		IRQ = irq;
	}

	public IOPortReadWrite GetIOPortReadWrite(byte index, ushort offset) =>
		new IOPortReadWrite((ushort)(IOPortRegions[index].BaseIOPort + offset));

	public IOPortRead GetIOPortRead(byte index, ushort offset) =>
		new IOPortRead((ushort)(IOPortRegions[index].BaseIOPort + offset));

	public IOPortWrite GetIOPortWrite(byte index, ushort offset) =>
		new IOPortWrite((ushort)(IOPortRegions[index].BaseIOPort + offset));

	public ConstrainedPointer GetMemory(byte region) =>
		HAL.GetPhysicalMemory(AddressRegions[region].Address, AddressRegions[region].Size);
}
