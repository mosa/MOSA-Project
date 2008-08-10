/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Devices
{
	///TODO: Add API to set Serial settings, like baud rate, parity, etc.
	public interface ISerialDevice
	{
		 void Write (byte ch);
		 int ReadByte ();
	}
}
