// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.IO;

namespace Mosa.Compiler.Common
{
	public static class BinaryWriterExtensions
	{
		public static long GetPosition(this BinaryWriter writer)
		{
			return writer.BaseStream.Position;
		}

		public static void SetPosition(this BinaryWriter writer, long position)
		{
			writer.BaseStream.Position = position;
		}

		public static void WriteZeroBytes(this BinaryWriter writer, int size)
		{
			for (int i = 0; i < size; i++)
				writer.Write((byte)0);
		}

		public static void WriteZeroBytes(this BinaryWriter writer, uint size)
		{
			for (uint i = 0; i < size; i++)
				writer.Write((byte)0);
		}

		public static void Write(this BinaryWriter writer, byte[] value, int nativeSize)
		{
			var bytesToWrite = new byte[nativeSize];

			for (int i = 0; i < nativeSize && i < value.Length; i++)
			{
				bytesToWrite[i] = value[i];
			}

			writer.Write(bytesToWrite);
		}

		public static void Write(this BinaryWriter writer, bool value, int nativeSize)
		{
			Write(writer, BitConverter.GetBytes(value), nativeSize);
		}

		public static void Write(this BinaryWriter writer, sbyte value, int nativeSize)
		{
			Write(writer, BitConverter.GetBytes(value), nativeSize);
		}

		public static void Write(this BinaryWriter writer, char value, int nativeSize)
		{
			Write(writer, BitConverter.GetBytes(value), nativeSize);
		}

		public static void Write(this BinaryWriter writer, short value, int nativeSize)
		{
			Write(writer, BitConverter.GetBytes(value), nativeSize);
		}

		public static void Write(this BinaryWriter writer, int value, int nativeSize)
		{
			Write(writer, BitConverter.GetBytes(value), nativeSize);
		}

		public static void Write(this BinaryWriter writer, long value, int nativeSize)
		{
			Write(writer, BitConverter.GetBytes(value), nativeSize);
		}

		public static void Write(this BinaryWriter writer, byte value, int nativeSize)
		{
			Write(writer, BitConverter.GetBytes(value), nativeSize);
		}

		public static void Write(this BinaryWriter writer, ushort value, int nativeSize)
		{
			Write(writer, BitConverter.GetBytes(value), nativeSize);
		}

		public static void Write(this BinaryWriter writer, uint value, int nativeSize)
		{
			Write(writer, BitConverter.GetBytes(value), nativeSize);
		}

		public static void Write(this BinaryWriter writer, ulong value, int nativeSize)
		{
			Write(writer, BitConverter.GetBytes(value), nativeSize);
		}

		public static void WriteByte(this BinaryWriter stream, byte value)
		{
			stream.Write(value);
		}

		public static void WriteByte(this BinaryWriter stream, int value)
		{
			stream.Write((byte)value);
		}

	}
}
