// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.Runtime.x86;

namespace Mosa.CoolWorld.x86.HAL
{
	/// <summary>
	/// Implementation of IReadWriteIOPort
	/// </summary>
	public sealed class IOPort : IReadWriteIOPort, IWriteOnlyIOPort, IReadOnlyIOPort, IBaseIOPort
	{
		public ushort Address { get; private set; }

		public IOPort(ushort address)
		{
			this.Address = address;
		}

		/// <summary>
		/// Read8s this instance.
		/// </summary>
		/// <returns></returns>
		public byte Read8()
		{
			return Native.In8(Address);
		}

		/// <summary>
		/// Read16s this instance.
		/// </summary>
		/// <returns></returns>
		public ushort Read16()
		{
			return Native.In16(Address);
		}

		/// <summary>
		/// Read32s this instance.
		/// </summary>
		/// <returns></returns>
		public uint Read32()
		{
			return Native.In32(Address);
		}

		/// <summary>
		/// Write8s the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		public void Write8(byte data)
		{
			Native.Out8(Address, data);
		}

		/// <summary>
		/// Write16s the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		public void Write16(ushort data)
		{
			Native.Out16(Address, data);
		}

		/// <summary>
		/// Write32s the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		public void Write32(uint data)
		{
			Native.Out32(Address, data);
		}
	}
}
