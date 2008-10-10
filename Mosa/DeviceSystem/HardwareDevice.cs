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
	/// Abstract class for hardware devices
	/// </summary>
	public abstract class HardwareDevice : Device, IHardwareDevice
	{
		/// <summary>
		/// 
		/// </summary>
		protected IBusResources busResources;

		/// <summary>
		/// Initializes a new instance of the <see cref="HardwareDevice"/> class.
		/// </summary>
		public HardwareDevice() { base.deviceStatus = DeviceStatus.Initializing; }

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		public abstract bool Setup();

		/// <summary>
		/// Probes for this device.
		/// </summary>
		/// <returns></returns>
		public abstract bool Probe();

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
		public abstract bool Start();

		/// <summary>
		/// Creates the sub devices.
		/// </summary>
		/// <returns></returns>
		public abstract LinkedList<IDevice> CreateSubDevices();

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public abstract bool OnInterrupt();

		/// <summary>
		/// Stops this hardware device.
		/// </summary>
		/// <returns></returns>
		public bool Stop() { return false; }

		/// <summary>
		/// Activates the device driver
		/// </summary>
		/// <param name="deviceManager">The device manager.</param>
		/// <returns></returns>
		public bool Activate(IDeviceManager deviceManager)
		{
			base.deviceStatus = DeviceStatus.Initializing;

			if (!Setup()) {
				base.deviceStatus = DeviceStatus.Error;
				return false;
			}

			this.busResources.EnableIRQ();

			if (!Probe()) {
				base.deviceStatus = DeviceStatus.NotFound;
				return false;
			}

			if (!Start()) {
				base.deviceStatus = DeviceStatus.Error;
				return false;
			}

			base.deviceStatus = DeviceStatus.Online;

			LinkedList<IDevice> devices = CreateSubDevices();

			if (devices != null)
				foreach (IDevice device in devices)
					deviceManager.Add(device);

			return true;
		}

		/// <summary>
		/// Assigns the bus resources to this driver
		/// </summary>
		/// <param name="busResources">The bus resources.</param>
		public void AssignBusResources(IBusResources busResources)
		{
			this.busResources = busResources;
		}

	}
}
