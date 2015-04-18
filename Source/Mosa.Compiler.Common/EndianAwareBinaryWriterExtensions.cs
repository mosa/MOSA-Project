/*
 * (c) 2015 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

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
