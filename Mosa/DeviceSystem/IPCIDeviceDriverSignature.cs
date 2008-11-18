/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// 
	/// </summary>
	public interface IPCIDeviceDriverSignature : IDeviceDriverSignature
	{
		/// <summary>
		/// Gets or sets the device ID.
		/// </summary>
		/// <value>The device ID.</value>
		ushort DeviceID { get; }
		/// <summary>
		/// Gets or sets the vendor ID.
		/// </summary>
		/// <value>The vendor ID.</value>
		ushort VendorID { get; }
		/// <summary>
		/// Gets or sets the sub vendor ID.
		/// </summary>
		/// <value>The sub vendor ID.</value>
		ushort SubVendorID { get; }
		/// <summary>
		/// Gets or sets the sub device ID.
		/// </summary>
		/// <value>The sub device ID.</value>
		ushort SubDeviceID { get; }
		/// <summary>
		/// Gets or sets the revision ID.
		/// </summary>
		/// <value>The revision ID.</value>
		byte RevisionID { get; }
		/// <summary>
		/// Gets or sets the prog IF.
		/// </summary>
		/// <value>The prog IF.</value>
		byte ProgIF { get; }
		/// <summary>
		/// Gets or sets the class code.
		/// </summary>
		/// <value>The class code.</value>
		ushort ClassCode { get; }
		/// <summary>
		/// Gets or sets the sub class code.
		/// </summary>
		/// <value>The sub class code.</value>
		byte SubClassCode { get; }
		/// <summary>
		/// Gets the priority.
		/// </summary>
		/// <value>The priority.</value>
		int Priority { get; }

		/// <summary>
		/// Compares to.
		/// </summary>
		/// <param name="pciDevice">The pci device.</param>
		/// <returns></returns>
		bool CompareTo(PCIDevice pciDevice);
	}

}
