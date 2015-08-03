// Copyright (c) MOSA Project. Licensed under the New BSD License.

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