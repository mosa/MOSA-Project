// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;

namespace Mosa.DeviceSystem.Misc;

/// <summary>
/// Provides functions for reading and writing specific sized types of data (e.g. various sized integers or byte arrays). It also
/// provides functions for writing those types in a reserved endianness.
/// </summary>
public class DataBlock
{
	public byte[] Data { get; }

	public uint Length => (uint)Data.Length;

	public DataBlock(byte[] data) => Data = data;

	public DataBlock(uint length) => Data = new byte[length];

	public byte this[int index]
	{
		get => Data[index];
		set => Data[index] = value;
	}

	public char GetChar(uint offset) => (char)Data[offset];

	public void SetChar(uint offset, char value) => Data[offset] = (byte)value;

	public char[] GetChars(uint offset, uint length)
	{
		var value = new char[length];
		for (var index = 0U; index < length; index++)
			value[index] = GetChar(offset + index);

		return value;
	}

	public byte[] GetBytes(uint offset, uint length)
	{
		var value = new byte[length];
		for (var index = 0U; index < length; index++)
			value[index] = Data[offset + index];

		return value;
	}

	public void SetChars(uint offset, char[] value)
	{
		for (var index = 0U; index < value.Length; index++)
			Data[offset + index] = (byte)value[index];
	}

	public void SetString(uint offset, string value)
	{
		for (var index = 0; index < value.Length; index++)
			Data[offset + index] = (byte)value[index];
	}

	public void SetString(uint offset, string value, uint length)
	{
		for (var index = 0; index < length; index++)
			Data[offset + index] = (byte)value[index];
	}

	public uint GetUInt24(uint offset)
	{
		uint value = Data[offset++];
		value += (uint)(Data[offset++] << 8);
		value += (uint)(Data[offset] << 16);

		return value;
	}

	public void SetUInt24(uint offset, uint value)
	{
		Data[offset++] = (byte)(value & 0xFF);
		Data[offset++] = (byte)((value >> 8) & 0xFF);
		Data[offset] = (byte)((value >> 16) & 0xFF);
	}

	public void SetUInt24Reversed(uint offset, uint value)
	{
		Data[offset++] = (byte)((value >> 16) & 0xFF);
		Data[offset++] = (byte)((value >> 8) & 0xFF);
		Data[offset] = (byte)(value & 0xFF);
	}

	public uint GetUInt32(uint offset)
	{
		uint value = Data[offset++];
		value += (uint)(Data[offset++] << 8);
		value += (uint)(Data[offset++] << 16);
		value += (uint)(Data[offset] << 24);

		return value;
	}

	public void SetUInt32(uint offset, uint value)
	{
		Data[offset++] = (byte)(value & 0xFF);
		Data[offset++] = (byte)((value >> 8) & 0xFF);
		Data[offset++] = (byte)((value >> 16) & 0xFF);
		Data[offset] = (byte)((value >> 24) & 0xFF);
	}

	public void SetUInt32Reversed(uint offset, uint value)
	{
		Data[offset++] = (byte)((value >> 24) & 0xFF);
		Data[offset++] = (byte)((value >> 16) & 0xFF);
		Data[offset++] = (byte)((value >> 8) & 0xFF);
		Data[offset] = (byte)(value & 0xFF);
	}

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

	public ushort GetUShort(uint offset)
	{
		ushort value = Data[offset++];
		value += (ushort)(Data[offset] << 8);

		return value;
	}

	public void SetUShort(uint offset, ushort value)
	{
		Data[offset++] = (byte)(value & 0xFF);
		Data[offset] = (byte)((value >> 8) & 0xFF);
	}

	public void SetUShortReversed(uint offset, ushort value)
	{
		Data[offset++] = (byte)((value >> 8) & 0xFF);
		Data[offset] = (byte)(value & 0xFF);
	}

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

	public byte GetByte(uint offset) => Data[offset];

	public void SetByte(uint offset, byte value) => Data[offset] = value;

	public void SetBytes(uint offset, byte[] value)
	{
		for (var index = 0U; index < value.Length; index++)
			Data[offset + index] = value[index];
	}

	public void SetBytes(uint offset, byte[] value, uint start, uint length)
	{
		for (var index = 0U; index < length; index++)
			Data[offset + index] = value[start + index];
	}

	public void Fill(uint offset, byte value, uint length)
	{
		for (var index = 0U; index < length; index++)
			Data[offset + index] = value;
	}

	public string GetString(uint offset, uint length) => Encoding.ASCII.GetString(Data, (int)offset, (int)length);
}
