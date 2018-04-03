// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public static class Setup
	{
		public static DeviceManager Initialize(PlatformArchitecture platform, BaseHardwareAbstraction hardware)
		{
			// Create Device Manager
			var deviceManager = new DeviceManager(platform);

			deviceManager.RegisterDaemon(new DiskDeviceMountDeamon());

			// Set device driver system to the hardware HAL
			HAL.SetHardwareAbstraction(hardware);

			// Set the interrupt handler
			HAL.SetInterruptHandler(deviceManager.ProcessInterrupt);

			return deviceManager;
		}
	}
}
