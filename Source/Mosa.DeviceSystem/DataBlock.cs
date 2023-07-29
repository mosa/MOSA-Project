// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Text;

namespace Mosa.DeviceSystem;

/// <summary>
/// Data Block
/// </summary>
public class DataBlock
{
	/// <summary>
	/// Gets the data.
	/// </summary>
	/// <value>The data.</value>
	public byte[] Data { get; }

	/// <summary>
	/// Gets the length.
	/// </summary>
	/// <value>The length.</value>
	public uint Length => (uint)Data.Length;

	/// <summary>
	/// Initializes a new instance of the <see cref="DataBlock"/> struct.
	/// </summary>
	/// <param name="data">The data.</param>
	public DataBlock(byte[] data)
	{
		Data = data;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="DataBlock"/> struct.
	/// </summary>
	/// <param name="length">The length.</param>
	public DataBlock(uint length)
	{
		Data = new byte[length];
	}

	/// <summary>
	/// Gets or sets the <see cref="Byte" /> at the specified index.
	/// </summary>
	/// <value>
	/// The <see cref="Byte"/>.
	/// </value>
	/// <param name="index">The index.</param>
	/// <returns></returns>
	public byte this[int index]
	{
		get => Data[index];
		set => Data[index] = value;
	}

	/// <summary>
	/// Gets the char.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <returns></returns>
	public char GetChar(uint offset)
	{
		return (char)Data[offset];
	}

	/// <summary>
	/// Sets the char.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <param name="value">The value.</param>
	public void SetChar(uint offset, char value)
	{
		Data[offset] = (byte)value;
	}

	/// <summary>
	/// Gets the chars.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <param name="length">The length.</param>
	/// <returns></returns>
	public char[] GetChars(uint offset, uint length)
	{
		var value = new char[length];

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
		var value = new byte[length];

		for (uint index = 0; index < length; index++) { value[index] = Data[offset + index]; }

		return value;
	}

	/// <summary>
	/// Sets the chars.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <param name="value">The value.</param>
	public void SetChars(uint offset, char[] value)
	{
		for (uint index = 0; index < value.Length; index++) { Data[offset + index] = (byte)value[index]; }
	}

	/// <summary>
	/// Sets the string.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <param name="value">The value.</param>
	public void SetString(uint offset, string value)
	{
		for (int index = 0; index < value.Length; index++) { Data[offset + index] = (byte)value[index]; }
	}

	/// <summary>
	/// Sets the string.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <param name="value">The value.</param>
	/// <param name="length">The length.</param>
	public void SetString(uint offset, string value, uint length)
	{
		for (int index = 0; index < length; index++) { Data[offset + index] = (byte)value[index]; }
	}

	/// <summary>
	/// Gets the unsigned 24-bit int.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <returns></returns>
	public uint GetUInt24(uint offset)
	{
		uint value = Data[offset++];
		value += (uint)(Data[offset++] << 8);
		value += (uint)(Data[offset] << 16);

		return value;
	}

	/// <summary>
	/// Sets the unsigned 24-bit int.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <param name="value">The value.</param>
	public void SetUInt24(uint offset, uint value)
	{
		Data[offset++] = (byte)(value & 0xFF);
		Data[offset++] = (byte)((value >> 8) & 0xFF);
		Data[offset] = (byte)((value >> 16) & 0xFF);
	}

	/// <summary>
	/// Sets the unsigned int reversed.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <param name="value">The value.</param>
	public void SetUInt24Reversed(uint offset, uint value)
	{
		Data[offset++] = (byte)((value >> 16) & 0xFF);
		Data[offset++] = (byte)((value >> 8) & 0xFF);
		Data[offset] = (byte)(value & 0xFF);
	}

	/// <summary>
	/// Gets the unsigned 32-bit int.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <returns></returns>
	public uint GetUInt32(uint offset)
	{
		uint value = Data[offset++];
		value += (uint)(Data[offset++] << 8);
		value += (uint)(Data[offset++] << 16);
		value += (uint)(Data[offset] << 24);

		return value;
	}

	/// <summary>
	/// Sets the unsigned 32-bit int.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <param name="value">The value.</param>
	public void SetUInt32(uint offset, uint value)
	{
		Data[offset++] = (byte)(value & 0xFF);
		Data[offset++] = (byte)((value >> 8) & 0xFF);
		Data[offset++] = (byte)((value >> 16) & 0xFF);
		Data[offset] = (byte)((value >> 24) & 0xFF);
	}

	/// <summary>
	/// Sets the unsigned 32-bit int reversed.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <param name="value">The value.</param>
	public void SetUInt32Reversed(uint offset, uint value)
	{
		Data[offset++] = (byte)((value >> 24) & 0xFF);
		Data[offset++] = (byte)((value >> 16) & 0xFF);
		Data[offset++] = (byte)((value >> 8) & 0xFF);
		Data[offset] = (byte)(value & 0xFF);
	}

	/// <summary>
	/// Sets the unsigned long.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <param name="value">The value.</param>
	public void SetULong(uint offset, ulong value)
	{
		Data[offset++] = (byte)(value & 0xFF);
		Data[offset++] = (byte)((value >> 8) & 0xFF);
		Data[offset++] = (byte)((value >> 16) & 0xFF);
		Data[offset++] = (byte)((value >> 24) & 0xFF);
		Data[offset++] = (byte)((value >> 32) & 0xFF);
		Data[offset++] = (byte)((value >> 40) & 0xFF);
		Data[offset++] = (byte)((value >> 48) & 0xFF);
		Data[offset] = (byte)((value >> 56) & 0xFF);
	}

	/// <summary>
	/// Sets the unsigned long reversed.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <param name="value">The value.</param>
	public void SetULongReversed(uint offset, ulong value)
	{
		Data[offset++] = (byte)((value >> 56) & 0xFF);
		Data[offset++] = (byte)((value >> 48) & 0xFF);
		Data[offset++] = (byte)((value >> 40) & 0xFF);
		Data[offset++] = (byte)((value >> 32) & 0xFF);
		Data[offset++] = (byte)((value >> 24) & 0xFF);
		Data[offset++] = (byte)((value >> 16) & 0xFF);
		Data[offset++] = (byte)((value >> 8) & 0xFF);
		Data[offset] = (byte)(value & 0xFF);
	}

	/// <summary>
	/// Gets the unsigned short.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <returns></returns>
	public ushort GetUShort(uint offset)
	{
		ushort value = Data[offset++];
		value += (ushort)(Data[offset] << 8);

		return value;
	}

	/// <summary>
	/// Sets the unsigned short.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <param name="value">The value.</param>
	public void SetUShort(uint offset, ushort value)
	{
		Data[offset++] = (byte)(value & 0xFF);
		Data[offset] = (byte)((value >> 8) & 0xFF);
	}

	/// <summary>
	/// Sets the unsigned short reversed.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <param name="value">The value.</param>
	public void SetUShortReversed(uint offset, ushort value)
	{
		Data[offset++] = (byte)((value >> 8) & 0xFF);
		Data[offset] = (byte)(value & 0xFF);
	}

	/// <summary>
	/// Gets the unsigned long.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <returns></returns>
	public ulong GetULong(uint offset)
	{
		ulong value = Data[offset++];
		value += (uint)(Data[offset++] << 8);
		value += (uint)(Data[offset++] << 16);
		value += (uint)(Data[offset++] << 24);
		value += (uint)(Data[offset++] << 32);
		value += (uint)(Data[offset++] << 40);
		value += (uint)(Data[offset++] << 48);
		value += (uint)(Data[offset] << 56);

		return value;
	}

	/// <summary>
	/// Sets the unsigned long.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <param name="value">The value.</param>
	public void SetULong(ulong offset, ulong value)
	{
		Data[offset++] = (byte)(value & 0xFF);
		Data[offset++] = (byte)((value >> 8) & 0xFF);
		Data[offset++] = (byte)((value >> 16) & 0xFF);
		Data[offset++] = (byte)((value >> 24) & 0xFF);
		Data[offset++] = (byte)((value >> 32) & 0xFF);
		Data[offset++] = (byte)((value >> 40) & 0xFF);
		Data[offset++] = (byte)((value >> 48) & 0xFF);
		Data[offset] = (byte)((value >> 56) & 0xFF);
	}

	/// <summary>
	/// Gets the byte.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <returns></returns>
	public byte GetByte(uint offset)
	{
		return Data[offset];
	}

	/// <summary>
	/// Sets the byte.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <param name="value">The value.</param>
	public void SetByte(uint offset, byte value)
	{
		Data[offset] = value;
	}

	/// <summary>
	/// Sets the bytes.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <param name="value">The value.</param>
	public void SetBytes(uint offset, byte[] value)
	{
		for (uint index = 0; index < value.Length; index++) { Data[offset + index] = value[index]; }
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
		for (uint index = 0; index < length; index++) { Data[offset + index] = value[start + index]; }
	}

	/// <summary>
	/// Fills the specified offset.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <param name="value">The value.</param>
	/// <param name="length">The length.</param>
	public void Fill(uint offset, byte value, uint length)
	{
		for (uint index = 0; index < length; index++) { Data[offset + index] = value; }
	}

	/// <summary>
	/// Gets the string.
	/// </summary>
	/// <param name="offset">The offset.</param>
	/// <param name="length">The length.</param>
	/// <returns></returns>
	public string GetString(uint offset, uint length)
	{
		return Encoding.ASCII.GetString(Data, (int)offset, (int)length);
	}
}
