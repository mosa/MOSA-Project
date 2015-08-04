// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
		public HardwareDevice()
		{
			base.deviceStatus = DeviceStatus.Initializing;
		}

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
		public bool Stop()
		{
			return false;
		}

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public abstract bool OnInterrupt();
	}
}
