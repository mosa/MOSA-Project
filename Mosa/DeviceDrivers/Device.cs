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
	public abstract class Device : IDevice
	{
		protected string name;
		protected IDevice parent;
		protected DeviceStatus deviceStatus;

		public string Name { get { return name; } }
		public IDevice Parent { get { return parent; } }
		public DeviceStatus Status { get { return deviceStatus; } }
	}
}
