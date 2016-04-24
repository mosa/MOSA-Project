// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.HardwareSystem.PCI;

namespace Mosa.HardwareSystem
{
	/// <summary>
	/// Interface to a hardware device driver
	/// </summary>
	public interface IHardwareDevice : IDevice
	{
		bool PreSetup(IPCIDeviceResource pciDeviceResource);

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		bool Setup(HardwareResources hardwareResources);

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
		DeviceDriverStartStatus Start();

		/// <summary>
		/// Stops this hardware device.
		/// </summary>
		/// <returns></returns>
		bool Stop();

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		bool OnInterrupt();
	}
}
