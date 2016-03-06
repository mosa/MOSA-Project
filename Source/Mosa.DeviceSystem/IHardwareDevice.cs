// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Interface to a hardware device driver
	/// </summary>
	public interface IHardwareDevice : IDevice
	{
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
