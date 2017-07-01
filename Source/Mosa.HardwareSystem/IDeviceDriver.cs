// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.HardwareSystem
{
	public delegate IDevice InstantiateDevice();

	public interface IDeviceDriver
	{
		PlatformArchitecture Platforms { get; }

		DeviceBusType BusType { get; }

		string Name { get; }

		InstantiateDevice Factory { get; }
	}
}
