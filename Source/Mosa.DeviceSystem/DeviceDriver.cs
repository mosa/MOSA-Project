// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;

namespace Mosa.DeviceSystem
{
	/// <summary>
	///
	/// </summary>
	public class DeviceDriver
	{
		/// <summary>
		/// Gets the signature attribute.
		/// </summary>
		/// <value>The signature attribute.</value>
		public IDeviceDriver Attribute { get; private set; }

		/// <summary>
		/// Gets the type of the driver.
		/// </summary>
		/// <value>The type of the driver.</value>
		public Type DriverType { get; private set; }

		/// <summary>
		/// Gets the memory attributes.
		/// </summary>
		/// <value>The memory attributes.</value>
		public LinkedList<DeviceDriverPhysicalMemoryAttribute> MemoryAttributes { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceDriver"/> class.
		/// </summary>
		/// <param name="deviceDriverAttribute">The device driver attribute.</param>
		/// <param name="driverType">Type of the driver.</param>
		public DeviceDriver(IDeviceDriver deviceDriverAttribute, Type driverType)
		{
			Attribute = deviceDriverAttribute;
			DriverType = driverType;
			MemoryAttributes = new LinkedList<DeviceDriverPhysicalMemoryAttribute>();
		}

		/// <summary>
		/// Adds the specified memory attribute.
		/// </summary>
		/// <param name="memoryAttribute">The memory attribute.</param>
		public void Add(DeviceDriverPhysicalMemoryAttribute memoryAttribute)
		{
			MemoryAttributes.AddLast(memoryAttribute);
		}
	}
}
