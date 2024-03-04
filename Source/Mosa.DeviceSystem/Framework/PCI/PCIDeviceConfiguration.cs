// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Framework.PCI;

/// <summary>
/// Indicates the bus, slot and function of a PCI device before being initialized by the device driver framework.
/// </summary>
public class PCIDeviceConfiguration : BaseDeviceConfiguration
{
	public byte Bus { get; set; }

	public byte Slot { get; set; }

	public byte Function { get; set; }
}
