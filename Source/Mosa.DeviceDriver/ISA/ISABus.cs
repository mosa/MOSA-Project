// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.DeviceSystem.Framework;
using Mosa.DeviceSystem.Framework.ISA;
using Mosa.DeviceSystem.HardwareAbstraction;
using Mosa.Runtime;

namespace Mosa.DeviceDriver.ISA;

/// <summary>
/// A device driver whose sole purpose is to initialize all ISA devices in the system.
/// </summary>
public class ISABus : BaseDeviceDriver
{
	public override void Initialize() => Device.Name = "ISABus";

	public override void Probe() => Device.Status = DeviceStatus.Available;

	public override void Start()
	{
		Device.Status = DeviceStatus.Online;
		StartDevices();
	}

	public override bool OnInterrupt() => true;

	private void StartDevices()
	{
		HAL.DebugWriteLine("ISABus:StartDevices()");

		var drivers = DeviceService.GetDeviceDrivers(DeviceBusType.ISA);

		foreach (var driver in drivers)
		{
			if (driver is not ISADeviceDriverRegistryEntry entry)
				continue;

			HAL.DebugWrite(" > ISA Driver: ");
			HAL.DebugWriteLine(entry.Name);
			StartDevice(entry);
		}

		HAL.DebugWriteLine("ISABus:StartDevices() [Exit]");
	}

	private void StartDevice(ISADeviceDriverRegistryEntry driverEntry)
	{
		var ioPortRegions = new List<IOPortRegion>();
		var memoryRegions = new List<AddressRegion>();

		ioPortRegions.Add(new IOPortRegion(driverEntry.BasePort, driverEntry.PortRange));

		if (driverEntry.AltBasePort != 0x00)
			ioPortRegions.Add(new IOPortRegion(driverEntry.AltBasePort, driverEntry.AltPortRange));

		if (driverEntry.BaseAddress != 0x00)
			memoryRegions.Add(new AddressRegion(new Pointer(driverEntry.BaseAddress), driverEntry.AddressRange));

		var hardwareResources = new HardwareResources(ioPortRegions, memoryRegions, driverEntry.IRQ);
		DeviceService.Initialize(driverEntry, Device, true, null, hardwareResources);
	}
}
