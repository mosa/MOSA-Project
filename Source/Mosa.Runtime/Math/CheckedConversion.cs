// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace Mosa.Runtime.Math
{
	internal static class CheckedConversion
	{
		#region I8

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long U8ToI8(ulong value)
		{
			if (value > long.MaxValue)
				Internal.ThrowOverflowException();
			return (long)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long R4ToI8(float value)
		{
			var r8Value = (double)value;
			const double two63 = 2147483648.0 * 4294967296.0;

			// 0x402 is epsilon used to get us to the next value
			if (!(r8Value > (-two63 - 0x402) && r8Value < two63))
				Internal.ThrowOverflowException();
			return (long)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long R8ToI8(double value)
		{
			const double two63 = 2147483648.0 * 4294967296.0;

			// 0x402 is epsilon used to get us to the next value
			if (!(value > (-two63 - 0x402) && value < two63))
				Internal.ThrowOverflowException();
			return (long)value;
		}

		#endregion I8

		#region U8

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong I4ToU8(long value)
		{
			if (value < 0)
				Internal.ThrowOverflowException();
			return (ulong)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong I8ToU8(long value)
		{
			if (value < 0)
				Internal.ThrowOverflowException();
			return (ulong)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong R4ToU8(float value)
		{
			var r8Value = (double)value;
			const double two64 = 4294967296.0 * 4294967296.0;
			if (!(r8Value > -1.0 && r8Value < two64))
				Internal.ThrowOverflowException();
			return (ulong)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong R8ToU8(double value)
		{
			const double two64 = 4294967296.0 * 4294967296.0;
			if (!(value > -1.0 && value < two64))
				Internal.ThrowOverflowException();
			return (ulong)value;
		}

		#endregion U8

		#region I4

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int U4ToI4(uint value)
		{
			var sValue = (int)value;
			if (sValue < 0 || sValue > int.MaxValue)
				Internal.ThrowOverflowException();
			return (int)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int I8ToI4(long value)
		{
			if (value < int.MinValue || value > int.MaxValue)
				Internal.ThrowOverflowException();
			return (int)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int U8ToI4(ulong value)
		{
			if (value > int.MaxValue)
				Internal.ThrowOverflowException();
			return (int)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int R4ToI4(float value)
		{
			var r8Value = (double)value;
			if (!(r8Value > ((double)int.MinValue - 1) && r8Value < ((double)int.MaxValue + 1)))
				Internal.ThrowOverflowException();
			return (int)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int R8ToI4(double value)
		{
			if (!(value > ((double)int.MinValue - 1) && value < ((double)int.MaxValue + 1)))
				Internal.ThrowOverflowException();
			return (int)value;
		}

		#endregion I4

		#region U4

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint I4ToU4(int value)
		{
			if (value < 0)
				Internal.ThrowOverflowException();
			return (uint)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint I8ToU4(long value)
		{
			if (value < 0 || value > uint.MaxValue)
				Internal.ThrowOverflowException();
			return (uint)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint U8ToU4(ulong value)
		{
			if (value > uint.MaxValue)
				Internal.ThrowOverflowException();
			return (uint)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint R4ToU4(float value)
		{
			var r8Value = (double)value;
			if (!(r8Value > -1.0 && r8Value < ((double)uint.MaxValue + 1)))
				Internal.ThrowOverflowException();
			return (uint)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint R8ToU4(double value)
		{
			if (!(value > -1.0 && value < ((double)uint.MaxValue + 1)))
				Internal.ThrowOverflowException();
			return (uint)value;
		}

		#endregion U4

		#region I2

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short I4ToI2(int value)
		{
			if (value < short.MinValue || value > short.MaxValue)
				Internal.ThrowOverflowException();
			return (short)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short U4ToI2(uint value)
		{
			var sValue = (int)value;
			if (sValue < 0 || sValue > short.MaxValue)
				Internal.ThrowOverflowException();
			return (short)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short I8ToI2(long value)
		{
			if (value < short.MinValue || value > short.MaxValue)
				Internal.ThrowOverflowException();
			return (short)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short U8ToI2(ulong value)
		{
			var sValue = (long)value;
			if (sValue < 0 || sValue > short.MaxValue)
				Internal.ThrowOverflowException();
			return (short)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short R4ToI2(float value)
		{
			if (!(value > (short.MinValue - 1) && value < (short.MaxValue + 1)))
				Internal.ThrowOverflowException();
			return (short)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static short R8ToI2(double value)
		{
			if (!(value > (short.MinValue - 1) && value < (short.MaxValue + 1)))
				Internal.ThrowOverflowException();
			return (short)value;
		}

		#endregion I2

		#region U2

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort I4ToU2(int value)
		{
			if (value < 0 || value > ushort.MaxValue)
				Internal.ThrowOverflowException();
			return (ushort)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort U4ToU2(uint value)
		{
			if (value > ushort.MaxValue)
				Internal.ThrowOverflowException();
			return (ushort)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort I8ToU2(long value)
		{
			if (value < 0 || value > ushort.MaxValue)
				Internal.ThrowOverflowException();
			return (ushort)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort U8ToU2(ulong value)
		{
			if (value > ushort.MaxValue)
				Internal.ThrowOverflowException();
			return (ushort)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort R4ToU2(float value)
		{
			if (!(value > -1.0f && value < (ushort.MaxValue + 1)))
				Internal.ThrowOverflowException();
			return (ushort)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort R8ToU2(double value)
		{
			if (!(value > -1.0 && value < (ushort.MaxValue + 1)))
				Internal.ThrowOverflowException();
			return (ushort)value;
		}

		#endregion U2

		#region I1

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static sbyte I4ToI1(int value)
		{
			if (value < sbyte.MinValue || value > sbyte.MaxValue)
				Internal.ThrowOverflowException();
			return (sbyte)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static sbyte U4ToI1(uint value)
		{
			var sValue = (int)value;
			if (sValue < 0 || sValue > sbyte.MaxValue)
				Internal.ThrowOverflowException();
			return (sbyte)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static sbyte I8ToI1(long value)
		{
			if (value < sbyte.MinValue || value > sbyte.MaxValue)
				Internal.ThrowOverflowException();
			return (sbyte)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static sbyte U8ToI1(ulong value)
		{
			var sValue = (long)value;
			if (sValue < 0 || sValue > sbyte.MaxValue)
				Internal.ThrowOverflowException();
			return (sbyte)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static sbyte R4ToI1(float value)
		{
			if (!(value > (sbyte.MinValue - 1) && value < (sbyte.MaxValue + 1)))
				Internal.ThrowOverflowException();
			return (sbyte)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static sbyte R8ToI1(double value)
		{
			if (!(value > (sbyte.MinValue - 1) && value < (sbyte.MaxValue + 1)))
				Internal.ThrowOverflowException();
			return (sbyte)value;
		}

		#endregion I1

		#region U1

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte I4ToU1(int value)
		{
			if (value < 0 || value > byte.MaxValue)
				Internal.ThrowOverflowException();
			return (byte)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte U4ToU1(uint value)
		{
			var sValue = (int)value;
			if (sValue < 0 || sValue > byte.MaxValue)
				Internal.ThrowOverflowException();
			return (byte)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte I8ToU1(long value)
		{
			if (value < 0 || value > byte.MaxValue)
				Internal.ThrowOverflowException();
			return (byte)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte U8ToU1(ulong value)
		{
			var sValue = (long)value;
			if (sValue < 0 || sValue > byte.MaxValue)
				Internal.ThrowOverflowException();
			return (byte)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte R4ToU1(float value)
		{
			if (!(value > -1.0f && value < (byte.MaxValue + 1)))
				Internal.ThrowOverflowException();
			return (byte)value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte R8ToU1(double value)
		{
			if (!(value > -1.0 && value < (byte.MaxValue + 1)))
				Internal.ThrowOverflowException();
			return (byte)value;
		}

		#endregion U1
	}
}
