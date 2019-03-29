// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.PCI
{
	/// <summary>
	///
	/// </summary>
	public interface IPCIDeviceResource
	{
		/// <summary>
		/// Gets the function.
		/// </summary>
		/// <value>The function.</value>
		byte Function { get; }

		/// <summary>
		/// Gets the vendor ID.
		/// </summary>
		/// <value>The vendor ID.</value>
		ushort VendorID { get; }

		/// <summary>
		/// Gets the device ID.
		/// </summary>
		/// <value>The device ID.</value>
		ushort DeviceID { get; }

		/// <summary>
		/// Gets the revision ID.
		/// </summary>
		/// <value>The revision ID.</value>
		byte RevisionID { get; }

		/// <summary>
		/// Gets the class code.
		/// </summary>
		/// <value>The class code.</value>
		byte ClassCode { get; }

		/// <summary>
		/// Gets the prog IF.
		/// </summary>
		/// <value>The prog IF.</value>
		byte ProgIF { get; }

		/// <summary>
		/// Gets the sub class code.
		/// </summary>
		/// <value>The sub class code.</value>
		byte SubClassCode { get; }

		/// <summary>
		/// Gets the sub vendor ID.
		/// </summary>
		/// <value>The sub vendor ID.</value>
		ushort SubSystemVendorID { get; }

		/// <summary>
		/// Gets the sub device ID.
		/// </summary>
		/// <value>The sub device ID.</value>
		ushort SubSystemID { get; }

		/// <summary>
		/// Gets or sets the status register.
		/// </summary>
		/// <value>The status register.</value>
		ushort StatusRegister { get; set; }

		/// <summary>
		/// Gets or sets the command register.
		/// </summary>
		/// <value>The status.</value>
		ushort CommandRegister { get; set; }

		/// <summary>
		/// Enables the device.
		/// </summary>
		void EnableDevice();

		/// <summary>
		/// Disables the device.
		/// </summary>
		void DisableDevice();
	}
}
