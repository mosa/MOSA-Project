// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace System
{
	public static class BitConverter
	{
		public static readonly bool IsLittleEndian = true;

		// Converts a Boolean into an array of bytes with length one.
		public static byte[] GetBytes(bool value)
		{
			byte[] r = new byte[1];
			r[0] = (value ? (byte)1 : (byte)0);
			return r;
		}

		// Converts a char into an array of bytes with length two.
		public static byte[] GetBytes(char value)
		{
			byte[] bytes = new byte[sizeof(char)];
			Unsafe.As<byte, char>(ref bytes[0]) = value;
			return bytes;
		}

		// Converts a short into an array of bytes with length
		// two.
		public static byte[] GetBytes(short value)
		{
			byte[] bytes = new byte[sizeof(short)];
			Unsafe.As<byte, short>(ref bytes[0]) = value;
			return bytes;
		}

		// Converts an int into an array of bytes with length
		// four.
		public static byte[] GetBytes(int value)
		{
			byte[] bytes = new byte[sizeof(int)];
			Unsafe.As<byte, int>(ref bytes[0]) = value;
			return bytes;
		}

		// Converts a long into an array of bytes with length
		// eight.
		public static byte[] GetBytes(long value)
		{
			byte[] bytes = new byte[sizeof(long)];
			Unsafe.As<byte, long>(ref bytes[0]) = value;
			return bytes;
		}

		// Converts an ushort into an array of bytes with
		// length two.
		public static byte[] GetBytes(ushort value)
		{
			byte[] bytes = new byte[sizeof(ushort)];
			Unsafe.As<byte, ushort>(ref bytes[0]) = value;
			return bytes;
		}

		// Converts an uint into an array of bytes with
		// length four.
		public static byte[] GetBytes(uint value)
		{
			byte[] bytes = new byte[sizeof(uint)];
			Unsafe.As<byte, uint>(ref bytes[0]) = value;
			return bytes;
		}

		// Converts an unsigned long into an array of bytes with
		// length eight.
		public static byte[] GetBytes(ulong value)
		{
			byte[] bytes = new byte[sizeof(ulong)];
			Unsafe.As<byte, ulong>(ref bytes[0]) = value;
			return bytes;
		}

		// Converts a float into an array of bytes with length
		// four.
		public static byte[] GetBytes(float value)
		{
			byte[] bytes = new byte[sizeof(float)];
			Unsafe.As<byte, float>(ref bytes[0]) = value;
			return bytes;
		}

		// Converts a double into an array of bytes with length
		// eight.
		public static byte[] GetBytes(double value)
		{
			byte[] bytes = new byte[sizeof(double)];
			Unsafe.As<byte, double>(ref bytes[0]) = value;
			return bytes;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe long DoubleToInt64Bits(double value)
		{
			return *((long*)&value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe double Int64BitsToDouble(long value)
		{
			return *((double*)&value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe int SingleToInt32Bits(float value)
		{
			return *((int*)&value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe float Int32BitsToSingle(int value)
		{
			return *((float*)&value);
		}
	}
}
