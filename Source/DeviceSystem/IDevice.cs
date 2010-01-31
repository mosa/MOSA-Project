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
	/// Interface to a device
	/// </summary>
	public interface IDevice
	{
		/// <summary>
		/// Gets the name of the device.
		/// </summary>
		/// <value>The name.</value>
		string Name { get; }

		/// <summary>
		/// Gets the parent device, if any, of the device.
		/// </summary>
		/// <value>The parent.</value>
		IDevice Parent { get; }

		/// <summary>
		/// Gets the status of the device.
		/// </summary>
		/// <value>The status.</value>
		DeviceStatus Status { get; }
	}
}
