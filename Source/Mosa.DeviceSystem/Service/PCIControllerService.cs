// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;
using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceSystem.Service;

/// <summary>
/// PCI Controller Service
/// </summary>
public class PCIControllerService : BaseService
{
	/// <summary>
	/// The device service
	/// </summary>
	protected DeviceService DeviceService;

	/// <summary>
	/// Initializes this instance.
	/// </summary>
	protected override void Initialize()
	{
		DeviceService = ServiceManager.GetFirstService<DeviceService>();
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

	/// <summary>
	/// Creates the partition devices.
	/// </summary>
	private void CreatePCIDevices(Device device, IPCIControllerLegacy pciController)
	{
		// For each controller
		for (int bus = 0; bus < 255; bus++)
		{
			for (int slot = 0; slot < 16; slot++)
			{
				for (int fun = 0; fun < 7; fun++)
				{
					if (!ProbeDevice(pciController, (byte)bus, (byte)slot, (byte)fun))
						continue;

					// TODO: Check for duplicate

					var configuration = new PCIDeviceConfiguration
					{
						Bus = (byte)bus,
						Slot = (byte)slot,
						Function = (byte)fun
					};

					DeviceService.Initialize(new PCIDevice(), device, true, configuration, null, null);
				}
			}
		}
	}

	/// <summary>
	/// Probes for a PCI device.
	/// </summary>
	/// <param name="pciController">The pci controller.</param>
	/// <param name="bus">The bus.</param>
	/// <param name="slot">The slot.</param>
	/// <param name="fun">The fun.</param>
	/// <returns></returns>
	protected static bool ProbeDevice(IPCIControllerLegacy pciController, byte bus, byte slot, byte fun)
	{
		uint value = pciController.ReadConfig32(bus, slot, fun, 0);

		return value != 0xFFFFFFFF;
	}
}
