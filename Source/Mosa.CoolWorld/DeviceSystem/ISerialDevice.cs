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
	/// Interface to Serial Device
	/// </summary>
	/// TODO: Add API to set Serial settings, like baud rate, parity, etc.
	public interface ISerialDevice
	{
		/// <summary>
		/// Writes the specified character.
		/// </summary>
		/// <param name="c">The character.</param>
		void Write(byte c);
		/// <summary>
		/// Reads the byte.
		/// </summary>
		/// <returns></returns>
		int ReadByte();
	}
}
