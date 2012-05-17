/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
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
		public static void WriteTo(this Stream src, Stream dest)
		{
			int size = (src.CanSeek) ? System.Math.Min((int)(src.Length - src.Position), 0x2000) : 0x2000;
			byte[] buffer = new byte[size];
			int n;
			do
			{
				n = src.Read(buffer, 0, buffer.Length);
				dest.Write(buffer, 0, n);
			} while (n != 0);
		}

		public static uint ReadUInt32(this Stream src, bool asLittleEndian)
		{
			int a = src.ReadByte();
			int b = src.ReadByte();
			int c = src.ReadByte();
			int d = src.ReadByte();

			if (a == -1 || b == -1 || c == -1 || d == -1)
				throw new EndOfStreamException();

			if (asLittleEndian)
				return (uint)(a | (b << 8) | (c << 16) | (d << 24));
			else
				return (uint)(d | (c << 8) | (b << 16) | (a << 24));
		}

		public static ushort ReadUInt16(this Stream src, bool asLittleEndian)
		{
			int a = src.ReadByte();
			int b = src.ReadByte();

			if (a == -1 || b == -1)
				throw new EndOfStreamException();

			if (asLittleEndian)
				return (ushort)(a | (b << 8));
			else
				return (ushort)(b | (a << 8));
		}

		public static void Write(this Stream src, ushort value, bool asLittleEndian)
		{
			if (asLittleEndian)
			{
				src.WriteByte((byte)(value & 0xFF));
				src.WriteByte((byte)((value >> 8) & 0xFF));
			}
			else
			{
				src.WriteByte((byte)((value >> 8) & 0xFF));
				src.WriteByte((byte)(value & 0xFF));
			}
		}

		public static void Write(this Stream src, short value, bool asLittleEndian)
		{
			src.Write((ushort)value, asLittleEndian);
		}

		public static void Write(this Stream src, uint value, bool asLittleEndian)
		{
			if (asLittleEndian)
			{
				src.WriteByte((byte)(value & 0xFF));
				src.WriteByte((byte)((value >> 8) & 0xFF));
				src.WriteByte((byte)((value >> 16) & 0xFF));
				src.WriteByte((byte)((value >> 24) & 0xFF));
			}
			else
			{
				src.WriteByte((byte)((value >> 24) & 0xFF));
				src.WriteByte((byte)((value >> 16) & 0xFF));
				src.WriteByte((byte)((value >> 8) & 0xFF));
				src.WriteByte((byte)(value & 0xFF));
			}
		}

		public static void Write(this Stream src, int value, bool asLittleEndian)
		{
			src.Write((uint)value, asLittleEndian);
		}

		public static void Write(this Stream src, ulong value, bool asLittleEndian)
		{
			if (asLittleEndian)
			{
				src.WriteByte((byte)(value & 0xFF));
				src.WriteByte((byte)((value >> 8) & 0xFF));
				src.WriteByte((byte)((value >> 16) & 0xFF));
				src.WriteByte((byte)((value >> 24) & 0xFF));
				src.WriteByte((byte)((value >> 32) & 0xFF));
				src.WriteByte((byte)((value >> 40) & 0xFF));
				src.WriteByte((byte)((value >> 48) & 0xFF));
				src.WriteByte((byte)((value >> 56) & 0xFF));
			}
			else
			{
				src.WriteByte((byte)((value >> 56) & 0xFF));
				src.WriteByte((byte)((value >> 48) & 0xFF));
				src.WriteByte((byte)((value >> 40) & 0xFF));
				src.WriteByte((byte)((value >> 32) & 0xFF));
				src.WriteByte((byte)((value >> 24) & 0xFF));
				src.WriteByte((byte)((value >> 16) & 0xFF));
				src.WriteByte((byte)((value >> 8) & 0xFF));
				src.WriteByte((byte)(value & 0xFF));
			}
		}

		public static void Write(this Stream src, long value, bool asLittleEndian)
		{
			src.Write((ulong)value, asLittleEndian);
		}


	}
}
