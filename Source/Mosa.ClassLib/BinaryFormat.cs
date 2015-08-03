// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;

namespace Mosa.ClassLib
{
	/// <summary>
	/// Binary Formatter (Little Endian)
	/// </summary>
	public struct BinaryFormat
	{
		/// <summary>
		///
		/// </summary>
		private byte[] data;

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryFormat"/> struct.
		/// </summary>
		/// <param name="data">The data.</param>
		public BinaryFormat(byte[] data)
		{
			this.data = data;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BinaryFormat"/> struct.
		/// </summary>
		/// <param name="length">The length.</param>
		public BinaryFormat(uint length)
		{
			this.data = new byte[length];
		}

		/// <summary>
		/// Gets the length.
		/// </summary>
		/// <value>The length.</value>
		public uint Length
		{
			get { return (uint)data.Length; }
		}

		/// <summary>
		/// Gets the data.
		/// </summary>
		/// <value>The data.</value>
		public byte[] Data
		{
			get { return data; }
		}

		/// <summary>
		/// Gets or sets the <see cref="System.Byte"/> at the specified index.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		public byte this[int index]
		{
			get { return data[index]; }
			set { data[index] = value; }
		}

		/// <summary>
		/// Gets the char.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		public char GetChar(uint offset)
		{
			return (char)(data[offset]);
		}

		/// <summary>
		/// Sets the char.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		public void SetChar(uint offset, char value)
		{
			data[offset] = (byte)value;
		}

		/// <summary>
		/// Gets the chars.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="length">The length.</param>
		/// <returns></returns>
		public char[] GetChars(uint offset, uint length)
		{
			char[] value = new char[length];

			for (uint index = 0; index < length; index++) { value[index] = GetChar(offset + index); }

			return value;
		}

		/// <summary>
		/// Gets the bytes.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="length">The length.</param>
		/// <returns></returns>
		public byte[] GetBytes(uint offset, uint length)
		{
			byte[] value = new byte[length];

			for (uint index = 0; index < length; index++) { value[index] = data[offset + index]; }

			return value;
		}

		/// <summary>
		/// Sets the chars.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		public void SetChars(uint offset, char[] value)
		{
			for (uint index = 0; index < value.Length; index++) { data[offset + index] = (byte)value[index]; }
		}

		/// <summary>
		/// Sets the string.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		public void SetString(uint offset, string value)
		{
			for (int index = 0; index < value.Length; index++) { data[offset + index] = (byte)value[index]; }
		}

		/// <summary>
		/// Sets the string.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		/// <param name="length">The length.</param>
		public void SetString(uint offset, string value, uint length)
		{
			for (int index = 0; index < length; index++) { data[offset + index] = (byte)value[index]; }
		}

		/// <summary>
		/// Gets the usigned int.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		public uint GetUInt(uint offset)
		{
			uint value = data[offset++];
			value += (uint)(data[offset++] << 8);
			value += (uint)(data[offset++] << 16);
			value += (uint)(data[offset++] << 24);

			return value;
		}

		/// <summary>
		/// Sets the unsigned int.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		public void SetUInt(uint offset, uint value)
		{
			data[offset++] = (byte)(value & 0xFF);
			data[offset++] = (byte)((value >> 8) & 0xFF);
			data[offset++] = (byte)((value >> 16) & 0xFF);
			data[offset++] = (byte)((value >> 24) & 0xFF);
		}

		/// <summary>
		/// Sets the unsigned int reversed.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		public void SetUIntReversed(uint offset, uint value)
		{
			data[offset++] = (byte)((value >> 24) & 0xFF);
			data[offset++] = (byte)((value >> 16) & 0xFF);
			data[offset++] = (byte)((value >> 8) & 0xFF);
			data[offset++] = (byte)(value & 0xFF);
		}

		/// <summary>
		/// Sets the unsigned long.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		public void SetULong(uint offset, ulong value)
		{
			data[offset++] = (byte)(value & 0xFF);
			data[offset++] = (byte)((value >> 8) & 0xFF);
			data[offset++] = (byte)((value >> 16) & 0xFF);
			data[offset++] = (byte)((value >> 24) & 0xFF);
			data[offset++] = (byte)((value >> 32) & 0xFF);
			data[offset++] = (byte)((value >> 40) & 0xFF);
			data[offset++] = (byte)((value >> 48) & 0xFF);
			data[offset++] = (byte)((value >> 56) & 0xFF);
		}

		/// <summary>
		/// Sets the unsigned long reversed.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		public void SetULongReversed(uint offset, ulong value)
		{
			data[offset++] = (byte)((value >> 56) & 0xFF);
			data[offset++] = (byte)((value >> 48) & 0xFF);
			data[offset++] = (byte)((value >> 40) & 0xFF);
			data[offset++] = (byte)((value >> 32) & 0xFF);
			data[offset++] = (byte)((value >> 24) & 0xFF);
			data[offset++] = (byte)((value >> 16) & 0xFF);
			data[offset++] = (byte)((value >> 8) & 0xFF);
			data[offset++] = (byte)(value & 0xFF);
		}

		/// <summary>
		/// Gets the unsigned short.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		public ushort GetUShort(uint offset)
		{
			return (ushort)(data[offset++] | (data[offset++] << 8));
		}

		/// <summary>
		/// Sets the unsigned short.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		public void SetUShort(uint offset, ushort value)
		{
			data[offset++] = (byte)(value & 0xFF);
			data[offset++] = (byte)((value >> 8) & 0xFF);
		}

		/// <summary>
		/// Sets the unsigned short reversed.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		public void SetUShortReversed(uint offset, ushort value)
		{
			data[offset++] = (byte)((value >> 8) & 0xFF);
			data[offset++] = (byte)(value & 0xFF);
		}

		/// <summary>
		/// Gets the unsigned long.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		public ulong GetULong(uint offset)
		{
			ulong value = data[offset++];
			value += (uint)(data[offset++] << 8);
			value += (uint)(data[offset++] << 16);
			value += (uint)(data[offset++] << 24);
			value += (uint)(data[offset++] << 32);
			value += (uint)(data[offset++] << 40);
			value += (uint)(data[offset++] << 48);
			value += (uint)(data[offset++] << 56);

			return value;
		}

		/// <summary>
		/// Sets the unsigned long.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		public void SetULong(ulong offset, ulong value)
		{
			data[offset++] = (byte)(value & 0xFF);
			data[offset++] = (byte)((value >> 8) & 0xFF);
			data[offset++] = (byte)((value >> 16) & 0xFF);
			data[offset++] = (byte)((value >> 24) & 0xFF);
			data[offset++] = (byte)((value >> 32) & 0xFF);
			data[offset++] = (byte)((value >> 40) & 0xFF);
			data[offset++] = (byte)((value >> 48) & 0xFF);
			data[offset++] = (byte)((value >> 56) & 0xFF);
		}

		/// <summary>
		/// Gets the byte.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		public byte GetByte(uint offset)
		{
			return data[offset];
		}

		/// <summary>
		/// Sets the byte.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		public void SetByte(uint offset, byte value)
		{
			data[offset] = value;
		}

		/// <summary>
		/// Sets the bytes.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		public void SetBytes(uint offset, byte[] value)
		{
			for (uint index = 0; index < value.Length; index++) { data[offset + index] = value[index]; }
		}

		/// <summary>
		/// Sets the bytes.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		/// <param name="start">The start.</param>
		/// <param name="length">The length.</param>
		public void SetBytes(uint offset, byte[] value, uint start, uint length)
		{
			for (uint index = 0; index < length; index++) { data[offset + index] = value[start + index]; }
		}

		/// <summary>
		/// Fills the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="value">The value.</param>
		/// <param name="length">The length.</param>
		public void Fill(uint offset, byte value, uint length)
		{
			for (uint index = 0; index < length; index++) { data[offset + index] = value; }
		}

		/// <summary>
		/// Gets the string.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="length">The length.</param>
		/// <returns></returns>
		public string GetString(uint offset, uint length)
		{
			return new ASCIIEncoding().GetString(data, (int)offset, (int)length);
		}
	}
}