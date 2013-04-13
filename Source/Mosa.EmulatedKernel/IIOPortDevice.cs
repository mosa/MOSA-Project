/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.EmulatedKernel
{
	/// <summary>
	///
	/// </summary>
	public interface IIOPortDevice
	{
		/// <summary>
		/// Reads the specified port.
		/// </summary>
		/// <param name="port"></param>
		/// <returns></returns>
		byte Read8(ushort port);

		/// <summary>
		/// Read3s the specified port.
		/// </summary>
		/// <param name="port"></param>
		/// <returns></returns>
		ushort Read16(ushort port);

		/// <summary>
		/// Reads the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <returns></returns>
		uint Read32(ushort port);

		/// <summary>
		/// Writes to the specified port.
		/// </summary>
		/// <param name="port"></param>
		/// <param name="data"></param>
		void Write8(ushort port, byte data);

		/// <summary>
		/// Writes to the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <param name="data">The data.</param>
		void Write16(ushort port, ushort data);

		/// <summary>
		/// Writes to the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <param name="data">The data.</param>
		void Write32(ushort port, uint data);

		/// <summary>
		/// Gets the ports requested.
		/// </summary>
		/// <returns></returns>
		ushort[] GetPortsRequested();
	}
}