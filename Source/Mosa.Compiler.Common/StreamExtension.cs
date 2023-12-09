// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Common;

public static class StreamExtension
{
	/// <summary>
	/// Writes the specified stream.
	/// </summary>
	/// <param name="stream">The stream.</param>
	/// <param name="buffer">The buffer.</param>
	public static void Write(this Stream stream, byte[] buffer)
	{
		stream.Write(buffer, 0, buffer.Length);
	}

	/// <summary>
	/// Writes the zero bytes.
	/// </summary>
	/// <param name="stream">The stream.</param>
	/// <param name="size">The size.</param>
	public static void WriteZeroBytes(this Stream stream, uint size)
	{
		for (int i = 0; i < size; i++)
			stream.WriteByte(0);
	}

	/// <summary>
	/// Writes to from MemoryStream to stream
	/// </summary>
	/// <param name="stream">The stream.</param>
	/// <param name="memoryStream">The memory stream.</param>
	public static void WriteTo(this Stream stream, Stream dest)
	{
		int size = stream.CanSeek ? System.Math.Min((int)(stream.Length - stream.Position), 0x2000) : 0x2000;
		byte[] buffer = new byte[size];
		int n;
		do
		{
			n = stream.Read(buffer, 0, buffer.Length);
			dest.Write(buffer, 0, n);
		} while (n != 0);
	}

	public static void Write8(this Stream stream, byte value)
	{
		stream.WriteByte(value);
	}

	public static void Write16(this Stream stream, ushort value)
	{
		stream.WriteByte((byte)(value & 0xFF));
		stream.WriteByte((byte)((value >> 8) & 0xFF));
	}

	public static void Write16(this Stream stream, short value)
	{
		stream.Write16((ushort)value);
	}

	public static void Write32(this Stream stream, uint value)
	{
		stream.WriteByte((byte)(value & 0xFF));
		stream.WriteByte((byte)((value >> 8) & 0xFF));
		stream.WriteByte((byte)((value >> 16) & 0xFF));
		stream.WriteByte((byte)((value >> 24) & 0xFF));
	}

	public static void Write32(this Stream stream, int value)
	{
		stream.Write32((uint)value);
	}

	public static void Write64(this Stream stream, ulong value)
	{
		stream.WriteByte((byte)(value & 0xFF));
		stream.WriteByte((byte)((value >> 8) & 0xFF));
		stream.WriteByte((byte)((value >> 16) & 0xFF));
		stream.WriteByte((byte)((value >> 24) & 0xFF));
		stream.WriteByte((byte)((value >> 32) & 0xFF));
		stream.WriteByte((byte)((value >> 40) & 0xFF));
		stream.WriteByte((byte)((value >> 48) & 0xFF));
		stream.WriteByte((byte)((value >> 56) & 0xFF));
	}

	public static void Write64(this Stream stream, long value)
	{
		stream.Write64((ulong)value);
	}

	public static int Read8(this Stream stream)
	{
		return stream.ReadByte();
	}

	public static int Read16(this Stream stream)
	{
		return stream.ReadByte() | (stream.ReadByte() << 8);
	}

	public static int Read32(this Stream stream)
	{
		return stream.ReadByte() | (stream.ReadByte() << 8) | (stream.ReadByte() << 16) | (stream.ReadByte() << 24);
	}
}
