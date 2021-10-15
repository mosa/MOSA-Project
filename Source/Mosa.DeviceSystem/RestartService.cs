// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Partition Manager
	/// </summary>
	public class RestartService : BaseService
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
			var acpi = DeviceService.GetDevices<IAPCI>(DeviceStatus.Online);

			if (acpi == null)
				return false;

			// TODO: Get Reset information (type, address, value) from ACPI
			int type = 0;
			int address = 0;
			int value = 0;

			// If via PCI Bus, get Host bridge controller:
			{
				var device = DeviceService.GetDevices<IHostBridgeController>(DeviceStatus.Online);
				var controller = device as IHostBridgeController;

				controller.SetCPUResetInformation(address, value);
				return controller.CPUReset();
			}

			return false;
		}
	}
}
