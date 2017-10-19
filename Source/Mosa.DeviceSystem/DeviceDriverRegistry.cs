// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.DeviceSystem
{
	public class DeviceDriverRegistry
	{
		protected PlatformArchitecture PlatformArchitecture;
		protected readonly List<DeviceDriverRegistryEntry> DeviceDrivers = new List<DeviceDriverRegistryEntry>();

		public DeviceDriverRegistry(PlatformArchitecture platformArchitecture)
		{
			PlatformArchitecture = platformArchitecture;
		}

		public void AddDeviceDriver(DeviceDriverRegistryEntry deviceDriver)
		{
			DeviceDrivers.Add(deviceDriver);
		}

		public List<DeviceDriverRegistryEntry> GetDeviceDrivers(DeviceBusType busType)
		{
			var drivers = new List<DeviceDriverRegistryEntry>();

			foreach (var deviceDriver in DeviceDrivers)
			{
				if (deviceDriver.BusType == busType)
				{
					drivers.Add(deviceDriver);
				}
			}

			return drivers;
		}
	}
}
