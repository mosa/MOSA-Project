// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Common
{
	public static class EndianAwareBinaryWriterExtensions
	{
		public static void Write(this EndianAwareBinaryWriter writer, byte[] value, int nativeSize)
		{
			byte[] bytesToWrite = new byte[nativeSize];
			for (int i = 0; i < nativeSize && i < value.Length; i++)
				bytesToWrite[i] = value[i];
			writer.Write(bytesToWrite);
		}

		public static void Write(this EndianAwareBinaryWriter writer, bool value, int nativeSize)
		{
			Write(writer, BitConverter.GetBytes(value), nativeSize);
		}

		public static void Write(this EndianAwareBinaryWriter writer, sbyte value, int nativeSize)
		{
			Write(writer, BitConverter.GetBytes(value), nativeSize);
		}

		public static void Write(this EndianAwareBinaryWriter writer, char value, int nativeSize)
		{
			Write(writer, BitConverter.GetBytes(value), nativeSize);
		}

		public static void Write(this EndianAwareBinaryWriter writer, short value, int nativeSize)
		{
			Write(writer, BitConverter.GetBytes(value), nativeSize);
		}

		public static void Write(this EndianAwareBinaryWriter writer, int value, int nativeSize)
		{
			Write(writer, BitConverter.GetBytes(value), nativeSize);
		}

		public static void Write(this EndianAwareBinaryWriter writer, long value, int nativeSize)
		{
			Write(writer, BitConverter.GetBytes(value), nativeSize);
		}

		public static void Write(this EndianAwareBinaryWriter writer, byte value, int nativeSize)
		{
			Write(writer, BitConverter.GetBytes(value), nativeSize);
		}

		public static void Write(this EndianAwareBinaryWriter writer, ushort value, int nativeSize)
		{
			Write(writer, BitConverter.GetBytes(value), nativeSize);
		}

		public static void Write(this EndianAwareBinaryWriter writer, uint value, int nativeSize)
		{
			Write(writer, BitConverter.GetBytes(value), nativeSize);
		}

		public static void Write(this EndianAwareBinaryWriter writer, ulong value, int nativeSize)
		{
			Write(writer, BitConverter.GetBytes(value), nativeSize);
		}
	}
}
