// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Implementation of IReadWriteIOPort
	/// </summary>
	public static class IOPort
	{
		/// <summary>
		/// Read8s this instance.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		public static byte In8(ushort address)
		{
			return Native.In8(address);
		}

		/// <summary>
		/// Read16s this instance.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		public static ushort In16(ushort address)
		{
			return Native.In16(address);
		}

		/// <summary>
		/// Read32s this instance.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		public static uint In32(ushort address)
		{
			return Native.In32(address);
		}

		/// <summary>
		/// Write8s the specified data.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="data">The data.</param>
		public static void Out8(ushort address, byte data)
		{
			Native.Out8(address, data);
		}

		/// <summary>
		/// Write16s the specified data.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="data">The data.</param>
		public static void Out16(ushort address, ushort data)
		{
			Native.Out16(address, data);
		}

		/// <summary>
		/// Write32s the specified data.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="data">The data.</param>
		public static void Out32(ushort address, uint data)
		{
			Native.Out32(address, data);
		}
	}
}
