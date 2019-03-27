// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceSystem
{
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
		public override void Initialize()
		{
			DeviceService = ServiceManager.GetFirstService<DeviceService>();
		}

		/// <summary>
		/// Probes for a PCI device.
		/// </summary>
		/// <param name="pciController">The pci controller.</param>
		/// <param name="bus">The bus.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="fun">The fun.</param>
		/// <returns></returns>
		protected bool ProbeDevice(IPCIController pciController, byte bus, byte slot, byte fun)
		{
			uint value = pciController.ReadConfig32(bus, slot, fun, 0);

			return value != 0xFFFFFFFF;
		}

		/// <summary>
		/// Creates the partition devices.
		/// </summary>
		public void CreatePCIDevices()
		{
			// Find PCI controller devices
			var devices = DeviceService.GetDevices<IPCIController>(DeviceStatus.Online);

			if (devices.Count == 0)
				return;

			var device = devices[0];

			var pciController = device.DeviceDriver as IPCIController;

			// For each controller
			for (int bus = 0; bus < 255; bus++)
			{
				for (int slot = 0; slot < 16; slot++)
				{
					for (int fun = 0; fun < 7; fun++)
					{
						if (!ProbeDevice(pciController, (byte)bus, (byte)slot, (byte)fun))
							continue;

						var configuration = new PCIDeviceConfiguration()
						{
							Bus = (byte)bus,
							Slot = (byte)slot,
							Function = (byte)fun
						};

						DeviceService.Initialize(new PCIDevice(), device, configuration, null, null);
					}
				}
			}
		}
	}
}
