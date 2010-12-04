/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Represents the state of a device
	/// </summary>
	public enum DeviceStatus : byte
	{
		/// <summary>
		/// The device is initializing
		/// </summary>
		Initializing,
		/// <summary>
		/// The device is available to be put online
		/// </summary>
		Available,
		/// <summary>
		/// The device is online and in use by the system
		/// </summary>
		Online,
		/// <summary>
		/// No driver can be found for the device
		/// </summary>
		NotFound,
		/// <summary>
		/// Something is wrong with this device
		/// </summary>
		Error
	}
}
