/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceSystem.PCI
{
	/// <summary>
	/// 
	/// </summary>
	public class PCIDriverEntry
	{
		private PCIDeviceSignatureAttribute pciDeviceSignatureAttribute;
		private System.Type driverType;

		/// <summary>
		/// Gets the signature attribute.
		/// </summary>
		/// <value>The signature attribute.</value>
		public PCIDeviceSignatureAttribute SignatureAttribute { get { return pciDeviceSignatureAttribute; } }

		/// <summary>
		/// Gets the type of the driver.
		/// </summary>
		/// <value>The type of the driver.</value>
		public System.Type DriverType { get { return driverType; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="PCIDriverEntry"/> class.
		/// </summary>
		/// <param name="pciDeviceSignatureAttribute">The pci device signature attribute.</param>
		/// <param name="driverType">Type of the driver.</param>
		public PCIDriverEntry(PCIDeviceSignatureAttribute pciDeviceSignatureAttribute, System.Type driverType)
		{
			this.pciDeviceSignatureAttribute = pciDeviceSignatureAttribute;
			this.driverType = driverType;
		}

	}
}
