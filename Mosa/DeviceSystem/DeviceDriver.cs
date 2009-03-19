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
		private IDeviceDriverAttribute deviceDriverAttribute;
		private System.Type driverType;
		private LinkedList<DeviceDriverMemoryAttribute> memoryAttributes;

		/// <summary>
		/// Gets the signature attribute.
		/// </summary>
		/// <value>The signature attribute.</value>
		public IDeviceDriverAttribute Attribute { get { return deviceDriverAttribute; } }

		/// <summary>
		/// Gets the type of the driver.
		/// </summary>
		/// <value>The type of the driver.</value>
		public System.Type DriverType { get { return driverType; } }

		/// <summary>
		/// Gets the memory attributes.
		/// </summary>
		/// <value>The memory attributes.</value>
		public LinkedList<DeviceDriverMemoryAttribute> MemoryAttributes { get { return memoryAttributes; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceDriver"/> class.
		/// </summary>
		/// <param name="deviceDriverAttribute">The device driver attribute.</param>
		/// <param name="driverType">Type of the driver.</param>
		public DeviceDriver(IDeviceDriverAttribute deviceDriverAttribute, System.Type driverType)
		{
			this.deviceDriverAttribute = deviceDriverAttribute;
			this.driverType = driverType;
			this.memoryAttributes = new LinkedList<DeviceDriverMemoryAttribute>();
		}

		/// <summary>
		/// Adds the specified memory attribute.
		/// </summary>
		/// <param name="memoryAttribute">The memory attribute.</param>
		public void Add(DeviceDriverMemoryAttribute memoryAttribute)
		{
			memoryAttributes.Add(memoryAttribute);
		}
	}
}
