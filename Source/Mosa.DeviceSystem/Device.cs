// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Abstract base class for a device
	/// </summary>
	public abstract class Device : IDevice
	{
		/// <summary>
		///
		/// </summary>
		protected string name;

		/// <summary>
		///
		/// </summary>
		protected IDevice parent;

		/// <summary>
		///
		/// </summary>
		protected DeviceStatus deviceStatus;

		/// <summary>
		/// Gets the name of the device.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get { return name; } }

		/// <summary>
		/// Gets the parent device, if any, of the device.
		/// </summary>
		/// <value>The parent.</value>
		public IDevice Parent { get { return parent; } }

		/// <summary>
		/// Gets the status of the device.
		/// </summary>
		/// <value>The status.</value>
		public DeviceStatus Status { get { return deviceStatus; } }
	}
}