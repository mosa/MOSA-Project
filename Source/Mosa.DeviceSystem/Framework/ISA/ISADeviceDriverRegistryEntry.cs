// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Framework.ISA;

/// <summary>
/// Describes an ISA device entry in the registry of the device driver framework.
/// It is used by Mosa.DeviceDriver, and more specifically the ISABus driver, for initializing and managing specific PCI devices in the
/// kernel.
/// </summary>
public class ISADeviceDriverRegistryEntry : DeviceDriverRegistryEntry
{
	public override DeviceBusType BusType => DeviceBusType.ISA;

	public ushort BasePort { get; set; }

	public ushort PortRange { get; set; }

	public ushort AltBasePort { get; set; }

	public ushort AltPortRange { get; set; }

	public bool AutoLoad { get; set; }

	public string ForceOption { get; set; }

	public uint BaseAddress { get; set; }

	public uint AddressRange { get; set; }
}
