// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using System.Collections.Generic;

namespace Mosa.DeviceDriver.ISA
{
	/// <summary>
	/// ISA Bus Driver
	/// </summary>
	public class ISABus : BaseDeviceDriver
	{
		protected SpinLock spinLock;

		protected override void Initialize()
		{
			Device.Name = "ISA-BUS";

			Device.Status = DeviceStatus.Available;
		}

		public override void Probe() => Device.Status = DeviceStatus.Available;

		public override void Start()
		{
			if (Device.Status != DeviceStatus.Available)
				return;

			Device.Status = DeviceStatus.Online;

			StartISADevices();
		}

		public override bool OnInterrupt() => true;

		protected static void StartISADevices()
		{
			// Start ISA Drivers
			var drivers = Mosa.DeviceSystem.Setup.DeviceDriverRegistry.GetDeviceDrivers(DeviceBusType.ISA);

			foreach (var driver in drivers)
			{
				if (driver is ISADeviceDriverRegistryEntry)
				{
					StartISADevice(driver as ISADeviceDriverRegistryEntry);
				}
			}
		}

		protected static void StartISADevice(ISADeviceDriverRegistryEntry driverEntry)
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

			var deviceDriver = Mosa.DeviceSystem.Setup.DeviceManager.Initialize(driverEntry, null, null, hardwareResources);
		}
	}
}
