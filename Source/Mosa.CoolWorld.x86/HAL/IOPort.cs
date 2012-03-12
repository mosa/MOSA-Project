/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;
using Mosa.Platform.x86.Intrinsic;

namespace Mosa.CoolWorld.x86.HAL
{

	/// <summary>
	/// Implementation of IReadWriteIOPort
	/// </summary>
	public class IOPort : IReadWriteIOPort, IWriteOnlyIOPort, IReadOnlyIOPort, IBaseIOPort
	{
		private ushort address;

		public ushort Address { get { return address; } }

		public IOPort(ushort address)
		{
			this.address = address;
		}
		/// <summary>
		/// Read8s this instance.
		/// </summary>
		/// <returns></returns>
		public byte Read8()
		{
			return Native.In8(address);
		}
		/// <summary>
		/// Read16s this instance.
		/// </summary>
		/// <returns></returns>
		public ushort Read16()
		{
			return Native.In16(address);
		}
		/// <summary>
		/// Read32s this instance.
		/// </summary>
		/// <returns></returns>
		public uint Read32()
		{
			return Native.In32(address);
		}
		/// <summary>
		/// Write8s the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		public void Write8(byte data)
		{
			Native.Out8(address, data);
		}
		/// <summary>
		/// Write16s the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		public void Write16(ushort data)
		{
			Native.Out16(address, data);
		}
		/// <summary>
		/// Write32s the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		public void Write32(uint data)
		{
			Native.Out32(address, data);
		}
	}

}
