// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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

		/// <summary>
        /// Returns a Unicode character converted from two bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array.</param>
        /// <param name="startIndex">The starting position within <paramref name="value"/>.</param>
        /// <returns>A character formed by two bytes beginning at <paramref name="startIndex"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="startIndex"/> equals the length of <paramref name="value"/> minus 1.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than the length of <paramref name="value"/> minus 1.</exception>
        public static char ToChar(byte[] value, int startIndex) => unchecked((char)ToInt16(value, startIndex));

        /// <summary>
        /// Converts a read-only byte span into a character.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <returns>A character representing the converted bytes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The length of <paramref name="value"/> is less than the length of a <see cref="char"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ToChar(ReadOnlySpan<byte> value)
        {
            if (value.Length < sizeof(char))
                throw new ArgumentOutOfRangeException(nameof(value));
            return Unsafe.ReadUnaligned<char>(ref MemoryMarshal.GetReference(value));
        }

        /// <summary>
        /// Returns a 16-bit signed integer converted from two bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes.</param>
        /// <param name="startIndex">The starting position within <paramref name="value"/>.</param>
        /// <returns>A 16-bit signed integer formed by two bytes beginning at <paramref name="startIndex"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="startIndex"/> equals the length of <paramref name="value"/> minus 1.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than the length of <paramref name="value"/> minus 1.</exception>
        public static short ToInt16(byte[] value, int startIndex)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (unchecked((uint)startIndex) >= unchecked((uint)value.Length))
                throw new ArgumentOutOfRangeException(nameof(startIndex), "Index must be less than the number of values.");
            if (startIndex > value.Length - sizeof(short))
                throw new ArgumentException("Array + offset is too small.", nameof(value));

            return Unsafe.ReadUnaligned<short>(ref value[startIndex]);
        }

        /// <summary>
        /// Converts a read-only byte span into a 16-bit signed integer.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <returns>A 16-bit signed integer representing the converted bytes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The length of <paramref name="value"/> is less than 2.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ToInt16(ReadOnlySpan<byte> value)
        {
            if (value.Length < sizeof(short))
                throw new ArgumentOutOfRangeException(nameof(value));
            return Unsafe.ReadUnaligned<short>(ref MemoryMarshal.GetReference(value));
        }

        /// <summary>
        /// Returns a 32-bit signed integer converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes.</param>
        /// <param name="startIndex">The starting position within <paramref name="value"/>.</param>
        /// <returns>A 32-bit signed integer formed by four bytes beginning at <paramref name="startIndex"/>.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="startIndex"/> is greater than or equal to the length of <paramref name="value"/> minus 3,
        /// and is less than or equal to the length of <paramref name="value"/> minus 1.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than the length of <paramref name="value"/> minus 1.</exception>
        public static int ToInt32(byte[] value, int startIndex)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (unchecked((uint)startIndex) >= unchecked((uint)value.Length))
                throw new ArgumentOutOfRangeException(nameof(startIndex), "Index must be less than the number of values.");
            if (startIndex > value.Length - sizeof(int))
                throw new ArgumentException("Array + offset is too small.", nameof(value));

            return Unsafe.ReadUnaligned<int>(ref value[startIndex]);
        }

        /// <summary>
        /// Converts a read-only byte span into a 32-bit signed integer.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <returns>A 32-bit signed integer representing the converted bytes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The length of <paramref name="value"/> is less than 4.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt32(ReadOnlySpan<byte> value)
        {
            if (value.Length < sizeof(int))
                throw new ArgumentOutOfRangeException(nameof(value));
            return Unsafe.ReadUnaligned<int>(ref MemoryMarshal.GetReference(value));
        }

        /// <summary>
        /// Returns a 64-bit signed integer converted from eight bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes.</param>
        /// <param name="startIndex">The starting position within <paramref name="value"/>.</param>
        /// <returns>A 64-bit signed integer formed by eight bytes beginning at <paramref name="startIndex"/>.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="startIndex"/> is greater than or equal to the length of <paramref name="value"/> minus 7,
        /// and is less than or equal to the length of <paramref name="value"/> minus 1.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than the length of <paramref name="value"/> minus 1.</exception>
        public static long ToInt64(byte[] value, int startIndex)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (unchecked((uint)startIndex) >= unchecked((uint)value.Length))
                throw new ArgumentOutOfRangeException(nameof(startIndex), "Index must be less than the number of values.");
            if (startIndex > value.Length - sizeof(long))
                throw new ArgumentException("Array + offset is too small.", nameof(value));

            return Unsafe.ReadUnaligned<long>(ref value[startIndex]);
        }

        /// <summary>
        /// Converts a read-only byte span into a 64-bit signed integer.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <returns>A 64-bit signed integer representing the converted bytes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The length of <paramref name="value"/> is less than 8.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ToInt64(ReadOnlySpan<byte> value)
        {
            if (value.Length < sizeof(long))
                throw new ArgumentOutOfRangeException(nameof(value));
            return Unsafe.ReadUnaligned<long>(ref MemoryMarshal.GetReference(value));
        }

        /// <summary>
        /// Returns a 16-bit unsigned integer converted from two bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes.</param>
        /// <param name="startIndex">The starting position within <paramref name="value"/>.</param>
        /// <returns>A 16-bit unsigned integer formed by two bytes beginning at <paramref name="startIndex"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="startIndex"/> equals the length of <paramref name="value"/> minus 1.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than the length of <paramref name="value"/> minus 1.</exception>
        [CLSCompliant(false)]
        public static ushort ToUInt16(byte[] value, int startIndex) => unchecked((ushort)ToInt16(value, startIndex));

        /// <summary>
        /// Converts a read-only byte span into a 16-bit unsigned integer.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <returns>A 16-bit unsigned integer representing the converted bytes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The length of <paramref name="value"/> is less than 2.</exception>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ToUInt16(ReadOnlySpan<byte> value)
        {
            if (value.Length < sizeof(ushort))
                throw new ArgumentOutOfRangeException(nameof(value));
            return Unsafe.ReadUnaligned<ushort>(ref MemoryMarshal.GetReference(value));
        }

        /// <summary>
        /// Returns a 32-bit unsigned integer converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes.</param>
        /// <param name="startIndex">The starting position within <paramref name="value"/>.</param>
        /// <returns>A 32-bit unsigned integer formed by four bytes beginning at <paramref name="startIndex"/>.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="startIndex"/> is greater than or equal to the length of <paramref name="value"/> minus 3,
        /// and is less than or equal to the length of <paramref name="value"/> minus 1.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than the length of <paramref name="value"/> minus 1.</exception>
        [CLSCompliant(false)]
        public static uint ToUInt32(byte[] value, int startIndex) => unchecked((uint)ToInt32(value, startIndex));

        /// <summary>
        /// Converts a read-only byte span into a 32-bit unsigned integer.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <returns>A 32-bit unsigned integer representing the converted bytes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The length of <paramref name="value"/> is less than 4.</exception>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ToUInt32(ReadOnlySpan<byte> value)
        {
            if (value.Length < sizeof(uint))
                throw new ArgumentOutOfRangeException(nameof(value));
            return Unsafe.ReadUnaligned<uint>(ref MemoryMarshal.GetReference(value));
        }

        /// <summary>
        /// Returns a 64-bit unsigned integer converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes.</param>
        /// <param name="startIndex">The starting position within <paramref name="value"/>.</param>
        /// <returns>A 64-bit unsigned integer formed by eight bytes beginning at <paramref name="startIndex"/>.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="startIndex"/> is greater than or equal to the length of <paramref name="value"/> minus 7,
        /// and is less than or equal to the length of <paramref name="value"/> minus 1.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than the length of <paramref name="value"/> minus 1.</exception>
        [CLSCompliant(false)]
        public static ulong ToUInt64(byte[] value, int startIndex) => unchecked((ulong)ToInt64(value, startIndex));

        /// <summary>
        /// Converts a read-only byte span into a 64-bit unsigned integer.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <returns>A 64-bit unsigned integer representing the converted bytes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The length of <paramref name="value"/> is less than 8.</exception>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ToUInt64(ReadOnlySpan<byte> value)
        {
            if (value.Length < sizeof(ulong))
                throw new ArgumentOutOfRangeException(nameof(value));
            return Unsafe.ReadUnaligned<ulong>(ref MemoryMarshal.GetReference(value));
        }

        /// <summary>
        /// Returns a single-precision floating point number converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes.</param>
        /// <param name="startIndex">The starting position within <paramref name="value"/>.</param>
        /// <returns>A single-precision floating point number formed by four bytes beginning at <paramref name="startIndex"/>.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="startIndex"/> is greater than or equal to the length of <paramref name="value"/> minus 3,
        /// and is less than or equal to the length of <paramref name="value"/> minus 1.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than the length of <paramref name="value"/> minus 1.</exception>
        public static float ToSingle(byte[] value, int startIndex) => Int32BitsToSingle(ToInt32(value, startIndex));

        /// <summary>
        /// Converts a read-only byte span into a single-precision floating-point value.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <returns>A single-precision floating-point value representing the converted bytes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The length of <paramref name="value"/> is less than 4.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToSingle(ReadOnlySpan<byte> value)
        {
            if (value.Length < sizeof(float))
                throw new ArgumentOutOfRangeException(nameof(value));
            return Unsafe.ReadUnaligned<float>(ref MemoryMarshal.GetReference(value));
        }

        /// <summary>
        /// Returns a double-precision floating point number converted from four bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes.</param>
        /// <param name="startIndex">The starting position within <paramref name="value"/>.</param>
        /// <returns>A double-precision floating point number formed by eight bytes beginning at <paramref name="startIndex"/>.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="startIndex"/> is greater than or equal to the length of <paramref name="value"/> minus 7,
        /// and is less than or equal to the length of <paramref name="value"/> minus 1.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or greater than the length of <paramref name="value"/> minus 1.</exception>
        public static double ToDouble(byte[] value, int startIndex) => Int64BitsToDouble(ToInt64(value, startIndex));

        /// <summary>
        /// Converts a read-only byte span into a double-precision floating-point value.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <returns>A double-precision floating-point value representing the converted bytes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The length of <paramref name="value"/> is less than 8.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToDouble(ReadOnlySpan<byte> value)
        {
            if (value.Length < sizeof(double))
                throw new ArgumentOutOfRangeException(nameof(value));
            return Unsafe.ReadUnaligned<double>(ref MemoryMarshal.GetReference(value));
        }
	}
}
