// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// PC Service
	/// </summary>
	public class PCService : BaseService
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

		public bool Reset()
		{
			var acpi = DeviceService.GetFirstDevice<IAPCI>(DeviceStatus.Online);

			if (acpi == null)
				return false;

			// TODO: Get Reset information (type, address, value) from ACPI
			byte type = 0;
			byte address = 0;
			byte value = 0;

			// If via PCI Bus, get Host bridge controller:
			{
				var device = DeviceService.GetDevices<IHostBridgeController>(DeviceStatus.Online);
				var controller = device as IHostBridgeController;

				controller.SetCPUResetInformation(address, value);
				return controller.CPUReset();
			}

			return false;
		}

		public bool Shutdown()
		{
			// TODO
			return Reset();
		}
	}
}
