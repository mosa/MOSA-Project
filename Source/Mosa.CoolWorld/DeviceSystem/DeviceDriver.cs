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
	/// 
	/// </summary>
	public class DeviceDriver
	{
		private IDeviceDriver deviceDriverAttribute;
		private System.Type driverType;
		private LinkedList<DeviceDriverPhysicalMemoryAttribute> memoryAttributes;

		/// <summary>
		/// Gets the signature attribute.
		/// </summary>
		/// <value>The signature attribute.</value>
		public IDeviceDriver Attribute { get { return deviceDriverAttribute; } }

		/// <summary>
		/// Gets the type of the driver.
		/// </summary>
		/// <value>The type of the driver.</value>
		public System.Type DriverType { get { return driverType; } }

		/// <summary>
		/// Gets the memory attributes.
		/// </summary>
		/// <value>The memory attributes.</value>
		public LinkedList<DeviceDriverPhysicalMemoryAttribute> MemoryAttributes { get { return memoryAttributes; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceDriver"/> class.
		/// </summary>
		/// <param name="deviceDriverAttribute">The device driver attribute.</param>
		/// <param name="driverType">Type of the driver.</param>
		public DeviceDriver(IDeviceDriver deviceDriverAttribute, System.Type driverType)
		{
			this.deviceDriverAttribute = deviceDriverAttribute;
			this.driverType = driverType;
			this.memoryAttributes = new LinkedList<DeviceDriverPhysicalMemoryAttribute>();
		}

		/// <summary>
		/// Adds the specified memory attribute.
		/// </summary>
		/// <param name="memoryAttribute">The memory attribute.</param>
		public void Add(DeviceDriverPhysicalMemoryAttribute memoryAttribute)
		{
			memoryAttributes.Add(memoryAttribute);
		}
	}
}
