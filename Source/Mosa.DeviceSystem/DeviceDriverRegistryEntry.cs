// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public delegate IDevice InstantiateDevice(); // legacy

	public delegate DeviceDriverX InstantiateDeviceDriver();

	public abstract class DeviceDriverRegistryEntry
	{
		public virtual PlatformArchitecture Platforms { get; set; }

		public virtual DeviceBusType BusType { get; set; }

		public string Name { get; set; }

		public InstantiateDevice Factory { get; set; } // legacy

		public InstantiateDeviceDriver FactoryX { get; set; }
	}
}
