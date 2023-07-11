// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem;

public delegate BaseDeviceDriver InstantiateDeviceDriver();

public abstract class DeviceDriverRegistryEntry
{
	public virtual PlatformArchitecture Platform { get; set; }

	public virtual DeviceBusType BusType { get; set; }

	public string Name { get; set; }

	public InstantiateDeviceDriver Factory { get; set; }

	public byte IRQ { get; set; }

	public bool AutoStart { get; set; } = true;
}
