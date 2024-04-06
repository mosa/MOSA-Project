// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.DeviceSystem.Framework;
using Mosa.DeviceSystem.Framework.ISA;
using Mosa.DeviceSystem.HardwareAbstraction;
using Mosa.Runtime;

namespace Mosa.DeviceSystem.Services;

/// <summary>
/// Initializes and starts all ISA devices in the system.
/// </summary>
public class ISADeviceService : BaseService
{
	protected override void Initialize()
	{
		HAL.DebugWriteLine("ISADeviceService::Initialize()");

		var deviceService = ServiceManager.GetFirstService<DeviceService>();
		var drivers = deviceService.GetDeviceDrivers(DeviceBusType.ISA);

		foreach (var driver in drivers)
		{
			var entry = (ISADeviceDriverRegistryEntry)driver;

			HAL.DebugWriteLine(" > ISA Driver: ");
			HAL.DebugWriteLine(entry.Name);

			var ioPortRegions = new List<IOPortRegion>();
			var memoryRegions = new List<AddressRegion>();

			ioPortRegions.Add(new IOPortRegion(entry.BasePort, entry.PortRange));

			if (entry.AltBasePort != 0x00)
				ioPortRegions.Add(new IOPortRegion(entry.AltBasePort, entry.AltPortRange));

			if (entry.BaseAddress != 0x00)
				memoryRegions.Add(new AddressRegion(new Pointer(entry.BaseAddress), entry.AddressRange));

			var hardwareResources = new HardwareResources(ioPortRegions, memoryRegions, entry.IRQ);
			deviceService.Initialize(entry, null, true, null, hardwareResources);
		}

		HAL.DebugWriteLine("ISADeviceService::Initialize() [Exit]");
	}
}
