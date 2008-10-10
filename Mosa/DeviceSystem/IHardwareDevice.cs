/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Interface to a hardware device driver
	/// </summary>
	public interface IHardwareDevice
	{
		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		bool Setup();

		/// <summary>
		/// Probes for this device.
		/// </summary>
		/// <returns></returns>
		bool Probe();

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
		bool Start();

		/// <summary>
		/// Creates the sub devices.
		/// </summary>
		/// <returns></returns>
		LinkedList<IDevice> CreateSubDevices();

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		bool OnInterrupt();

		/// <summary>
		/// Stops this hardware device.
		/// </summary>
		/// <returns></returns>
		bool Stop();

		/// <summary>
		/// Assigns the bus resources to this driver
		/// </summary>
		/// <param name="busResources">The bus resources.</param>
		void AssignBusResources(IBusResources busResources);

		/// <summary>
		/// Activates the device driver
		/// </summary>
		/// <param name="deviceManager">The device manager.</param>
		/// <returns></returns>
		bool Activate(IDeviceManager deviceManager);
	}
}
