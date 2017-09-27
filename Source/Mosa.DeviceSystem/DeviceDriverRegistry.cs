// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.DeviceSystem
{
	public class DeviceDriverRegistry
	{
		protected PlatformArchitecture PlatformArchitecture;
		protected List<IDeviceDriver> DeviceDrivers;

		public DeviceDriverRegistry(PlatformArchitecture platformArchitecture)
		{
			this.PlatformArchitecture = platformArchitecture;
			DeviceDrivers = new List<IDeviceDriver>();
		}

		public void AddDeviceDriver(IDeviceDriver deviceDriver)
		{
			DeviceDrivers.Add(deviceDriver);
		}

		public List<IDeviceDriver> GetDeviceDrivers(DeviceBusType busType)
		{
			var drivers = new List<IDeviceDriver>();

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
