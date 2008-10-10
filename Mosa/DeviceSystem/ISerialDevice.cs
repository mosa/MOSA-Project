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
    /// 
    /// </summary>
    /// TODO: Add API to set Serial settings, like baud rate, parity, etc.
	public interface ISerialDevice
	{
        /// <summary>
        /// Writes the specified ch.
        /// </summary>
        /// <param name="ch">The ch.</param>
		void Write(byte ch);
        /// <summary>
        /// Reads the byte.
        /// </summary>
        /// <returns></returns>
		int ReadByte();
	}
}
