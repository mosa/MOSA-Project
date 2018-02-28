// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public static class Setup
	{
		public static DeviceManager Initialize(PlatformArchitecture platform, BaseHardwareAbstraction hardware)
		{
			// Create Device Manager
			var DeviceManager = new DeviceManager(platform);

			// Create the PCI Controller Manager
			//PCIControllerManager = new PCIControllerManager(DeviceManager);

			// Set device driver system to the hardware HAL
			HAL.SetHardwareAbstraction(hardware);

			// Set the interrupt handler
			HAL.SetInterruptHandler(DeviceManager.ProcessInterrupt);

			return DeviceManager;
		}
	}
}
