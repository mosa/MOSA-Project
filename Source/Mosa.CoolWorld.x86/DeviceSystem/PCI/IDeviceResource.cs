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
	public interface IDeviceResource
	{
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
