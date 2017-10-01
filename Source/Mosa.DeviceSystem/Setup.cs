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

		public static void Start()
		{
			StartISADevices();
		}

		public static void StartISADevices()
		{
			// Start ISA Drivers
			var drivers = DeviceDriverRegistry.GetDeviceDrivers(DeviceBusType.ISA);

			foreach (var driver in drivers)
			{
				if (driver is ISADeviceDriver)
				{
					StartISADevice(driver as ISADeviceDriver);
				}
			}
		}

		static public void StartPCIDevices()
		{
			PCIControllerManager.CreatePCIDevices();

			//var devices = deviceManager.GetDevices(new IsPCIDevice(), new IsAvailable());

			//foreach (var device in devices)
			//{
			//	var pciDevice = device as IPCIDevice;
			//	StartDevice(pciDevice);
			//}
		}

		public static void StartISADevice(ISADeviceDriver driver)
		{
			var hardwareDevice = driver.Factory() as IHardwareDevice;

			var ioPortRegions = new List<IOPortRegion>();
			var memoryRegions = new List<MemoryRegion>();

			ioPortRegions.Add(new IOPortRegion(driver.BasePort, driver.PortRange));

			if (driver.AltBasePort != 0x00)
			{
				ioPortRegions.Add(new IOPortRegion(driver.AltBasePort, driver.AltPortRange));
			}

			if (driver.BaseAddress != 0x00)
			{
				memoryRegions.Add(new MemoryRegion(driver.BaseAddress, driver.AddressRange));
			}

			//if (driver.PhysicalMemory != null)
			//{
			//	foreach (var physicalMemory in driver.PhysicalMemory)
			//	{
			//		if (physicalMemory.MemorySize > 0)
			//		{
			//			var memory = HAL.AllocateMemory(physicalMemory.MemorySize, physicalMemory.MemoryAlignment);

			//			memoryRegions.Add(new MemoryRegion(memory.Address, memory.Size));
			//		}
			//	}
			//}

			var interruptHandler = new InterruptHandler(InterruptManager, driver.IRQ, hardwareDevice);

			var hardwareResources = new HardwareResources(ioPortRegions, memoryRegions, interruptHandler);

			hardwareDevice.Setup(hardwareResources);

			if (!hardwareDevice.Probe())
				return;

			DeviceManager.Add(hardwareDevice);

			hardwareResources.EnableIRQ();

			hardwareDevice.Start();
		}
	}
}
