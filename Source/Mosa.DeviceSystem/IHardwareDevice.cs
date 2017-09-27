// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceSystem
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
		/// Probes this instance.
		/// </summary>
		/// <returns></returns>
		bool Probe();

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
