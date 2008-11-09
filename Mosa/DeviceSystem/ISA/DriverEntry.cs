/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceSystem.ISA
{
	/// <summary>
	/// 
	/// </summary>
	public class DriverEntry
	{
		private DeviceSignatureAttribute isaDeviceSignatureAttribute;
		private System.Type driverType;

		/// <summary>
		/// Gets the ISA device signature attribute.
		/// </summary>
		/// <value>The ISA device signature attribute.</value>
		public DeviceSignatureAttribute SignatureAttribute { get { return isaDeviceSignatureAttribute; } }

		/// <summary>
		/// Gets the type of the driver.
		/// </summary>
		/// <value>The type of the driver.</value>
		public System.Type DriverType { get { return driverType; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="DriverEntry"/> class.
		/// </summary>
		/// <param name="isaDeviceSignatureAttribute">The isa device signature attribute.</param>
		/// <param name="driverType">Type of the driver.</param>
		public DriverEntry(DeviceSignatureAttribute isaDeviceSignatureAttribute, System.Type driverType)
		{
			this.isaDeviceSignatureAttribute = isaDeviceSignatureAttribute;
			this.driverType = driverType;
		}

	}
}
