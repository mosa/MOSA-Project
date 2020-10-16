// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.PCI
{
	/// <summary>
	/// PCI Device interface
	/// </summary>
	public interface IPCIDevice
	{
		/// <summary>
		/// Gets the bus.
		/// </summary>
		/// <value>The bus.</value>
		byte Bus { get; }

		/// <summary>
		/// Gets the slot.
		/// </summary>
		/// <value>The slot.</value>
		byte Slot { get; }

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
		/// Gets the IRQ.
		/// </summary>
		/// <value>The IRQ.</value>
		byte IRQ { get; }

		/// <summary>
		/// Gets the PCI base addresses.
		/// </summary>
		/// <value>The PCI base addresses.</value>
		BaseAddress[] BaseAddresses { get; }

		/// <summary>
		/// Sets the no driver found.
		/// </summary>
		void SetNoDriverFound();

		/// <summary>
		/// Sets the device online.
		/// </summary>
		void SetDeviceOnline();
	}
}
