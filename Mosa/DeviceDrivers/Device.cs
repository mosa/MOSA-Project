/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceDrivers
{
	/// <summary>
	/// Abstract base class for a device
	/// </summary>
	public abstract class Device : IDevice
	{
		protected string name;
		protected IDevice parent;
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
