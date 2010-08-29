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
	public class IOPortRegister
	{
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort ioPort;
		/// <summary>
		/// 
		/// </summary>
		protected byte bits;

		/// <summary>
		/// IOs the port region.
		/// </summary>
		/// <param name="ioPort">The io port.</param>
		/// <param name="bits">The bits.</param>
		public IOPortRegister(IReadWriteIOPort ioPort, byte bits)
		{
			this.ioPort = ioPort;
			this.bits = bits;
		}

		/// <summary>
		/// Sets the bit.
		/// </summary>
		/// <param name="bit">The bit.</param>
		protected void SetBit(byte bit)
		{
			switch (bits)
			{
				case 8: ioPort.Write8(SetBit(ioPort.Read8(), bit)); break;
				case 16: ioPort.Write16(SetBit(ioPort.Read16(), bit)); break;
				case 32: ioPort.Write32(SetBit(ioPort.Read32(), bit)); break;
			}
		}

		/// <summary>
		/// Sets the bit.
		/// </summary>
		/// <param name="bit">The bit.</param>
		protected void ClearBit(byte bit)
		{
			switch (bits)
			{
				case 8: ioPort.Write8(ClearBit(ioPort.Read8(), bit)); break;
				case 16: ioPort.Write16(ClearBit(ioPort.Read16(), bit)); break;
				case 32: ioPort.Write32(ClearBit(ioPort.Read32(), bit)); break;
			}
		}

		/// <summary>
		/// Sets the bit.
		/// </summary>
		/// <param name="bit">The bit.</param>
		/// <param name="value">if set to <c>true</c> [value].</param>
		protected void SetBit(byte bit, bool value)
		{
			if (value) SetBit(bit); else ClearBit(bit);
		}

		/// <summary>
		/// Gets the bit.
		/// </summary>
		/// <param name="bit">The bit.</param>
		/// <returns></returns>
		protected bool GetBit(byte bit)
		{
			switch (bits)
			{
				case 8: return ((ioPort.Read8() & (1 << bit)) != 0);
				case 16: return ((ioPort.Read16() & (1 << bit)) != 0);
				case 32: return ((ioPort.Read32() & (1 << bit)) != 0);
			}

			return false;
		}

		/// <summary>
		/// Sets the bit.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="bit">The bit.</param>
		/// <returns></returns>
		private static byte SetBit(byte value, byte bit)
		{
			return (byte)(value | (1 << bit));
		}

		/// <summary>
		/// Sets the bit.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="bit">The bit.</param>
		/// <returns></returns>
		private static ushort SetBit(ushort value, byte bit)
		{
			return (ushort)(value | (1 << bit));
		}

		/// <summary>
		/// Sets the bit.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="bit">The bit.</param>
		/// <returns></returns>
		private static uint SetBit(uint value, byte bit)
		{
			return (uint)(value | ((uint)1 << bit));
		}

		/// <summary>
		/// Clears the bit.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="bit">The bit.</param>
		/// <returns></returns>
		private static byte ClearBit(byte value, byte bit)
		{
			return (byte)(value & ~(1 << bit));
		}

		/// <summary>
		/// Clears the bit.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="bit">The bit.</param>
		/// <returns></returns>
		private static ushort ClearBit(ushort value, byte bit)
		{
			return (ushort)(value & ~(1 << bit));
		}

		/// <summary>
		/// Clears the bit.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="bit">The bit.</param>
		/// <returns></returns>
		private static uint ClearBit(uint value, byte bit)
		{
			return (uint)(value & ~(1 << bit));
		}

		/// <summary>
		/// Sets the bits.
		/// </summary>
		/// <param name="current">The current.</param>
		/// <param name="value">The value.</param>
		/// <param name="startbit">The startbit.</param>
		/// <param name="bits">The bits.</param>
		/// <returns></returns>
		private static byte SetBits(byte current, byte value, byte startbit, byte bits)
		{
			if (bits == 0) return current;
			if (bits == 8) return value;

			byte mask = (byte)((1 << bits) - 1);

			return (byte)((current & (~mask)) | ((value & mask) << startbit));
		}

		/// <summary>
		/// Sets the bits.
		/// </summary>
		/// <param name="current">The current.</param>
		/// <param name="value">The value.</param>
		/// <param name="startbit">The startbit.</param>
		/// <param name="bits">The bits.</param>
		/// <returns></returns>
		private static ushort SetBits(ushort current, ushort value, byte startbit, byte bits)
		{
			if (bits == 0) return current;
			if (bits == 16) return value;

			uint mask = (ushort)((1 << bits) - 1);

			return (ushort)((current & (~mask)) | ((value & mask) << startbit));
		}

		/// <summary>
		/// Sets the bits.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="startbit">The startbit.</param>
		/// <param name="bits">The bits.</param>
		/// <returns></returns>
		protected uint SetBits(byte value, byte startbit, byte bits)
		{
			return SetBits(ioPort.Read8(), value, startbit, bits);
		}

		/// <summary>
		/// Sets the bits.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="startbit">The startbit.</param>
		/// <param name="bits">The bits.</param>
		/// <returns></returns>
		protected uint SetBits(ushort value, byte startbit, byte bits)
		{
			return SetBits(ioPort.Read16(), value, startbit, bits);
		}


		/// <summary>
		/// Sets the bits.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="startbit">The startbit.</param>
		/// <param name="bits">The bits.</param>
		/// <returns></returns>
		protected uint SetBits(uint value, byte startbit, byte bits)
		{
			return SetBits(ioPort.Read32(), value, startbit, bits);
		}

		/// <summary>
		/// Sets the bits.
		/// </summary>
		/// <param name="current">The current.</param>
		/// <param name="value">The value.</param>
		/// <param name="startbit">The startbit.</param>
		/// <param name="bits">The bits.</param>
		/// <returns></returns>
		private static uint SetBits(uint current, uint value, byte startbit, byte bits)
		{
			if (bits == 0) return current;
			if (bits == 32) return value;

			uint mask = (uint)(1 << bits) - 1;

			return (current & (~mask)) | ((value & mask) << startbit);
		}

		/// <summary>
		/// Gets the bits8.
		/// </summary>
		/// <param name="startbit">The startbit.</param>
		/// <param name="bits">The bits.</param>
		/// <returns></returns>
		protected uint GetBits(byte startbit, byte bits)
		{
			if (bits == 0) return 0;

			uint value = 0;

			switch (bits)
			{
				case 8: value = ioPort.Read8(); break;
				case 16: value = ioPort.Read16(); break;
				case 32: value = ioPort.Read32(); break;
			}

			return (uint)((value >> startbit) & ((1 << bits) - 1));
		}
	}
}
