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
			return GetBytes((short)value);
		}

		// Converts a short into an array of bytes with length two.
		public unsafe static byte[] GetBytes(short value)
		{
			//byte[] bytes = new byte[sizeof(short)];
			//Unsafe.As<byte, short>(ref bytes[0]) = value;
			//return bytes;
			byte[] bytes = new byte[2];
			fixed (byte* b = bytes)
				*((short*)b) = value;
			return bytes;
		}

		// Converts an int into an array of bytes with length
		// four.
		public unsafe static byte[] GetBytes(int value)
		{
			byte[] bytes = new byte[4];
			fixed (byte* b = bytes)
				*((int*)b) = value;
			return bytes;
		}

		// Converts a long into an array of bytes with length
		// eight.
		public unsafe static byte[] GetBytes(long value)
		{
			byte[] bytes = new byte[8];
			fixed (byte* b = bytes)
				*((long*)b) = value;
			return bytes;
		}

		// Converts an ushort into an array of bytes with
		// length two.
		public static byte[] GetBytes(ushort value)
		{
			return GetBytes((short)value);
		}

		// Converts an uint into an array of bytes with
		// length four.
		public static byte[] GetBytes(uint value)
		{
			return GetBytes((int)value);
		}

		// Converts an unsigned long into an array of bytes with
		// length eight.
		public static byte[] GetBytes(ulong value)
		{
			return GetBytes((long)value);
		}

		// Converts a float into an array of bytes with length
		// four.
		public unsafe static byte[] GetBytes(float value)
		{
			return GetBytes(*(int*)&value);
		}

		// Converts a double into an array of bytes with length
		// eight.
		public unsafe static byte[] GetBytes(double value)
		{
			return GetBytes(*(long*)&value);
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
