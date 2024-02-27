// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Framework.ISA;
using Mosa.DeviceSystem.Framework.PCI;
using Mosa.DeviceSystem.Misc;

namespace Mosa.DeviceSystem.Framework;

public delegate BaseDeviceDriver InstantiateDeviceDriver();

/// <summary>
/// Describes a generic entry in the registry of the device driver framework. See <see cref="PCIDeviceDriverRegistryEntry"/> and
/// <see cref="ISADeviceDriverRegistryEntry"/> for implementations of this class.
/// </summary>
public abstract class DeviceDriverRegistryEntry
{
	public virtual PlatformArchitecture Platform { get; set; }

	public virtual DeviceBusType BusType { get; set; }

	public string Name { get; set; }

	/// <summary>
	/// A delegate describing an instantiated device driver. It is executed by the device driver framework when initializing
	/// the device driver (essentially, it is an instantiation of the device driver itself).
	/// </summary>
	public InstantiateDeviceDriver Factory { get; set; }

	public byte IRQ { get; set; }

	public bool AutoStart { get; set; } = true;
}
