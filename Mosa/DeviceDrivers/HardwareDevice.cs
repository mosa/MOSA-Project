/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;

namespace Mosa.DeviceDrivers
{
    /// <summary>
    /// 
    /// </summary>
	public abstract class HardwareDevice : Device, IHardwareDevice
	{
        /// <summary>
        /// 
        /// </summary>
		protected IBusResources busResources;

        /// <summary>
        /// 
        /// </summary>
		public HardwareDevice() { base.deviceStatus = DeviceStatus.Initializing; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public abstract bool Setup();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public abstract bool Probe();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public abstract bool Start();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public abstract LinkedList<IDevice> CreateSubDevices();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public abstract bool OnInterrupt();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public bool Stop() { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceManager"></param>
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
        /// 
        /// </summary>
        /// <param name="busResources"></param>
		public void AssignBusResources(IBusResources busResources)
		{
			this.busResources = busResources;
		}

	}
}
