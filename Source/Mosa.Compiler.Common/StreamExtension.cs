/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.IO;

namespace Mosa.Compiler.Common
{
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
		public static void WriteZeroBytes(this Stream stream, int size)
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
			int size = (stream.CanSeek) ? System.Math.Min((int)(stream.Length - stream.Position), 0x2000) : 0x2000;
			byte[] buffer = new byte[size];
			int n;
			do
			{
				n = stream.Read(buffer, 0, buffer.Length);
				dest.Write(buffer, 0, n);
			} while (n != 0);
		}

		public static ushort ReadUInt16(this Stream stream, Endianness endianness)
		{
			int a = stream.ReadByte();
			int b = stream.ReadByte();

			if (a == -1 || b == -1)
				throw new EndOfStreamException();

			if (endianness == Endianness.Little)
				return (ushort)(a | (b << 8));
			else
				return (ushort)(b | (a << 8));
		}

		public static uint ReadUInt32(this Stream stream, Endianness endianness)
		{
			int a = stream.ReadByte();
			int b = stream.ReadByte();
			int c = stream.ReadByte();
			int d = stream.ReadByte();

			if (a == -1 || b == -1 || c == -1 || d == -1)
				throw new EndOfStreamException();

			if (endianness == Endianness.Little)
				return (uint)(a | (b << 8) | (c << 16) | (d << 24));
			else
				return (uint)(d | (c << 8) | (b << 16) | (a << 24));
		}

		public static uint ReadUInt64(this Stream stream, Endianness endianness)
		{
			int a = stream.ReadByte();
			int b = stream.ReadByte();
			int c = stream.ReadByte();
			int d = stream.ReadByte();
			int e = stream.ReadByte();
			int f = stream.ReadByte();
			int g = stream.ReadByte();
			int h = stream.ReadByte();

			if (a == -1 || b == -1 || c == -1 || d == -1 || e == -1 || f == -1 || g == -1 || h == -1)
				throw new EndOfStreamException();

			if (endianness == Endianness.Little)
				return (uint)(a | (b << 8) | (c << 16) | (d << 24) | (e << 32) | (f << 40) | (g << 48) | (h << 56));
			else
				return (uint)(h | (g << 8) | (f << 16) | (e << 24) | (d << 32) | (c << 40) | (b << 48) | (a << 56));
		}

		public static void Write(this Stream stream, ushort value, Endianness endianness)
		{
			if (endianness == Endianness.Little)
			{
				stream.WriteByte((byte)(value & 0xFF));
				stream.WriteByte((byte)((value >> 8) & 0xFF));
			}
			else
			{
				stream.WriteByte((byte)((value >> 8) & 0xFF));
				stream.WriteByte((byte)(value & 0xFF));
			}
		}

		public static void Write(this Stream stream, short value, Endianness endianness)
		{
			stream.Write((ushort)value, endianness);
		}

		public static void Write(this Stream stream, uint value, Endianness endianness)
		{
			if (endianness == Endianness.Little)
			{
				stream.WriteByte((byte)(value & 0xFF));
				stream.WriteByte((byte)((value >> 8) & 0xFF));
				stream.WriteByte((byte)((value >> 16) & 0xFF));
				stream.WriteByte((byte)((value >> 24) & 0xFF));
			}
			else
			{
				stream.WriteByte((byte)((value >> 24) & 0xFF));
				stream.WriteByte((byte)((value >> 16) & 0xFF));
				stream.WriteByte((byte)((value >> 8) & 0xFF));
				stream.WriteByte((byte)(value & 0xFF));
			}
		}

		public static void Write(this Stream stream, int value, Endianness endianness)
		{
			stream.Write((uint)value, endianness);
		}

		public static void Write(this Stream stream, ulong value, Endianness endianness)
		{
			if (endianness == Endianness.Little)
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
			else
			{
				stream.WriteByte((byte)((value >> 56) & 0xFF));
				stream.WriteByte((byte)((value >> 48) & 0xFF));
				stream.WriteByte((byte)((value >> 40) & 0xFF));
				stream.WriteByte((byte)((value >> 32) & 0xFF));
				stream.WriteByte((byte)((value >> 24) & 0xFF));
				stream.WriteByte((byte)((value >> 16) & 0xFF));
				stream.WriteByte((byte)((value >> 8) & 0xFF));
				stream.WriteByte((byte)(value & 0xFF));
			}
		}

		public static void Write(this Stream stream, long value, Endianness endianness)
		{
			stream.Write((ulong)value, endianness);
		}
	}
}