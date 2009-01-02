/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;

namespace Mosa.DeviceSystem.PCI
{
	/// <summary>
	/// 
	/// </summary>
	public interface IPCIDeviceResource
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
	}
}
