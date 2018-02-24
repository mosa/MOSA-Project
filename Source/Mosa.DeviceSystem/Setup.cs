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
			// todo: this needs to move into ISAManagerDeviceDriver
			StartISADevices();
		}

		public static void StartISADevices()
		{
			// Start ISA Drivers
			var drivers = DeviceDriverRegistry.GetDeviceDrivers(DeviceBusType.ISA);

			foreach (var driver in drivers)
			{
				if (driver is ISADeviceDriverRegistryEntry)
				{
					StartISADevice(driver as ISADeviceDriverRegistryEntry);
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

		public static void StartISADevice(ISADeviceDriverRegistryEntry driverEntry)
		{
			var ioPortRegions = new List<IOPortRegion>();
			var memoryRegions = new List<MemoryRegion>();

			ioPortRegions.Add(new IOPortRegion(driverEntry.BasePort, driverEntry.PortRange));

			if (driverEntry.AltBasePort != 0x00)
			{
				ioPortRegions.Add(new IOPortRegion(driverEntry.AltBasePort, driverEntry.AltPortRange));
			}

			if (driverEntry.BaseAddress != 0x00)
			{
				memoryRegions.Add(new MemoryRegion(driverEntry.BaseAddress, driverEntry.AddressRange));
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

			//var interruptHandler = (driverEntry.IRQ != 0) ? new InterruptHandler(InterruptManager, driverEntry.IRQ, deviceDriver as IHardwareDevice) : null;

			var hardwareResources = new HardwareResources(ioPortRegions, memoryRegions, null);

			DeviceManager.Initialize(driverEntry, null, null, hardwareResources);

			//driver.Setup(device);

			//if (!driver.Probe())
			//	return;

			//hardwareResources.EnableIRQ();

			//driver.Start();
		}
	}
}
