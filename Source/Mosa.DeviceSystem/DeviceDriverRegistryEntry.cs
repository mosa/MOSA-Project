// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public delegate DeviceDriver InstantiateDeviceDriver();

	public abstract class DeviceDriverRegistryEntry
	{
		public virtual PlatformArchitecture Platforms { get; set; }

		public virtual DeviceBusType BusType { get; set; }

		public string Name { get; set; }

		public InstantiateDeviceDriver Factory { get; set; }
	}
}
