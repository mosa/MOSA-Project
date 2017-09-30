// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
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
