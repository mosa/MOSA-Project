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
		protected IHardwareResources hardwareResources;

		/// <summary>
		/// Initializes a new instance of the <see cref="HardwareDevice"/> class.
		/// </summary>
		public HardwareDevice() { base.deviceStatus = DeviceStatus.Initializing; }

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		public abstract bool Setup(IHardwareResources hardwareResources);

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
		public abstract DeviceDriverStartStatus Start();

		/// <summary>
		/// Stops this hardware device.
		/// </summary>
		/// <returns></returns>
		public bool Stop() { return false; }

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

		///// <summary>
		///// Activates the device driver
		///// </summary>
		///// <param name="deviceManager">The device manager.</param>
		///// <returns></returns>
		//public bool Activate(IDeviceManager deviceManager)
		//{
		//    base.deviceStatus = DeviceStatus.Initializing;
		//
		//    if (!Setup()) {
		//        base.deviceStatus = DeviceStatus.Error;
		//        return false;
		//    }
		//
		//    this.hardwareResources.EnableIRQ();
		//
		//    if (!Probe()) {
		//        base.deviceStatus = DeviceStatus.NotFound;
		//        return false;
		//    }
		//
		//    if (!Start()) {
		//        base.deviceStatus = DeviceStatus.Error;
		//        return false;
		//    }
		//
		//    base.deviceStatus = DeviceStatus.Online;
		//
		//    LinkedList<IDevice> devices = CreateSubDevices();
		//
		//    if (devices != null)
		//        foreach (IDevice device in devices)
		//            deviceManager.Add(device);
		//
		//    return true;
		//}

	}
}
