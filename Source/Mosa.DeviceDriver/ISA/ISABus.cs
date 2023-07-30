// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.DeviceSystem;
using Mosa.Runtime;

namespace Mosa.DeviceDriver.ISA;

/// <summary>
/// ISA Bus Driver
/// </summary>
public class ISABus : BaseDeviceDriver
{
	public override void Initialize()
	{
		Device.Name = "ISA-BUS";
	}

	public override void Probe() => Device.Status = DeviceStatus.Available;

	public override void Start()
	{
		Device.Status = DeviceStatus.Online;

		StartISADevices();
	}

	public override bool OnInterrupt() => true;

	protected void StartISADevices()
	{
		HAL.DebugWriteLine("ISABus:StartISADevices()");

		// Start ISA Drivers
		var drivers = DeviceService.GetDeviceDrivers(DeviceBusType.ISA);

		foreach (var driver in drivers)
		{
			if (driver is ISADeviceDriverRegistryEntry)
			{
				HAL.DebugWrite(" > ISA Driver: ");
				HAL.DebugWriteLine(driver.Name);

				StartISADevice(driver as ISADeviceDriverRegistryEntry);
			}
		}

		HAL.DebugWriteLine("ISABus:StartISADevices() [Exit]");
	}

	protected void StartISADevice(ISADeviceDriverRegistryEntry driverEntry)
	{
		var ioPortRegions = new List<IOPortRegion>();
		var memoryRegions = new List<AddressRegion>();

		ioPortRegions.Add(new IOPortRegion(driverEntry.BasePort, driverEntry.PortRange));

		if (driverEntry.AltBasePort != 0x00)
		{
			ioPortRegions.Add(new IOPortRegion(driverEntry.AltBasePort, driverEntry.AltPortRange));
		}

		if (driverEntry.BaseAddress != 0x00)
		{
			memoryRegions.Add(new AddressRegion(new Pointer(driverEntry.BaseAddress), driverEntry.AddressRange));
		}

		//if (driverEntry.PhysicalMemory != null)
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

		var hardwareResources = new HardwareResources(ioPortRegions, memoryRegions, driverEntry.IRQ);

		DeviceService.Initialize(driverEntry, Device, true, null, hardwareResources);
	}
}
