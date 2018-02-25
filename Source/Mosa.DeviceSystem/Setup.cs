// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.DeviceSystem;

namespace Mosa.DeviceSystem
{
	public static class Setup
	{
		public static DeviceDriverRegistry DeviceDriverRegistry { get; private set; }
		public static DeviceManager DeviceManager { get; private set; }
		public static InterruptManager InterruptManager { get; private set; }
		public static PCIControllerManager PCIControllerManager { get; private set; }

		public static void Initialize(BaseHardwareAbstraction hardware)
		{
			// Create Device Manager
			DeviceManager = new DeviceManager();

			// Create Interrupt Manager
			InterruptManager = new InterruptManager();

			// Create the Device Driver Manager
			DeviceDriverRegistry = new DeviceDriverRegistry(PlatformArchitecture.X86);

			// Create the PCI Controller Manager
			PCIControllerManager = new PCIControllerManager(DeviceManager);

			// Set device driver system to the hardware HAL
			HAL.SetHardwareAbstraction(hardware);

			// Set the interrupt handler
			HAL.SetInterruptHandler(InterruptManager.ProcessInterrupt);
		}
	}
}
