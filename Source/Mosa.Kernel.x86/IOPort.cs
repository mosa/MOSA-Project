// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// IOPort
	/// </summary>
	public static class IOPort
	{
		/// <summary>
		/// Reads a byte
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		public static byte In8(ushort address)
		{
			return Native.In8(address);
		}

		/// <summary>
		/// Reads a short
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		public static ushort In16(ushort address)
		{
			return Native.In16(address);
		}

		/// <summary>
		/// Reads an integer
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		public static uint In32(ushort address)
		{
			return Native.In32(address);
		}

		/// <summary>
		/// Writes a byte
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="data">The data.</param>
		public static void Out8(ushort address, byte data)
		{
			Native.Out8(address, data);
		}

		/// <summary>
		/// Writes a short
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="data">The data.</param>
		public static void Out16(ushort address, ushort data)
		{
			Native.Out16(address, data);
		}

		/// <summary>
		/// Writes an integer
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="data">The data.</param>
		public static void Out32(ushort address, uint data)
		{
			Native.Out32(address, data);
		}
	}
}
