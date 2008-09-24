/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Reflection;
using Mosa.ClassLib;

namespace Mosa.DeviceDrivers.ISA
{
	/// <summary>
	/// 
	/// </summary>
	public class ISADriverEntry
	{
		private ISADeviceSignatureAttribute isaDeviceSignatureAttribute;
		private Type driverType;

		/// <summary>
		/// Gets the ISA device signature attribute.
		/// </summary>
		/// <value>The ISA device signature attribute.</value>
		public ISADeviceSignatureAttribute SignatureAttribute { get { return isaDeviceSignatureAttribute; } }

		/// <summary>
		/// Gets the type of the driver.
		/// </summary>
		/// <value>The type of the driver.</value>
		public Type DriverType { get { return driverType; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="ISADriverEntry"/> class.
		/// </summary>
		/// <param name="isaDeviceSignatureAttribute">The isa device signature attribute.</param>
		/// <param name="driverType">Type of the driver.</param>
		public ISADriverEntry(ISADeviceSignatureAttribute isaDeviceSignatureAttribute, Type driverType)
		{
			this.isaDeviceSignatureAttribute = isaDeviceSignatureAttribute;
			this.driverType = driverType;
		}

	}
}
