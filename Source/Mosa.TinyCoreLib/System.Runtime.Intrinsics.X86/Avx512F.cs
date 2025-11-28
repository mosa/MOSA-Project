using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class Avx512F : Avx2
{
	public abstract class VL
	{
		public static bool IsSupported
		{
			get
			{
				throw null;
			}
		}

		internal VL()
		{
		}

		public static Vector128<ulong> Abs(Vector128<long> value)
		{
			throw null;
		}

		public static Vector256<ulong> Abs(Vector256<long> value)
		{
			throw null;
		}

		public static Vector128<int> AlignRight32(Vector128<int> left, Vector128<int> right, [ConstantExpected] byte mask)
		{
			throw null;
		}

		public static Vector128<uint> AlignRight32(Vector128<uint> left, Vector128<uint> right, [ConstantExpected] byte mask)
		{
			throw null;
		}

		public static Vector256<int> AlignRight32(Vector256<int> left, Vector256<int> right, [ConstantExpected] byte mask)
		{
			throw null;
		}

		public static Vector256<uint> AlignRight32(Vector256<uint> left, Vector256<uint> right, [ConstantExpected] byte mask)
		{
			throw null;
		}

		public static Vector128<long> AlignRight64(Vector128<long> left, Vector128<long> right, [ConstantExpected] byte mask)
		{
			throw null;
		}

		public static Vector128<ulong> AlignRight64(Vector128<ulong> left, Vector128<ulong> right, [ConstantExpected] byte mask)
		{
			throw null;
		}

		public static Vector256<long> AlignRight64(Vector256<long> left, Vector256<long> right, [ConstantExpected] byte mask)
		{
			throw null;
		}

		public static Vector256<ulong> AlignRight64(Vector256<ulong> left, Vector256<ulong> right, [ConstantExpected] byte mask)
		{
			throw null;
		}

		public static Vector128<uint> CompareGreaterThan(Vector128<uint> left, Vector128<uint> right)
		{
			throw null;
		}

		public static Vector128<ulong> CompareGreaterThan(Vector128<ulong> left, Vector128<ulong> right)
		{
			throw null;
		}

		public static Vector256<uint> CompareGreaterThan(Vector256<uint> left, Vector256<uint> right)
		{
			throw null;
		}

		public static Vector256<ulong> CompareGreaterThan(Vector256<ulong> left, Vector256<ulong> right)
		{
			throw null;
		}

		public static Vector128<int> CompareGreaterThanOrEqual(Vector128<int> left, Vector128<int> right)
		{
			throw null;
		}

		public static Vector128<long> CompareGreaterThanOrEqual(Vector128<long> left, Vector128<long> right)
		{
			throw null;
		}

		public static Vector128<uint> CompareGreaterThanOrEqual(Vector128<uint> left, Vector128<uint> right)
		{
			throw null;
		}

		public static Vector128<ulong> CompareGreaterThanOrEqual(Vector128<ulong> left, Vector128<ulong> right)
		{
			throw null;
		}

		public static Vector256<int> CompareGreaterThanOrEqual(Vector256<int> left, Vector256<int> right)
		{
			throw null;
		}

		public static Vector256<long> CompareGreaterThanOrEqual(Vector256<long> left, Vector256<long> right)
		{
			throw null;
		}

		public static Vector256<uint> CompareGreaterThanOrEqual(Vector256<uint> left, Vector256<uint> right)
		{
			throw null;
		}

		public static Vector256<ulong> CompareGreaterThanOrEqual(Vector256<ulong> left, Vector256<ulong> right)
		{
			throw null;
		}

		public static Vector128<int> CompareLessThan(Vector128<int> left, Vector128<int> right)
		{
			throw null;
		}

		public static Vector128<long> CompareLessThan(Vector128<long> left, Vector128<long> right)
		{
			throw null;
		}

		public static Vector128<uint> CompareLessThan(Vector128<uint> left, Vector128<uint> right)
		{
			throw null;
		}

		public static Vector128<ulong> CompareLessThan(Vector128<ulong> left, Vector128<ulong> right)
		{
			throw null;
		}

		public static Vector256<int> CompareLessThan(Vector256<int> left, Vector256<int> right)
		{
			throw null;
		}

		public static Vector256<long> CompareLessThan(Vector256<long> left, Vector256<long> right)
		{
			throw null;
		}

		public static Vector256<uint> CompareLessThan(Vector256<uint> left, Vector256<uint> right)
		{
			throw null;
		}

		public static Vector256<ulong> CompareLessThan(Vector256<ulong> left, Vector256<ulong> right)
		{
			throw null;
		}

		public static Vector128<int> CompareLessThanOrEqual(Vector128<int> left, Vector128<int> right)
		{
			throw null;
		}

		public static Vector128<long> CompareLessThanOrEqual(Vector128<long> left, Vector128<long> right)
		{
			throw null;
		}

		public static Vector128<uint> CompareLessThanOrEqual(Vector128<uint> left, Vector128<uint> right)
		{
			throw null;
		}

		public static Vector128<ulong> CompareLessThanOrEqual(Vector128<ulong> left, Vector128<ulong> right)
		{
			throw null;
		}

		public static Vector256<int> CompareLessThanOrEqual(Vector256<int> left, Vector256<int> right)
		{
			throw null;
		}

		public static Vector256<long> CompareLessThanOrEqual(Vector256<long> left, Vector256<long> right)
		{
			throw null;
		}

		public static Vector256<uint> CompareLessThanOrEqual(Vector256<uint> left, Vector256<uint> right)
		{
			throw null;
		}

		public static Vector256<ulong> CompareLessThanOrEqual(Vector256<ulong> left, Vector256<ulong> right)
		{
			throw null;
		}

		public static Vector128<int> CompareNotEqual(Vector128<int> left, Vector128<int> right)
		{
			throw null;
		}

		public static Vector128<long> CompareNotEqual(Vector128<long> left, Vector128<long> right)
		{
			throw null;
		}

		public static Vector128<uint> CompareNotEqual(Vector128<uint> left, Vector128<uint> right)
		{
			throw null;
		}

		public static Vector128<ulong> CompareNotEqual(Vector128<ulong> left, Vector128<ulong> right)
		{
			throw null;
		}

		public static Vector256<int> CompareNotEqual(Vector256<int> left, Vector256<int> right)
		{
			throw null;
		}

		public static Vector256<long> CompareNotEqual(Vector256<long> left, Vector256<long> right)
		{
			throw null;
		}

		public static Vector256<uint> CompareNotEqual(Vector256<uint> left, Vector256<uint> right)
		{
			throw null;
		}

		public static Vector256<ulong> CompareNotEqual(Vector256<ulong> left, Vector256<ulong> right)
		{
			throw null;
		}

		public static Vector128<byte> ConvertToVector128Byte(Vector128<int> value)
		{
			throw null;
		}

		public static Vector128<byte> ConvertToVector128Byte(Vector128<long> value)
		{
			throw null;
		}

		public static Vector128<byte> ConvertToVector128Byte(Vector128<uint> value)
		{
			throw null;
		}

		public static Vector128<byte> ConvertToVector128Byte(Vector128<ulong> value)
		{
			throw null;
		}

		public static Vector128<byte> ConvertToVector128Byte(Vector256<int> value)
		{
			throw null;
		}

		public static Vector128<byte> ConvertToVector128Byte(Vector256<long> value)
		{
			throw null;
		}

		public static Vector128<byte> ConvertToVector128Byte(Vector256<uint> value)
		{
			throw null;
		}

		public static Vector128<byte> ConvertToVector128Byte(Vector256<ulong> value)
		{
			throw null;
		}

		public static Vector128<byte> ConvertToVector128ByteWithSaturation(Vector128<uint> value)
		{
			throw null;
		}

		public static Vector128<byte> ConvertToVector128ByteWithSaturation(Vector128<ulong> value)
		{
			throw null;
		}

		public static Vector128<byte> ConvertToVector128ByteWithSaturation(Vector256<uint> value)
		{
			throw null;
		}

		public static Vector128<byte> ConvertToVector128ByteWithSaturation(Vector256<ulong> value)
		{
			throw null;
		}

		public static Vector128<double> ConvertToVector128Double(Vector128<uint> value)
		{
			throw null;
		}

		public static Vector128<short> ConvertToVector128Int16(Vector128<int> value)
		{
			throw null;
		}

		public static Vector128<short> ConvertToVector128Int16(Vector128<long> value)
		{
			throw null;
		}

		public static Vector128<short> ConvertToVector128Int16(Vector128<uint> value)
		{
			throw null;
		}

		public static Vector128<short> ConvertToVector128Int16(Vector128<ulong> value)
		{
			throw null;
		}

		public static Vector128<short> ConvertToVector128Int16(Vector256<int> value)
		{
			throw null;
		}

		public static Vector128<short> ConvertToVector128Int16(Vector256<long> value)
		{
			throw null;
		}

		public static Vector128<short> ConvertToVector128Int16(Vector256<uint> value)
		{
			throw null;
		}

		public static Vector128<short> ConvertToVector128Int16(Vector256<ulong> value)
		{
			throw null;
		}

		public static Vector128<short> ConvertToVector128Int16WithSaturation(Vector128<int> value)
		{
			throw null;
		}

		public static Vector128<short> ConvertToVector128Int16WithSaturation(Vector128<long> value)
		{
			throw null;
		}

		public static Vector128<short> ConvertToVector128Int16WithSaturation(Vector256<int> value)
		{
			throw null;
		}

		public static Vector128<short> ConvertToVector128Int16WithSaturation(Vector256<long> value)
		{
			throw null;
		}

		public static Vector128<int> ConvertToVector128Int32(Vector128<long> value)
		{
			throw null;
		}

		public static Vector128<int> ConvertToVector128Int32(Vector128<ulong> value)
		{
			throw null;
		}

		public static Vector128<int> ConvertToVector128Int32(Vector256<long> value)
		{
			throw null;
		}

		public static Vector128<int> ConvertToVector128Int32(Vector256<ulong> value)
		{
			throw null;
		}

		public static Vector128<int> ConvertToVector128Int32WithSaturation(Vector128<long> value)
		{
			throw null;
		}

		public static Vector128<int> ConvertToVector128Int32WithSaturation(Vector256<long> value)
		{
			throw null;
		}

		public static Vector128<sbyte> ConvertToVector128SByte(Vector128<int> value)
		{
			throw null;
		}

		public static Vector128<sbyte> ConvertToVector128SByte(Vector128<long> value)
		{
			throw null;
		}

		public static Vector128<sbyte> ConvertToVector128SByte(Vector128<uint> value)
		{
			throw null;
		}

		public static Vector128<sbyte> ConvertToVector128SByte(Vector128<ulong> value)
		{
			throw null;
		}

		public static Vector128<sbyte> ConvertToVector128SByte(Vector256<int> value)
		{
			throw null;
		}

		public static Vector128<sbyte> ConvertToVector128SByte(Vector256<long> value)
		{
			throw null;
		}

		public static Vector128<sbyte> ConvertToVector128SByte(Vector256<uint> value)
		{
			throw null;
		}

		public static Vector128<sbyte> ConvertToVector128SByte(Vector256<ulong> value)
		{
			throw null;
		}

		public static Vector128<sbyte> ConvertToVector128SByteWithSaturation(Vector128<int> value)
		{
			throw null;
		}

		public static Vector128<sbyte> ConvertToVector128SByteWithSaturation(Vector128<long> value)
		{
			throw null;
		}

		public static Vector128<sbyte> ConvertToVector128SByteWithSaturation(Vector256<int> value)
		{
			throw null;
		}

		public static Vector128<sbyte> ConvertToVector128SByteWithSaturation(Vector256<long> value)
		{
			throw null;
		}

		public static Vector128<float> ConvertToVector128Single(Vector128<uint> value)
		{
			throw null;
		}

		public static Vector128<ushort> ConvertToVector128UInt16(Vector128<int> value)
		{
			throw null;
		}

		public static Vector128<ushort> ConvertToVector128UInt16(Vector128<long> value)
		{
			throw null;
		}

		public static Vector128<ushort> ConvertToVector128UInt16(Vector128<uint> value)
		{
			throw null;
		}

		public static Vector128<ushort> ConvertToVector128UInt16(Vector128<ulong> value)
		{
			throw null;
		}

		public static Vector128<ushort> ConvertToVector128UInt16(Vector256<int> value)
		{
			throw null;
		}

		public static Vector128<ushort> ConvertToVector128UInt16(Vector256<long> value)
		{
			throw null;
		}

		public static Vector128<ushort> ConvertToVector128UInt16(Vector256<uint> value)
		{
			throw null;
		}

		public static Vector128<ushort> ConvertToVector128UInt16(Vector256<ulong> value)
		{
			throw null;
		}

		public static Vector128<ushort> ConvertToVector128UInt16WithSaturation(Vector128<uint> value)
		{
			throw null;
		}

		public static Vector128<ushort> ConvertToVector128UInt16WithSaturation(Vector128<ulong> value)
		{
			throw null;
		}

		public static Vector128<ushort> ConvertToVector128UInt16WithSaturation(Vector256<uint> value)
		{
			throw null;
		}

		public static Vector128<ushort> ConvertToVector128UInt16WithSaturation(Vector256<ulong> value)
		{
			throw null;
		}

		public static Vector128<uint> ConvertToVector128UInt32(Vector128<double> value)
		{
			throw null;
		}

		public static Vector128<uint> ConvertToVector128UInt32(Vector128<long> value)
		{
			throw null;
		}

		public static Vector128<uint> ConvertToVector128UInt32(Vector128<float> value)
		{
			throw null;
		}

		public static Vector128<uint> ConvertToVector128UInt32(Vector128<ulong> value)
		{
			throw null;
		}

		public static Vector128<uint> ConvertToVector128UInt32(Vector256<double> value)
		{
			throw null;
		}

		public static Vector128<uint> ConvertToVector128UInt32(Vector256<long> value)
		{
			throw null;
		}

		public static Vector128<uint> ConvertToVector128UInt32(Vector256<ulong> value)
		{
			throw null;
		}

		public static Vector128<uint> ConvertToVector128UInt32WithSaturation(Vector128<ulong> value)
		{
			throw null;
		}

		public static Vector128<uint> ConvertToVector128UInt32WithSaturation(Vector256<ulong> value)
		{
			throw null;
		}

		public static Vector128<uint> ConvertToVector128UInt32WithTruncation(Vector128<double> value)
		{
			throw null;
		}

		public static Vector128<uint> ConvertToVector128UInt32WithTruncation(Vector128<float> value)
		{
			throw null;
		}

		public static Vector128<uint> ConvertToVector128UInt32WithTruncation(Vector256<double> value)
		{
			throw null;
		}

		public static Vector256<double> ConvertToVector256Double(Vector128<uint> value)
		{
			throw null;
		}

		public static Vector256<float> ConvertToVector256Single(Vector256<uint> value)
		{
			throw null;
		}

		public static Vector256<uint> ConvertToVector256UInt32(Vector256<float> value)
		{
			throw null;
		}

		public static Vector256<uint> ConvertToVector256UInt32WithTruncation(Vector256<float> value)
		{
			throw null;
		}

		public static Vector128<double> Fixup(Vector128<double> left, Vector128<double> right, Vector128<long> table, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector128<float> Fixup(Vector128<float> left, Vector128<float> right, Vector128<int> table, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<double> Fixup(Vector256<double> left, Vector256<double> right, Vector256<long> table, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<float> Fixup(Vector256<float> left, Vector256<float> right, Vector256<int> table, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector128<double> GetExponent(Vector128<double> value)
		{
			throw null;
		}

		public static Vector128<float> GetExponent(Vector128<float> value)
		{
			throw null;
		}

		public static Vector256<double> GetExponent(Vector256<double> value)
		{
			throw null;
		}

		public static Vector256<float> GetExponent(Vector256<float> value)
		{
			throw null;
		}

		public static Vector128<double> GetMantissa(Vector128<double> value, [ConstantExpected(Max = 15)] byte control)
		{
			throw null;
		}

		public static Vector128<float> GetMantissa(Vector128<float> value, [ConstantExpected(Max = 15)] byte control)
		{
			throw null;
		}

		public static Vector256<double> GetMantissa(Vector256<double> value, [ConstantExpected(Max = 15)] byte control)
		{
			throw null;
		}

		public static Vector256<float> GetMantissa(Vector256<float> value, [ConstantExpected(Max = 15)] byte control)
		{
			throw null;
		}

		public static Vector128<long> Max(Vector128<long> left, Vector128<long> right)
		{
			throw null;
		}

		public static Vector128<ulong> Max(Vector128<ulong> left, Vector128<ulong> right)
		{
			throw null;
		}

		public static Vector256<long> Max(Vector256<long> left, Vector256<long> right)
		{
			throw null;
		}

		public static Vector256<ulong> Max(Vector256<ulong> left, Vector256<ulong> right)
		{
			throw null;
		}

		public static Vector128<long> Min(Vector128<long> left, Vector128<long> right)
		{
			throw null;
		}

		public static Vector128<ulong> Min(Vector128<ulong> left, Vector128<ulong> right)
		{
			throw null;
		}

		public static Vector256<long> Min(Vector256<long> left, Vector256<long> right)
		{
			throw null;
		}

		public static Vector256<ulong> Min(Vector256<ulong> left, Vector256<ulong> right)
		{
			throw null;
		}

		public static Vector128<double> PermuteVar2x64x2(Vector128<double> lower, Vector128<long> indices, Vector128<double> upper)
		{
			throw null;
		}

		public static Vector128<long> PermuteVar2x64x2(Vector128<long> lower, Vector128<long> indices, Vector128<long> upper)
		{
			throw null;
		}

		public static Vector128<ulong> PermuteVar2x64x2(Vector128<ulong> lower, Vector128<ulong> indices, Vector128<ulong> upper)
		{
			throw null;
		}

		public static Vector128<int> PermuteVar4x32x2(Vector128<int> lower, Vector128<int> indices, Vector128<int> upper)
		{
			throw null;
		}

		public static Vector128<float> PermuteVar4x32x2(Vector128<float> lower, Vector128<int> indices, Vector128<float> upper)
		{
			throw null;
		}

		public static Vector128<uint> PermuteVar4x32x2(Vector128<uint> lower, Vector128<uint> indices, Vector128<uint> upper)
		{
			throw null;
		}

		public static Vector256<long> PermuteVar4x64(Vector256<long> value, Vector256<long> control)
		{
			throw null;
		}

		public static Vector256<ulong> PermuteVar4x64(Vector256<ulong> value, Vector256<ulong> control)
		{
			throw null;
		}

		public static Vector256<double> PermuteVar4x64(Vector256<double> value, Vector256<long> control)
		{
			throw null;
		}

		public static Vector256<double> PermuteVar4x64x2(Vector256<double> lower, Vector256<long> indices, Vector256<double> upper)
		{
			throw null;
		}

		public static Vector256<long> PermuteVar4x64x2(Vector256<long> lower, Vector256<long> indices, Vector256<long> upper)
		{
			throw null;
		}

		public static Vector256<ulong> PermuteVar4x64x2(Vector256<ulong> lower, Vector256<ulong> indices, Vector256<ulong> upper)
		{
			throw null;
		}

		public static Vector256<int> PermuteVar8x32x2(Vector256<int> lower, Vector256<int> indices, Vector256<int> upper)
		{
			throw null;
		}

		public static Vector256<float> PermuteVar8x32x2(Vector256<float> lower, Vector256<int> indices, Vector256<float> upper)
		{
			throw null;
		}

		public static Vector256<uint> PermuteVar8x32x2(Vector256<uint> lower, Vector256<uint> indices, Vector256<uint> upper)
		{
			throw null;
		}

		public static Vector128<double> Reciprocal14(Vector128<double> value)
		{
			throw null;
		}

		public static Vector128<float> Reciprocal14(Vector128<float> value)
		{
			throw null;
		}

		public static Vector256<double> Reciprocal14(Vector256<double> value)
		{
			throw null;
		}

		public static Vector256<float> Reciprocal14(Vector256<float> value)
		{
			throw null;
		}

		public static Vector128<double> ReciprocalSqrt14(Vector128<double> value)
		{
			throw null;
		}

		public static Vector128<float> ReciprocalSqrt14(Vector128<float> value)
		{
			throw null;
		}

		public static Vector256<double> ReciprocalSqrt14(Vector256<double> value)
		{
			throw null;
		}

		public static Vector256<float> ReciprocalSqrt14(Vector256<float> value)
		{
			throw null;
		}

		public static Vector128<int> RotateLeft(Vector128<int> value, [ConstantExpected] byte count)
		{
			throw null;
		}

		public static Vector128<long> RotateLeft(Vector128<long> value, [ConstantExpected] byte count)
		{
			throw null;
		}

		public static Vector128<uint> RotateLeft(Vector128<uint> value, [ConstantExpected] byte count)
		{
			throw null;
		}

		public static Vector128<ulong> RotateLeft(Vector128<ulong> value, [ConstantExpected] byte count)
		{
			throw null;
		}

		public static Vector256<int> RotateLeft(Vector256<int> value, [ConstantExpected] byte count)
		{
			throw null;
		}

		public static Vector256<long> RotateLeft(Vector256<long> value, [ConstantExpected] byte count)
		{
			throw null;
		}

		public static Vector256<uint> RotateLeft(Vector256<uint> value, [ConstantExpected] byte count)
		{
			throw null;
		}

		public static Vector256<ulong> RotateLeft(Vector256<ulong> value, [ConstantExpected] byte count)
		{
			throw null;
		}

		public static Vector128<int> RotateLeftVariable(Vector128<int> value, Vector128<uint> count)
		{
			throw null;
		}

		public static Vector128<long> RotateLeftVariable(Vector128<long> value, Vector128<ulong> count)
		{
			throw null;
		}

		public static Vector128<uint> RotateLeftVariable(Vector128<uint> value, Vector128<uint> count)
		{
			throw null;
		}

		public static Vector128<ulong> RotateLeftVariable(Vector128<ulong> value, Vector128<ulong> count)
		{
			throw null;
		}

		public static Vector256<int> RotateLeftVariable(Vector256<int> value, Vector256<uint> count)
		{
			throw null;
		}

		public static Vector256<long> RotateLeftVariable(Vector256<long> value, Vector256<ulong> count)
		{
			throw null;
		}

		public static Vector256<uint> RotateLeftVariable(Vector256<uint> value, Vector256<uint> count)
		{
			throw null;
		}

		public static Vector256<ulong> RotateLeftVariable(Vector256<ulong> value, Vector256<ulong> count)
		{
			throw null;
		}

		public static Vector128<int> RotateRight(Vector128<int> value, [ConstantExpected] byte count)
		{
			throw null;
		}

		public static Vector128<long> RotateRight(Vector128<long> value, [ConstantExpected] byte count)
		{
			throw null;
		}

		public static Vector128<uint> RotateRight(Vector128<uint> value, [ConstantExpected] byte count)
		{
			throw null;
		}

		public static Vector128<ulong> RotateRight(Vector128<ulong> value, [ConstantExpected] byte count)
		{
			throw null;
		}

		public static Vector256<int> RotateRight(Vector256<int> value, [ConstantExpected] byte count)
		{
			throw null;
		}

		public static Vector256<long> RotateRight(Vector256<long> value, [ConstantExpected] byte count)
		{
			throw null;
		}

		public static Vector256<uint> RotateRight(Vector256<uint> value, [ConstantExpected] byte count)
		{
			throw null;
		}

		public static Vector256<ulong> RotateRight(Vector256<ulong> value, [ConstantExpected] byte count)
		{
			throw null;
		}

		public static Vector128<int> RotateRightVariable(Vector128<int> value, Vector128<uint> count)
		{
			throw null;
		}

		public static Vector128<long> RotateRightVariable(Vector128<long> value, Vector128<ulong> count)
		{
			throw null;
		}

		public static Vector128<uint> RotateRightVariable(Vector128<uint> value, Vector128<uint> count)
		{
			throw null;
		}

		public static Vector128<ulong> RotateRightVariable(Vector128<ulong> value, Vector128<ulong> count)
		{
			throw null;
		}

		public static Vector256<int> RotateRightVariable(Vector256<int> value, Vector256<uint> count)
		{
			throw null;
		}

		public static Vector256<long> RotateRightVariable(Vector256<long> value, Vector256<ulong> count)
		{
			throw null;
		}

		public static Vector256<uint> RotateRightVariable(Vector256<uint> value, Vector256<uint> count)
		{
			throw null;
		}

		public static Vector256<ulong> RotateRightVariable(Vector256<ulong> value, Vector256<ulong> count)
		{
			throw null;
		}

		public static Vector128<double> RoundScale(Vector128<double> value, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector128<float> RoundScale(Vector128<float> value, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<double> RoundScale(Vector256<double> value, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<float> RoundScale(Vector256<float> value, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector128<double> Scale(Vector128<double> left, Vector128<double> right)
		{
			throw null;
		}

		public static Vector128<float> Scale(Vector128<float> left, Vector128<float> right)
		{
			throw null;
		}

		public static Vector256<double> Scale(Vector256<double> left, Vector256<double> right)
		{
			throw null;
		}

		public static Vector256<float> Scale(Vector256<float> left, Vector256<float> right)
		{
			throw null;
		}

		public static Vector128<long> ShiftRightArithmetic(Vector128<long> value, [ConstantExpected] byte count)
		{
			throw null;
		}

		public static Vector128<long> ShiftRightArithmetic(Vector128<long> value, Vector128<long> count)
		{
			throw null;
		}

		public static Vector256<long> ShiftRightArithmetic(Vector256<long> value, [ConstantExpected] byte count)
		{
			throw null;
		}

		public static Vector256<long> ShiftRightArithmetic(Vector256<long> value, Vector128<long> count)
		{
			throw null;
		}

		public static Vector128<long> ShiftRightArithmeticVariable(Vector128<long> value, Vector128<ulong> count)
		{
			throw null;
		}

		public static Vector256<long> ShiftRightArithmeticVariable(Vector256<long> value, Vector256<ulong> count)
		{
			throw null;
		}

		public static Vector256<double> Shuffle2x128(Vector256<double> left, Vector256<double> right, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<int> Shuffle2x128(Vector256<int> left, Vector256<int> right, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<long> Shuffle2x128(Vector256<long> left, Vector256<long> right, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<float> Shuffle2x128(Vector256<float> left, Vector256<float> right, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<uint> Shuffle2x128(Vector256<uint> left, Vector256<uint> right, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<ulong> Shuffle2x128(Vector256<ulong> left, Vector256<ulong> right, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector128<byte> TernaryLogic(Vector128<byte> a, Vector128<byte> b, Vector128<byte> c, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector128<double> TernaryLogic(Vector128<double> a, Vector128<double> b, Vector128<double> c, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector128<int> TernaryLogic(Vector128<int> a, Vector128<int> b, Vector128<int> c, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector128<short> TernaryLogic(Vector128<short> a, Vector128<short> b, Vector128<short> c, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector128<long> TernaryLogic(Vector128<long> a, Vector128<long> b, Vector128<long> c, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector128<sbyte> TernaryLogic(Vector128<sbyte> a, Vector128<sbyte> b, Vector128<sbyte> c, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector128<float> TernaryLogic(Vector128<float> a, Vector128<float> b, Vector128<float> c, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector128<ushort> TernaryLogic(Vector128<ushort> a, Vector128<ushort> b, Vector128<ushort> c, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector128<uint> TernaryLogic(Vector128<uint> a, Vector128<uint> b, Vector128<uint> c, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector128<ulong> TernaryLogic(Vector128<ulong> a, Vector128<ulong> b, Vector128<ulong> c, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<byte> TernaryLogic(Vector256<byte> a, Vector256<byte> b, Vector256<byte> c, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<double> TernaryLogic(Vector256<double> a, Vector256<double> b, Vector256<double> c, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<int> TernaryLogic(Vector256<int> a, Vector256<int> b, Vector256<int> c, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<short> TernaryLogic(Vector256<short> a, Vector256<short> b, Vector256<short> c, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<long> TernaryLogic(Vector256<long> a, Vector256<long> b, Vector256<long> c, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<sbyte> TernaryLogic(Vector256<sbyte> a, Vector256<sbyte> b, Vector256<sbyte> c, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<float> TernaryLogic(Vector256<float> a, Vector256<float> b, Vector256<float> c, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<ushort> TernaryLogic(Vector256<ushort> a, Vector256<ushort> b, Vector256<ushort> c, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<uint> TernaryLogic(Vector256<uint> a, Vector256<uint> b, Vector256<uint> c, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<ulong> TernaryLogic(Vector256<ulong> a, Vector256<ulong> b, Vector256<ulong> c, [ConstantExpected] byte control)
		{
			throw null;
		}
	}

	public new abstract class X64 : Avx2.X64
	{
		public new static bool IsSupported
		{
			get
			{
				throw null;
			}
		}

		internal X64()
		{
		}

		public static Vector128<double> ConvertScalarToVector128Double(Vector128<double> upper, ulong value)
		{
			throw null;
		}

		public static Vector128<float> ConvertScalarToVector128Single(Vector128<float> upper, ulong value)
		{
			throw null;
		}

		public static ulong ConvertToUInt64(Vector128<double> value)
		{
			throw null;
		}

		public static ulong ConvertToUInt64(Vector128<float> value)
		{
			throw null;
		}

		public static ulong ConvertToUInt64WithTruncation(Vector128<double> value)
		{
			throw null;
		}

		public static ulong ConvertToUInt64WithTruncation(Vector128<float> value)
		{
			throw null;
		}
	}

	public new static bool IsSupported
	{
		get
		{
			throw null;
		}
	}

	internal Avx512F()
	{
	}

	public static Vector512<uint> Abs(Vector512<int> value)
	{
		throw null;
	}

	public static Vector512<ulong> Abs(Vector512<long> value)
	{
		throw null;
	}

	public static Vector512<double> Add(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<int> Add(Vector512<int> left, Vector512<int> right)
	{
		throw null;
	}

	public static Vector512<long> Add(Vector512<long> left, Vector512<long> right)
	{
		throw null;
	}

	public static Vector512<float> Add(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<uint> Add(Vector512<uint> left, Vector512<uint> right)
	{
		throw null;
	}

	public static Vector512<ulong> Add(Vector512<ulong> left, Vector512<ulong> right)
	{
		throw null;
	}

	public static Vector512<int> AlignRight32(Vector512<int> left, Vector512<int> right, [ConstantExpected] byte mask)
	{
		throw null;
	}

	public static Vector512<uint> AlignRight32(Vector512<uint> left, Vector512<uint> right, [ConstantExpected] byte mask)
	{
		throw null;
	}

	public static Vector512<long> AlignRight64(Vector512<long> left, Vector512<long> right, [ConstantExpected] byte mask)
	{
		throw null;
	}

	public static Vector512<ulong> AlignRight64(Vector512<ulong> left, Vector512<ulong> right, [ConstantExpected] byte mask)
	{
		throw null;
	}

	public static Vector512<byte> And(Vector512<byte> left, Vector512<byte> right)
	{
		throw null;
	}

	public static Vector512<short> And(Vector512<short> left, Vector512<short> right)
	{
		throw null;
	}

	public static Vector512<int> And(Vector512<int> left, Vector512<int> right)
	{
		throw null;
	}

	public static Vector512<long> And(Vector512<long> left, Vector512<long> right)
	{
		throw null;
	}

	public static Vector512<sbyte> And(Vector512<sbyte> left, Vector512<sbyte> right)
	{
		throw null;
	}

	public static Vector512<ushort> And(Vector512<ushort> left, Vector512<ushort> right)
	{
		throw null;
	}

	public static Vector512<uint> And(Vector512<uint> left, Vector512<uint> right)
	{
		throw null;
	}

	public static Vector512<ulong> And(Vector512<ulong> left, Vector512<ulong> right)
	{
		throw null;
	}

	public static Vector512<byte> AndNot(Vector512<byte> left, Vector512<byte> right)
	{
		throw null;
	}

	public static Vector512<short> AndNot(Vector512<short> left, Vector512<short> right)
	{
		throw null;
	}

	public static Vector512<int> AndNot(Vector512<int> left, Vector512<int> right)
	{
		throw null;
	}

	public static Vector512<long> AndNot(Vector512<long> left, Vector512<long> right)
	{
		throw null;
	}

	public static Vector512<sbyte> AndNot(Vector512<sbyte> left, Vector512<sbyte> right)
	{
		throw null;
	}

	public static Vector512<ushort> AndNot(Vector512<ushort> left, Vector512<ushort> right)
	{
		throw null;
	}

	public static Vector512<uint> AndNot(Vector512<uint> left, Vector512<uint> right)
	{
		throw null;
	}

	public static Vector512<ulong> AndNot(Vector512<ulong> left, Vector512<ulong> right)
	{
		throw null;
	}

	public static Vector512<double> BlendVariable(Vector512<double> left, Vector512<double> right, Vector512<double> mask)
	{
		throw null;
	}

	public static Vector512<int> BlendVariable(Vector512<int> left, Vector512<int> right, Vector512<int> mask)
	{
		throw null;
	}

	public static Vector512<long> BlendVariable(Vector512<long> left, Vector512<long> right, Vector512<long> mask)
	{
		throw null;
	}

	public static Vector512<float> BlendVariable(Vector512<float> left, Vector512<float> right, Vector512<float> mask)
	{
		throw null;
	}

	public static Vector512<uint> BlendVariable(Vector512<uint> left, Vector512<uint> right, Vector512<uint> mask)
	{
		throw null;
	}

	public static Vector512<ulong> BlendVariable(Vector512<ulong> left, Vector512<ulong> right, Vector512<ulong> mask)
	{
		throw null;
	}

	public static Vector512<double> BroadcastScalarToVector512(Vector128<double> value)
	{
		throw null;
	}

	public static Vector512<int> BroadcastScalarToVector512(Vector128<int> value)
	{
		throw null;
	}

	public static Vector512<long> BroadcastScalarToVector512(Vector128<long> value)
	{
		throw null;
	}

	public static Vector512<float> BroadcastScalarToVector512(Vector128<float> value)
	{
		throw null;
	}

	public static Vector512<uint> BroadcastScalarToVector512(Vector128<uint> value)
	{
		throw null;
	}

	public static Vector512<ulong> BroadcastScalarToVector512(Vector128<ulong> value)
	{
		throw null;
	}

	public unsafe static Vector512<int> BroadcastVector128ToVector512(int* address)
	{
		throw null;
	}

	public unsafe static Vector512<float> BroadcastVector128ToVector512(float* address)
	{
		throw null;
	}

	public unsafe static Vector512<uint> BroadcastVector128ToVector512(uint* address)
	{
		throw null;
	}

	public unsafe static Vector512<double> BroadcastVector256ToVector512(double* address)
	{
		throw null;
	}

	public unsafe static Vector512<long> BroadcastVector256ToVector512(long* address)
	{
		throw null;
	}

	public unsafe static Vector512<ulong> BroadcastVector256ToVector512(ulong* address)
	{
		throw null;
	}

	public static Vector512<double> Compare(Vector512<double> left, Vector512<double> right, [ConstantExpected(Max = FloatComparisonMode.UnorderedTrueSignaling)] FloatComparisonMode mode)
	{
		throw null;
	}

	public static Vector512<float> Compare(Vector512<float> left, Vector512<float> right, [ConstantExpected(Max = FloatComparisonMode.UnorderedTrueSignaling)] FloatComparisonMode mode)
	{
		throw null;
	}

	public static Vector512<double> CompareEqual(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<int> CompareEqual(Vector512<int> left, Vector512<int> right)
	{
		throw null;
	}

	public static Vector512<long> CompareEqual(Vector512<long> left, Vector512<long> right)
	{
		throw null;
	}

	public static Vector512<float> CompareEqual(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<uint> CompareEqual(Vector512<uint> left, Vector512<uint> right)
	{
		throw null;
	}

	public static Vector512<ulong> CompareEqual(Vector512<ulong> left, Vector512<ulong> right)
	{
		throw null;
	}

	public static Vector512<double> CompareGreaterThan(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<int> CompareGreaterThan(Vector512<int> left, Vector512<int> right)
	{
		throw null;
	}

	public static Vector512<long> CompareGreaterThan(Vector512<long> left, Vector512<long> right)
	{
		throw null;
	}

	public static Vector512<float> CompareGreaterThan(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<uint> CompareGreaterThan(Vector512<uint> left, Vector512<uint> right)
	{
		throw null;
	}

	public static Vector512<ulong> CompareGreaterThan(Vector512<ulong> left, Vector512<ulong> right)
	{
		throw null;
	}

	public static Vector512<double> CompareGreaterThanOrEqual(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<int> CompareGreaterThanOrEqual(Vector512<int> left, Vector512<int> right)
	{
		throw null;
	}

	public static Vector512<long> CompareGreaterThanOrEqual(Vector512<long> left, Vector512<long> right)
	{
		throw null;
	}

	public static Vector512<float> CompareGreaterThanOrEqual(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<uint> CompareGreaterThanOrEqual(Vector512<uint> left, Vector512<uint> right)
	{
		throw null;
	}

	public static Vector512<ulong> CompareGreaterThanOrEqual(Vector512<ulong> left, Vector512<ulong> right)
	{
		throw null;
	}

	public static Vector512<double> CompareLessThan(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<int> CompareLessThan(Vector512<int> left, Vector512<int> right)
	{
		throw null;
	}

	public static Vector512<long> CompareLessThan(Vector512<long> left, Vector512<long> right)
	{
		throw null;
	}

	public static Vector512<float> CompareLessThan(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<uint> CompareLessThan(Vector512<uint> left, Vector512<uint> right)
	{
		throw null;
	}

	public static Vector512<ulong> CompareLessThan(Vector512<ulong> left, Vector512<ulong> right)
	{
		throw null;
	}

	public static Vector512<double> CompareLessThanOrEqual(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<int> CompareLessThanOrEqual(Vector512<int> left, Vector512<int> right)
	{
		throw null;
	}

	public static Vector512<long> CompareLessThanOrEqual(Vector512<long> left, Vector512<long> right)
	{
		throw null;
	}

	public static Vector512<float> CompareLessThanOrEqual(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<uint> CompareLessThanOrEqual(Vector512<uint> left, Vector512<uint> right)
	{
		throw null;
	}

	public static Vector512<ulong> CompareLessThanOrEqual(Vector512<ulong> left, Vector512<ulong> right)
	{
		throw null;
	}

	public static Vector512<double> CompareNotEqual(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<int> CompareNotEqual(Vector512<int> left, Vector512<int> right)
	{
		throw null;
	}

	public static Vector512<long> CompareNotEqual(Vector512<long> left, Vector512<long> right)
	{
		throw null;
	}

	public static Vector512<float> CompareNotEqual(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<uint> CompareNotEqual(Vector512<uint> left, Vector512<uint> right)
	{
		throw null;
	}

	public static Vector512<ulong> CompareNotEqual(Vector512<ulong> left, Vector512<ulong> right)
	{
		throw null;
	}

	public static Vector512<double> CompareNotGreaterThan(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<float> CompareNotGreaterThan(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<double> CompareNotGreaterThanOrEqual(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<float> CompareNotGreaterThanOrEqual(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<double> CompareNotLessThan(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<float> CompareNotLessThan(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<double> CompareNotLessThanOrEqual(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<float> CompareNotLessThanOrEqual(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<double> CompareOrdered(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<float> CompareOrdered(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<double> CompareUnordered(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<float> CompareUnordered(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector128<double> ConvertScalarToVector128Double(Vector128<double> upper, uint value)
	{
		throw null;
	}

	public static Vector128<float> ConvertScalarToVector128Single(Vector128<float> upper, uint value)
	{
		throw null;
	}

	public static uint ConvertToUInt32(Vector128<double> value)
	{
		throw null;
	}

	public static uint ConvertToUInt32(Vector128<float> value)
	{
		throw null;
	}

	public static uint ConvertToUInt32WithTruncation(Vector128<double> value)
	{
		throw null;
	}

	public static uint ConvertToUInt32WithTruncation(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<byte> ConvertToVector128Byte(Vector512<int> value)
	{
		throw null;
	}

	public static Vector128<byte> ConvertToVector128Byte(Vector512<long> value)
	{
		throw null;
	}

	public static Vector128<byte> ConvertToVector128Byte(Vector512<uint> value)
	{
		throw null;
	}

	public static Vector128<byte> ConvertToVector128Byte(Vector512<ulong> value)
	{
		throw null;
	}

	public static Vector128<byte> ConvertToVector128ByteWithSaturation(Vector512<uint> value)
	{
		throw null;
	}

	public static Vector128<byte> ConvertToVector128ByteWithSaturation(Vector512<ulong> value)
	{
		throw null;
	}

	public static Vector128<short> ConvertToVector128Int16(Vector512<long> value)
	{
		throw null;
	}

	public static Vector128<short> ConvertToVector128Int16(Vector512<ulong> value)
	{
		throw null;
	}

	public static Vector128<short> ConvertToVector128Int16WithSaturation(Vector512<long> value)
	{
		throw null;
	}

	public static Vector128<sbyte> ConvertToVector128SByte(Vector512<int> value)
	{
		throw null;
	}

	public static Vector128<sbyte> ConvertToVector128SByte(Vector512<long> value)
	{
		throw null;
	}

	public static Vector128<sbyte> ConvertToVector128SByte(Vector512<uint> value)
	{
		throw null;
	}

	public static Vector128<sbyte> ConvertToVector128SByte(Vector512<ulong> value)
	{
		throw null;
	}

	public static Vector128<sbyte> ConvertToVector128SByteWithSaturation(Vector512<int> value)
	{
		throw null;
	}

	public static Vector128<sbyte> ConvertToVector128SByteWithSaturation(Vector512<long> value)
	{
		throw null;
	}

	public static Vector128<ushort> ConvertToVector128UInt16(Vector512<long> value)
	{
		throw null;
	}

	public static Vector128<ushort> ConvertToVector128UInt16(Vector512<ulong> value)
	{
		throw null;
	}

	public static Vector128<ushort> ConvertToVector128UInt16WithSaturation(Vector512<ulong> value)
	{
		throw null;
	}

	public static Vector256<short> ConvertToVector256Int16(Vector512<int> value)
	{
		throw null;
	}

	public static Vector256<short> ConvertToVector256Int16(Vector512<uint> value)
	{
		throw null;
	}

	public static Vector256<short> ConvertToVector256Int16WithSaturation(Vector512<int> value)
	{
		throw null;
	}

	public static Vector256<int> ConvertToVector256Int32(Vector512<double> value)
	{
		throw null;
	}

	public static Vector256<int> ConvertToVector256Int32(Vector512<long> value)
	{
		throw null;
	}

	public static Vector256<int> ConvertToVector256Int32(Vector512<ulong> value)
	{
		throw null;
	}

	public static Vector256<int> ConvertToVector256Int32WithSaturation(Vector512<long> value)
	{
		throw null;
	}

	public static Vector256<int> ConvertToVector256Int32WithTruncation(Vector512<double> value)
	{
		throw null;
	}

	public static Vector256<float> ConvertToVector256Single(Vector512<double> value)
	{
		throw null;
	}

	public static Vector256<ushort> ConvertToVector256UInt16(Vector512<int> value)
	{
		throw null;
	}

	public static Vector256<ushort> ConvertToVector256UInt16(Vector512<uint> value)
	{
		throw null;
	}

	public static Vector256<ushort> ConvertToVector256UInt16WithSaturation(Vector512<uint> value)
	{
		throw null;
	}

	public static Vector256<uint> ConvertToVector256UInt32(Vector512<double> value)
	{
		throw null;
	}

	public static Vector256<uint> ConvertToVector256UInt32(Vector512<long> value)
	{
		throw null;
	}

	public static Vector256<uint> ConvertToVector256UInt32(Vector512<ulong> value)
	{
		throw null;
	}

	public static Vector256<uint> ConvertToVector256UInt32WithSaturation(Vector512<ulong> value)
	{
		throw null;
	}

	public static Vector256<uint> ConvertToVector256UInt32WithTruncation(Vector512<double> value)
	{
		throw null;
	}

	public static Vector512<double> ConvertToVector512Double(Vector256<int> value)
	{
		throw null;
	}

	public static Vector512<double> ConvertToVector512Double(Vector256<float> value)
	{
		throw null;
	}

	public static Vector512<double> ConvertToVector512Double(Vector256<uint> value)
	{
		throw null;
	}

	public static Vector512<int> ConvertToVector512Int32(Vector128<byte> value)
	{
		throw null;
	}

	public static Vector512<int> ConvertToVector512Int32(Vector256<short> value)
	{
		throw null;
	}

	public static Vector512<int> ConvertToVector512Int32(Vector128<sbyte> value)
	{
		throw null;
	}

	public static Vector512<int> ConvertToVector512Int32(Vector512<float> value)
	{
		throw null;
	}

	public static Vector512<int> ConvertToVector512Int32(Vector256<ushort> value)
	{
		throw null;
	}

	public static Vector512<int> ConvertToVector512Int32WithTruncation(Vector512<float> value)
	{
		throw null;
	}

	public static Vector512<long> ConvertToVector512Int64(Vector128<byte> value)
	{
		throw null;
	}

	public static Vector512<long> ConvertToVector512Int64(Vector128<short> value)
	{
		throw null;
	}

	public static Vector512<long> ConvertToVector512Int64(Vector256<int> value)
	{
		throw null;
	}

	public static Vector512<long> ConvertToVector512Int64(Vector128<sbyte> value)
	{
		throw null;
	}

	public static Vector512<long> ConvertToVector512Int64(Vector128<ushort> value)
	{
		throw null;
	}

	public static Vector512<long> ConvertToVector512Int64(Vector256<uint> value)
	{
		throw null;
	}

	public static Vector512<float> ConvertToVector512Single(Vector512<int> value)
	{
		throw null;
	}

	public static Vector512<float> ConvertToVector512Single(Vector512<uint> value)
	{
		throw null;
	}

	public static Vector512<uint> ConvertToVector512UInt32(Vector128<byte> value)
	{
		throw null;
	}

	public static Vector512<uint> ConvertToVector512UInt32(Vector256<short> value)
	{
		throw null;
	}

	public static Vector512<uint> ConvertToVector512UInt32(Vector128<sbyte> value)
	{
		throw null;
	}

	public static Vector512<uint> ConvertToVector512UInt32(Vector512<float> value)
	{
		throw null;
	}

	public static Vector512<uint> ConvertToVector512UInt32(Vector256<ushort> value)
	{
		throw null;
	}

	public static Vector512<uint> ConvertToVector512UInt32WithTruncation(Vector512<float> value)
	{
		throw null;
	}

	public static Vector512<ulong> ConvertToVector512UInt64(Vector128<byte> value)
	{
		throw null;
	}

	public static Vector512<ulong> ConvertToVector512UInt64(Vector128<short> value)
	{
		throw null;
	}

	public static Vector512<ulong> ConvertToVector512UInt64(Vector256<int> value)
	{
		throw null;
	}

	public static Vector512<ulong> ConvertToVector512UInt64(Vector128<sbyte> value)
	{
		throw null;
	}

	public static Vector512<ulong> ConvertToVector512UInt64(Vector128<ushort> value)
	{
		throw null;
	}

	public static Vector512<ulong> ConvertToVector512UInt64(Vector256<uint> value)
	{
		throw null;
	}

	public static Vector512<float> Divide(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<double> Divide(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<float> DuplicateEvenIndexed(Vector512<float> value)
	{
		throw null;
	}

	public static Vector512<double> DuplicateEvenIndexed(Vector512<double> value)
	{
		throw null;
	}

	public static Vector512<float> DuplicateOddIndexed(Vector512<float> value)
	{
		throw null;
	}

	public static Vector128<byte> ExtractVector128(Vector512<byte> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<double> ExtractVector128(Vector512<double> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<short> ExtractVector128(Vector512<short> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<int> ExtractVector128(Vector512<int> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<long> ExtractVector128(Vector512<long> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<sbyte> ExtractVector128(Vector512<sbyte> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<float> ExtractVector128(Vector512<float> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<ushort> ExtractVector128(Vector512<ushort> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<uint> ExtractVector128(Vector512<uint> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<ulong> ExtractVector128(Vector512<ulong> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector256<byte> ExtractVector256(Vector512<byte> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector256<double> ExtractVector256(Vector512<double> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector256<short> ExtractVector256(Vector512<short> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector256<int> ExtractVector256(Vector512<int> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector256<long> ExtractVector256(Vector512<long> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector256<sbyte> ExtractVector256(Vector512<sbyte> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector256<float> ExtractVector256(Vector512<float> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector256<ushort> ExtractVector256(Vector512<ushort> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector256<uint> ExtractVector256(Vector512<uint> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector256<ulong> ExtractVector256(Vector512<ulong> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<double> Fixup(Vector512<double> left, Vector512<double> right, Vector512<long> table, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<float> Fixup(Vector512<float> left, Vector512<float> right, Vector512<int> table, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<double> FixupScalar(Vector128<double> left, Vector128<double> right, Vector128<long> table, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<float> FixupScalar(Vector128<float> left, Vector128<float> right, Vector128<int> table, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<double> FusedMultiplyAdd(Vector512<double> a, Vector512<double> b, Vector512<double> c)
	{
		throw null;
	}

	public static Vector512<float> FusedMultiplyAdd(Vector512<float> a, Vector512<float> b, Vector512<float> c)
	{
		throw null;
	}

	public static Vector512<double> FusedMultiplyAddNegated(Vector512<double> a, Vector512<double> b, Vector512<double> c)
	{
		throw null;
	}

	public static Vector512<float> FusedMultiplyAddNegated(Vector512<float> a, Vector512<float> b, Vector512<float> c)
	{
		throw null;
	}

	public static Vector512<double> FusedMultiplyAddSubtract(Vector512<double> a, Vector512<double> b, Vector512<double> c)
	{
		throw null;
	}

	public static Vector512<float> FusedMultiplyAddSubtract(Vector512<float> a, Vector512<float> b, Vector512<float> c)
	{
		throw null;
	}

	public static Vector512<double> FusedMultiplySubtract(Vector512<double> a, Vector512<double> b, Vector512<double> c)
	{
		throw null;
	}

	public static Vector512<float> FusedMultiplySubtract(Vector512<float> a, Vector512<float> b, Vector512<float> c)
	{
		throw null;
	}

	public static Vector512<double> FusedMultiplySubtractAdd(Vector512<double> a, Vector512<double> b, Vector512<double> c)
	{
		throw null;
	}

	public static Vector512<float> FusedMultiplySubtractAdd(Vector512<float> a, Vector512<float> b, Vector512<float> c)
	{
		throw null;
	}

	public static Vector512<double> FusedMultiplySubtractNegated(Vector512<double> a, Vector512<double> b, Vector512<double> c)
	{
		throw null;
	}

	public static Vector512<float> FusedMultiplySubtractNegated(Vector512<float> a, Vector512<float> b, Vector512<float> c)
	{
		throw null;
	}

	public static Vector512<double> GetExponent(Vector512<double> value)
	{
		throw null;
	}

	public static Vector512<float> GetExponent(Vector512<float> value)
	{
		throw null;
	}

	public static Vector128<double> GetExponentScalar(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> GetExponentScalar(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<double> GetExponentScalar(Vector128<double> upper, Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> GetExponentScalar(Vector128<float> upper, Vector128<float> value)
	{
		throw null;
	}

	public static Vector512<double> GetMantissa(Vector512<double> value, [ConstantExpected(Max = 15)] byte control)
	{
		throw null;
	}

	public static Vector512<float> GetMantissa(Vector512<float> value, [ConstantExpected(Max = 15)] byte control)
	{
		throw null;
	}

	public static Vector128<float> GetMantissaScalar(Vector128<float> value, [ConstantExpected(Max = 15)] byte control)
	{
		throw null;
	}

	public static Vector128<double> GetMantissaScalar(Vector128<double> value, [ConstantExpected(Max = 15)] byte control)
	{
		throw null;
	}

	public static Vector128<float> GetMantissaScalar(Vector128<float> upper, Vector128<float> value, [ConstantExpected(Max = 15)] byte control)
	{
		throw null;
	}

	public static Vector128<double> GetMantissaScalar(Vector128<double> upper, Vector128<double> value, [ConstantExpected(Max = 15)] byte control)
	{
		throw null;
	}

	public static Vector512<byte> InsertVector128(Vector512<byte> value, Vector128<byte> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<double> InsertVector128(Vector512<double> value, Vector128<double> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<short> InsertVector128(Vector512<short> value, Vector128<short> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<int> InsertVector128(Vector512<int> value, Vector128<int> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<long> InsertVector128(Vector512<long> value, Vector128<long> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<sbyte> InsertVector128(Vector512<sbyte> value, Vector128<sbyte> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<float> InsertVector128(Vector512<float> value, Vector128<float> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<ushort> InsertVector128(Vector512<ushort> value, Vector128<ushort> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<uint> InsertVector128(Vector512<uint> value, Vector128<uint> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<ulong> InsertVector128(Vector512<ulong> value, Vector128<ulong> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<byte> InsertVector256(Vector512<byte> value, Vector256<byte> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<double> InsertVector256(Vector512<double> value, Vector256<double> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<short> InsertVector256(Vector512<short> value, Vector256<short> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<int> InsertVector256(Vector512<int> value, Vector256<int> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<long> InsertVector256(Vector512<long> value, Vector256<long> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<sbyte> InsertVector256(Vector512<sbyte> value, Vector256<sbyte> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<float> InsertVector256(Vector512<float> value, Vector256<float> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<ushort> InsertVector256(Vector512<ushort> value, Vector256<ushort> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<uint> InsertVector256(Vector512<uint> value, Vector256<uint> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<ulong> InsertVector256(Vector512<ulong> value, Vector256<ulong> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public unsafe static Vector512<byte> LoadAlignedVector512(byte* address)
	{
		throw null;
	}

	public unsafe static Vector512<double> LoadAlignedVector512(double* address)
	{
		throw null;
	}

	public unsafe static Vector512<short> LoadAlignedVector512(short* address)
	{
		throw null;
	}

	public unsafe static Vector512<int> LoadAlignedVector512(int* address)
	{
		throw null;
	}

	public unsafe static Vector512<long> LoadAlignedVector512(long* address)
	{
		throw null;
	}

	public unsafe static Vector512<sbyte> LoadAlignedVector512(sbyte* address)
	{
		throw null;
	}

	public unsafe static Vector512<float> LoadAlignedVector512(float* address)
	{
		throw null;
	}

	public unsafe static Vector512<ushort> LoadAlignedVector512(ushort* address)
	{
		throw null;
	}

	public unsafe static Vector512<uint> LoadAlignedVector512(uint* address)
	{
		throw null;
	}

	public unsafe static Vector512<ulong> LoadAlignedVector512(ulong* address)
	{
		throw null;
	}

	public unsafe static Vector512<byte> LoadAlignedVector512NonTemporal(byte* address)
	{
		throw null;
	}

	public unsafe static Vector512<short> LoadAlignedVector512NonTemporal(short* address)
	{
		throw null;
	}

	public unsafe static Vector512<int> LoadAlignedVector512NonTemporal(int* address)
	{
		throw null;
	}

	public unsafe static Vector512<long> LoadAlignedVector512NonTemporal(long* address)
	{
		throw null;
	}

	public unsafe static Vector512<sbyte> LoadAlignedVector512NonTemporal(sbyte* address)
	{
		throw null;
	}

	public unsafe static Vector512<ushort> LoadAlignedVector512NonTemporal(ushort* address)
	{
		throw null;
	}

	public unsafe static Vector512<uint> LoadAlignedVector512NonTemporal(uint* address)
	{
		throw null;
	}

	public unsafe static Vector512<ulong> LoadAlignedVector512NonTemporal(ulong* address)
	{
		throw null;
	}

	public unsafe static Vector512<byte> LoadVector512(byte* address)
	{
		throw null;
	}

	public unsafe static Vector512<double> LoadVector512(double* address)
	{
		throw null;
	}

	public unsafe static Vector512<short> LoadVector512(short* address)
	{
		throw null;
	}

	public unsafe static Vector512<int> LoadVector512(int* address)
	{
		throw null;
	}

	public unsafe static Vector512<long> LoadVector512(long* address)
	{
		throw null;
	}

	public unsafe static Vector512<sbyte> LoadVector512(sbyte* address)
	{
		throw null;
	}

	public unsafe static Vector512<float> LoadVector512(float* address)
	{
		throw null;
	}

	public unsafe static Vector512<ushort> LoadVector512(ushort* address)
	{
		throw null;
	}

	public unsafe static Vector512<uint> LoadVector512(uint* address)
	{
		throw null;
	}

	public unsafe static Vector512<ulong> LoadVector512(ulong* address)
	{
		throw null;
	}

	public static Vector512<double> Max(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<int> Max(Vector512<int> left, Vector512<int> right)
	{
		throw null;
	}

	public static Vector512<long> Max(Vector512<long> left, Vector512<long> right)
	{
		throw null;
	}

	public static Vector512<float> Max(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<uint> Max(Vector512<uint> left, Vector512<uint> right)
	{
		throw null;
	}

	public static Vector512<ulong> Max(Vector512<ulong> left, Vector512<ulong> right)
	{
		throw null;
	}

	public static Vector512<double> Min(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<int> Min(Vector512<int> left, Vector512<int> right)
	{
		throw null;
	}

	public static Vector512<long> Min(Vector512<long> left, Vector512<long> right)
	{
		throw null;
	}

	public static Vector512<float> Min(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<uint> Min(Vector512<uint> left, Vector512<uint> right)
	{
		throw null;
	}

	public static Vector512<ulong> Min(Vector512<ulong> left, Vector512<ulong> right)
	{
		throw null;
	}

	public static Vector512<long> Multiply(Vector512<int> left, Vector512<int> right)
	{
		throw null;
	}

	public static Vector512<ulong> Multiply(Vector512<uint> left, Vector512<uint> right)
	{
		throw null;
	}

	public static Vector512<float> Multiply(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<double> Multiply(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<int> MultiplyLow(Vector512<int> left, Vector512<int> right)
	{
		throw null;
	}

	public static Vector512<uint> MultiplyLow(Vector512<uint> left, Vector512<uint> right)
	{
		throw null;
	}

	public static Vector512<byte> Or(Vector512<byte> left, Vector512<byte> right)
	{
		throw null;
	}

	public static Vector512<short> Or(Vector512<short> left, Vector512<short> right)
	{
		throw null;
	}

	public static Vector512<int> Or(Vector512<int> left, Vector512<int> right)
	{
		throw null;
	}

	public static Vector512<long> Or(Vector512<long> left, Vector512<long> right)
	{
		throw null;
	}

	public static Vector512<sbyte> Or(Vector512<sbyte> left, Vector512<sbyte> right)
	{
		throw null;
	}

	public static Vector512<ushort> Or(Vector512<ushort> left, Vector512<ushort> right)
	{
		throw null;
	}

	public static Vector512<uint> Or(Vector512<uint> left, Vector512<uint> right)
	{
		throw null;
	}

	public static Vector512<ulong> Or(Vector512<ulong> left, Vector512<ulong> right)
	{
		throw null;
	}

	public static Vector512<double> Permute2x64(Vector512<double> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<float> Permute4x32(Vector512<float> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<double> Permute4x64(Vector512<double> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<long> Permute4x64(Vector512<long> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<ulong> Permute4x64(Vector512<ulong> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<double> PermuteVar2x64(Vector512<double> left, Vector512<long> control)
	{
		throw null;
	}

	public static Vector512<float> PermuteVar4x32(Vector512<float> left, Vector512<int> control)
	{
		throw null;
	}

	public static Vector512<double> PermuteVar8x64(Vector512<double> value, Vector512<long> control)
	{
		throw null;
	}

	public static Vector512<long> PermuteVar8x64(Vector512<long> value, Vector512<long> control)
	{
		throw null;
	}

	public static Vector512<ulong> PermuteVar8x64(Vector512<ulong> value, Vector512<ulong> control)
	{
		throw null;
	}

	public static Vector512<double> PermuteVar8x64x2(Vector512<double> lower, Vector512<long> indices, Vector512<double> upper)
	{
		throw null;
	}

	public static Vector512<long> PermuteVar8x64x2(Vector512<long> lower, Vector512<long> indices, Vector512<long> upper)
	{
		throw null;
	}

	public static Vector512<ulong> PermuteVar8x64x2(Vector512<ulong> lower, Vector512<ulong> indices, Vector512<ulong> upper)
	{
		throw null;
	}

	public static Vector512<int> PermuteVar16x32(Vector512<int> left, Vector512<int> control)
	{
		throw null;
	}

	public static Vector512<float> PermuteVar16x32(Vector512<float> left, Vector512<int> control)
	{
		throw null;
	}

	public static Vector512<uint> PermuteVar16x32(Vector512<uint> left, Vector512<uint> control)
	{
		throw null;
	}

	public static Vector512<int> PermuteVar16x32x2(Vector512<int> lower, Vector512<int> indices, Vector512<int> upper)
	{
		throw null;
	}

	public static Vector512<float> PermuteVar16x32x2(Vector512<float> lower, Vector512<int> indices, Vector512<float> upper)
	{
		throw null;
	}

	public static Vector512<uint> PermuteVar16x32x2(Vector512<uint> lower, Vector512<uint> indices, Vector512<uint> upper)
	{
		throw null;
	}

	public static Vector512<double> Reciprocal14(Vector512<double> value)
	{
		throw null;
	}

	public static Vector512<float> Reciprocal14(Vector512<float> value)
	{
		throw null;
	}

	public static Vector128<double> Reciprocal14Scalar(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> Reciprocal14Scalar(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<double> Reciprocal14Scalar(Vector128<double> upper, Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> Reciprocal14Scalar(Vector128<float> upper, Vector128<float> value)
	{
		throw null;
	}

	public static Vector512<double> ReciprocalSqrt14(Vector512<double> value)
	{
		throw null;
	}

	public static Vector512<float> ReciprocalSqrt14(Vector512<float> value)
	{
		throw null;
	}

	public static Vector128<double> ReciprocalSqrt14Scalar(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> ReciprocalSqrt14Scalar(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<double> ReciprocalSqrt14Scalar(Vector128<double> upper, Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> ReciprocalSqrt14Scalar(Vector128<float> upper, Vector128<float> value)
	{
		throw null;
	}

	public static Vector512<int> RotateLeft(Vector512<int> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector512<long> RotateLeft(Vector512<long> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector512<uint> RotateLeft(Vector512<uint> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector512<ulong> RotateLeft(Vector512<ulong> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector512<int> RotateLeftVariable(Vector512<int> value, Vector512<uint> count)
	{
		throw null;
	}

	public static Vector512<long> RotateLeftVariable(Vector512<long> value, Vector512<ulong> count)
	{
		throw null;
	}

	public static Vector512<uint> RotateLeftVariable(Vector512<uint> value, Vector512<uint> count)
	{
		throw null;
	}

	public static Vector512<ulong> RotateLeftVariable(Vector512<ulong> value, Vector512<ulong> count)
	{
		throw null;
	}

	public static Vector512<int> RotateRight(Vector512<int> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector512<long> RotateRight(Vector512<long> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector512<uint> RotateRight(Vector512<uint> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector512<ulong> RotateRight(Vector512<ulong> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector512<int> RotateRightVariable(Vector512<int> value, Vector512<uint> count)
	{
		throw null;
	}

	public static Vector512<long> RotateRightVariable(Vector512<long> value, Vector512<ulong> count)
	{
		throw null;
	}

	public static Vector512<uint> RotateRightVariable(Vector512<uint> value, Vector512<uint> count)
	{
		throw null;
	}

	public static Vector512<ulong> RotateRightVariable(Vector512<ulong> value, Vector512<ulong> count)
	{
		throw null;
	}

	public static Vector512<double> RoundScale(Vector512<double> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<float> RoundScale(Vector512<float> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<double> RoundScaleScalar(Vector128<double> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<float> RoundScaleScalar(Vector128<float> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<double> RoundScaleScalar(Vector128<double> upper, Vector128<double> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<float> RoundScaleScalar(Vector128<float> upper, Vector128<float> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<double> Scale(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<float> Scale(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector128<double> ScaleScalar(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<float> ScaleScalar(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector512<int> ShiftLeftLogical(Vector512<int> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector512<int> ShiftLeftLogical(Vector512<int> value, Vector128<int> count)
	{
		throw null;
	}

	public static Vector512<long> ShiftLeftLogical(Vector512<long> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector512<long> ShiftLeftLogical(Vector512<long> value, Vector128<long> count)
	{
		throw null;
	}

	public static Vector512<uint> ShiftLeftLogical(Vector512<uint> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector512<uint> ShiftLeftLogical(Vector512<uint> value, Vector128<uint> count)
	{
		throw null;
	}

	public static Vector512<ulong> ShiftLeftLogical(Vector512<ulong> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector512<ulong> ShiftLeftLogical(Vector512<ulong> value, Vector128<ulong> count)
	{
		throw null;
	}

	public static Vector512<int> ShiftLeftLogicalVariable(Vector512<int> value, Vector512<uint> count)
	{
		throw null;
	}

	public static Vector512<long> ShiftLeftLogicalVariable(Vector512<long> value, Vector512<ulong> count)
	{
		throw null;
	}

	public static Vector512<uint> ShiftLeftLogicalVariable(Vector512<uint> value, Vector512<uint> count)
	{
		throw null;
	}

	public static Vector512<ulong> ShiftLeftLogicalVariable(Vector512<ulong> value, Vector512<ulong> count)
	{
		throw null;
	}

	public static Vector512<int> ShiftRightArithmetic(Vector512<int> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector512<int> ShiftRightArithmetic(Vector512<int> value, Vector128<int> count)
	{
		throw null;
	}

	public static Vector512<long> ShiftRightArithmetic(Vector512<long> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector512<long> ShiftRightArithmetic(Vector512<long> value, Vector128<long> count)
	{
		throw null;
	}

	public static Vector512<int> ShiftRightArithmeticVariable(Vector512<int> value, Vector512<uint> count)
	{
		throw null;
	}

	public static Vector512<long> ShiftRightArithmeticVariable(Vector512<long> value, Vector512<ulong> count)
	{
		throw null;
	}

	public static Vector512<int> ShiftRightLogical(Vector512<int> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector512<int> ShiftRightLogical(Vector512<int> value, Vector128<int> count)
	{
		throw null;
	}

	public static Vector512<long> ShiftRightLogical(Vector512<long> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector512<long> ShiftRightLogical(Vector512<long> value, Vector128<long> count)
	{
		throw null;
	}

	public static Vector512<uint> ShiftRightLogical(Vector512<uint> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector512<uint> ShiftRightLogical(Vector512<uint> value, Vector128<uint> count)
	{
		throw null;
	}

	public static Vector512<ulong> ShiftRightLogical(Vector512<ulong> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector512<ulong> ShiftRightLogical(Vector512<ulong> value, Vector128<ulong> count)
	{
		throw null;
	}

	public static Vector512<int> ShiftRightLogicalVariable(Vector512<int> value, Vector512<uint> count)
	{
		throw null;
	}

	public static Vector512<long> ShiftRightLogicalVariable(Vector512<long> value, Vector512<ulong> count)
	{
		throw null;
	}

	public static Vector512<uint> ShiftRightLogicalVariable(Vector512<uint> value, Vector512<uint> count)
	{
		throw null;
	}

	public static Vector512<ulong> ShiftRightLogicalVariable(Vector512<ulong> value, Vector512<ulong> count)
	{
		throw null;
	}

	public static Vector512<double> Shuffle(Vector512<double> value, Vector512<double> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<int> Shuffle(Vector512<int> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<float> Shuffle(Vector512<float> value, Vector512<float> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<uint> Shuffle(Vector512<uint> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<double> Shuffle4x128(Vector512<double> left, Vector512<double> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<int> Shuffle4x128(Vector512<int> left, Vector512<int> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<long> Shuffle4x128(Vector512<long> left, Vector512<long> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<float> Shuffle4x128(Vector512<float> left, Vector512<float> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<uint> Shuffle4x128(Vector512<uint> left, Vector512<uint> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<ulong> Shuffle4x128(Vector512<ulong> left, Vector512<ulong> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<float> Sqrt(Vector512<float> value)
	{
		throw null;
	}

	public static Vector512<double> Sqrt(Vector512<double> value)
	{
		throw null;
	}

	public unsafe static void Store(byte* address, Vector512<byte> source)
	{
	}

	public unsafe static void Store(double* address, Vector512<double> source)
	{
	}

	public unsafe static void Store(short* address, Vector512<short> source)
	{
	}

	public unsafe static void Store(int* address, Vector512<int> source)
	{
	}

	public unsafe static void Store(long* address, Vector512<long> source)
	{
	}

	public unsafe static void Store(sbyte* address, Vector512<sbyte> source)
	{
	}

	public unsafe static void Store(float* address, Vector512<float> source)
	{
	}

	public unsafe static void Store(ushort* address, Vector512<ushort> source)
	{
	}

	public unsafe static void Store(uint* address, Vector512<uint> source)
	{
	}

	public unsafe static void Store(ulong* address, Vector512<ulong> source)
	{
	}

	public unsafe static void StoreAligned(byte* address, Vector512<byte> source)
	{
	}

	public unsafe static void StoreAligned(double* address, Vector512<double> source)
	{
	}

	public unsafe static void StoreAligned(short* address, Vector512<short> source)
	{
	}

	public unsafe static void StoreAligned(int* address, Vector512<int> source)
	{
	}

	public unsafe static void StoreAligned(long* address, Vector512<long> source)
	{
	}

	public unsafe static void StoreAligned(sbyte* address, Vector512<sbyte> source)
	{
	}

	public unsafe static void StoreAligned(float* address, Vector512<float> source)
	{
	}

	public unsafe static void StoreAligned(ushort* address, Vector512<ushort> source)
	{
	}

	public unsafe static void StoreAligned(uint* address, Vector512<uint> source)
	{
	}

	public unsafe static void StoreAligned(ulong* address, Vector512<ulong> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(byte* address, Vector512<byte> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(double* address, Vector512<double> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(short* address, Vector512<short> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(int* address, Vector512<int> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(long* address, Vector512<long> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(sbyte* address, Vector512<sbyte> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(float* address, Vector512<float> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(ushort* address, Vector512<ushort> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(uint* address, Vector512<uint> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(ulong* address, Vector512<ulong> source)
	{
	}

	public static Vector512<double> Subtract(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<int> Subtract(Vector512<int> left, Vector512<int> right)
	{
		throw null;
	}

	public static Vector512<long> Subtract(Vector512<long> left, Vector512<long> right)
	{
		throw null;
	}

	public static Vector512<float> Subtract(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<uint> Subtract(Vector512<uint> left, Vector512<uint> right)
	{
		throw null;
	}

	public static Vector512<ulong> Subtract(Vector512<ulong> left, Vector512<ulong> right)
	{
		throw null;
	}

	public static Vector512<byte> TernaryLogic(Vector512<byte> a, Vector512<byte> b, Vector512<byte> c, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<double> TernaryLogic(Vector512<double> a, Vector512<double> b, Vector512<double> c, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<int> TernaryLogic(Vector512<int> a, Vector512<int> b, Vector512<int> c, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<short> TernaryLogic(Vector512<short> a, Vector512<short> b, Vector512<short> c, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<long> TernaryLogic(Vector512<long> a, Vector512<long> b, Vector512<long> c, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<sbyte> TernaryLogic(Vector512<sbyte> a, Vector512<sbyte> b, Vector512<sbyte> c, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<float> TernaryLogic(Vector512<float> a, Vector512<float> b, Vector512<float> c, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<ushort> TernaryLogic(Vector512<ushort> a, Vector512<ushort> b, Vector512<ushort> c, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<uint> TernaryLogic(Vector512<uint> a, Vector512<uint> b, Vector512<uint> c, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<ulong> TernaryLogic(Vector512<ulong> a, Vector512<ulong> b, Vector512<ulong> c, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<double> UnpackHigh(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<int> UnpackHigh(Vector512<int> left, Vector512<int> right)
	{
		throw null;
	}

	public static Vector512<long> UnpackHigh(Vector512<long> left, Vector512<long> right)
	{
		throw null;
	}

	public static Vector512<float> UnpackHigh(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<uint> UnpackHigh(Vector512<uint> left, Vector512<uint> right)
	{
		throw null;
	}

	public static Vector512<ulong> UnpackHigh(Vector512<ulong> left, Vector512<ulong> right)
	{
		throw null;
	}

	public static Vector512<double> UnpackLow(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<int> UnpackLow(Vector512<int> left, Vector512<int> right)
	{
		throw null;
	}

	public static Vector512<long> UnpackLow(Vector512<long> left, Vector512<long> right)
	{
		throw null;
	}

	public static Vector512<float> UnpackLow(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<uint> UnpackLow(Vector512<uint> left, Vector512<uint> right)
	{
		throw null;
	}

	public static Vector512<ulong> UnpackLow(Vector512<ulong> left, Vector512<ulong> right)
	{
		throw null;
	}

	public static Vector512<byte> Xor(Vector512<byte> left, Vector512<byte> right)
	{
		throw null;
	}

	public static Vector512<short> Xor(Vector512<short> left, Vector512<short> right)
	{
		throw null;
	}

	public static Vector512<int> Xor(Vector512<int> left, Vector512<int> right)
	{
		throw null;
	}

	public static Vector512<long> Xor(Vector512<long> left, Vector512<long> right)
	{
		throw null;
	}

	public static Vector512<sbyte> Xor(Vector512<sbyte> left, Vector512<sbyte> right)
	{
		throw null;
	}

	public static Vector512<ushort> Xor(Vector512<ushort> left, Vector512<ushort> right)
	{
		throw null;
	}

	public static Vector512<uint> Xor(Vector512<uint> left, Vector512<uint> right)
	{
		throw null;
	}

	public static Vector512<ulong> Xor(Vector512<ulong> left, Vector512<ulong> right)
	{
		throw null;
	}
}
