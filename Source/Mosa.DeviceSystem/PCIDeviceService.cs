// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// PCI Controller Service
	/// </summary>
	public class PCIDeviceService : BaseService
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

		public override void PostEvent(ServiceEvent serviceEvent)
		{
			var device = MatchEvent<PCIDevice>(serviceEvent, ServiceEventType.Start);

			if (device == null)
				return;

			// TODO
		}
	}
}
