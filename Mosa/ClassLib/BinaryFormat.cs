/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.ClassLib
{
    /// <summary>
    /// 
    /// </summary>
	public struct BinaryFormat
	{
        /// <summary>
        /// 
        /// </summary>
		private byte[] data;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
		public BinaryFormat(byte[] data) { this.data = data; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
		public BinaryFormat(uint length) { this.data = new byte[length]; }

        /// <summary>
        /// 
        /// </summary>
		public uint Length
		{
			get { return (uint)data.Length; }
		}

        /// <summary>
        /// 
        /// </summary>
		public byte[] Data
		{
			get { return data; }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
		public byte this[int index]
		{
			get { return data[index]; }
			set { data[index] = value; }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
		public char GetChar(uint offset) { return (char)(data[offset]); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
		public void SetChar(uint offset, char value) { data[offset] = (byte)value; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
		public char[] GetChars(uint offset, uint length)
		{
			char[] value = new char[length];

			for (uint index = 0; index < length; index++) { value[index] = GetChar(offset + index); }

			return value;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
		public byte[] GetBytes(uint offset, uint length)
		{
			byte[] value = new byte[length];

			for (uint index = 0; index < length; index++) { value[index] = data[offset + index]; }

			return value;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
		public void SetChars(uint offset, char[] value)
		{
			for (uint index = 0; index < value.Length; index++) { data[offset + index] = (byte)value[index]; }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
		public void SetString(uint offset, string value)
		{
			for (int index = 0; index < value.Length; index++) { data[offset + index] = (byte)value[index]; }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <param name="length"></param>
		public void SetString(uint offset, string value, uint length)
		{
			for (int index = 0; index < length; index++) { data[offset + index] = (byte)value[index]; }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
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
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
		public void SetUInt(uint offset, uint value)
		{
			data[offset++] = (byte)(value & 0xFF);
			data[offset++] = (byte)((value >> 8) & 0xFF);
			data[offset++] = (byte)((value >> 16) & 0xFF);
			data[offset++] = (byte)((value >> 24) & 0xFF);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
		public ushort GetUShort(uint offset)
		{
			ushort value = data[offset++];
			value += (ushort)(data[offset++] << 8);

			return value;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
		public void SetUShort(uint offset, ushort value)
		{
			data[offset++] = (byte)(value & 0xFF);
			data[offset++] = (byte)((value >> 8) & 0xFF);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
		public int GetInt(uint offset) { return (int)GetUInt(offset); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
		public void SetInt(uint offset, int value) { SetUInt(offset, (uint)value); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
		public byte GetByte(uint offset) { return data[offset]; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
		public void SetByte(uint offset, byte value) { data[offset] = value; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
		public void SetBytes(uint offset, byte[] value)
		{
			for (uint index = 0; index < value.Length; index++) { data[offset] = value[index]; }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
		public void SetBytes(uint offset, byte[] value, uint start, uint length)
		{
			for (uint index = 0; index < length; index++) { data[offset] = value[start + index]; }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <param name="length"></param>
		public void Fill(uint offset, byte value, uint length)
		{
			for (uint index = 0; index < length; index++) { data[offset] = value; }
		}
	}
}
