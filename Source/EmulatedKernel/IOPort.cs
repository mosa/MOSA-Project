/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;

namespace Mosa.EmulatedKernel
{
	/// <summary>
	/// 
	/// </summary>
	public class IOPort : IReadWriteIOPort
	{
		private ushort port;

		/// <summary>
		/// Initializes a new instance of the <see cref="IOPort"/> class.
		/// </summary>
		/// <param name="port">The port.</param>
		public IOPort(ushort port) { this.port = port; }

		/// <summary>
		/// Gets the address.
		/// </summary>
		/// <value>The address.</value>
		public ushort Address { get { return port; } }

		/// <summary>
		/// Reads this instance.
		/// </summary>
		/// <returns></returns>
		public byte Read8() { return IOPortDispatch.Read8(port); }

		/// <summary>
		/// Reads this instance.
		/// </summary>
		/// <returns></returns>
		public ushort Read16() { return IOPortDispatch.Read16(port); }

		/// <summary>
		/// Reads this instance.
		/// </summary>
		/// <returns></returns>
		public uint Read32() { return IOPortDispatch.Read32(port); }

		/// <summary>
		/// Writes the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		public void Write8(byte data) { IOPortDispatch.Write8(port, data); }

		/// <summary>
		/// Writes the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		public void Write16(ushort data) { IOPortDispatch.Write16(port, data); }

		/// <summary>
		/// Writes the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		public void Write32(uint data) { IOPortDispatch.Write32(port, data); }
	}
}
