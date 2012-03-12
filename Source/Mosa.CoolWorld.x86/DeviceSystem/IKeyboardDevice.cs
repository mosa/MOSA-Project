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
	/// Interface to a keyboard device
	/// </summary>
	public interface IKeyboardDevice
	{
		/// <summary>
		/// Gets the scan code from the keyboard device
		/// </summary>
		/// <returns></returns>
		byte GetScanCode();
	}

}
