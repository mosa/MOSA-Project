// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;
using Mosa.DeviceSystem.Drivers.PCI;
using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceSystem.Services;

/// <summary>
/// Initializes the PCI controller and creates dummy PCI devices for the <see cref="PCIDeviceService"/>.
/// </summary>
public class PCIControllerService : BaseService
{
	private DeviceService deviceService;

	protected override void Initialize()
	{
		deviceService = ServiceManager.GetFirstService<DeviceService>();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void PostEvent(ServiceEvent serviceEvent)
	{
		var device = MatchEvent<IPCIControllerLegacy>(serviceEvent, ServiceEventType.Start);
		if (device == null)
			return;

		var pciController = device.DeviceDriver as IPCIControllerLegacy;
		CreatePCIDevices(device, pciController);
	}

	private void CreatePCIDevices(Device device, IPCIControllerLegacy pciController)
	{
		// For each controller
		for (var bus = 0; bus < byte.MaxValue; bus++)
		{
			for (var slot = 0; slot < 16; slot++)
			{
				for (var fun = 0; fun < 7; fun++)
				{
					if (!ProbeDevice(pciController, (byte)bus, (byte)slot, (byte)fun))
						continue;

					// TODO: Check for duplicates

					var configuration = new PCIDeviceConfiguration
					{
						Bus = (byte)bus,
						Slot = (byte)slot,
						Function = (byte)fun
					};

					deviceService.Initialize(new PCIDevice(), device, true, configuration);
				}
			}
		}
	}

	private static bool ProbeDevice(IPCIControllerLegacy pciController, byte bus, byte slot, byte fun)
	{
		var value = pciController.ReadConfig32(bus, slot, fun, 0);
		return value != 0xFFFFFFFF;
	}
}
