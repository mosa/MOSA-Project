using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class Avx : Sse42
{
	public new abstract class X64 : Sse42.X64
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
	}

	public new static bool IsSupported
	{
		get
		{
			throw null;
		}
	}

	internal Avx()
	{
	}

	public static Vector256<double> Add(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> Add(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<double> AddSubtract(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> AddSubtract(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<double> And(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> And(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<double> AndNot(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> AndNot(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<double> Blend(Vector256<double> left, Vector256<double> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<float> Blend(Vector256<float> left, Vector256<float> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<double> BlendVariable(Vector256<double> left, Vector256<double> right, Vector256<double> mask)
	{
		throw null;
	}

	public static Vector256<float> BlendVariable(Vector256<float> left, Vector256<float> right, Vector256<float> mask)
	{
		throw null;
	}

	public unsafe static Vector128<float> BroadcastScalarToVector128(float* source)
	{
		throw null;
	}

	public unsafe static Vector256<double> BroadcastScalarToVector256(double* source)
	{
		throw null;
	}

	public unsafe static Vector256<float> BroadcastScalarToVector256(float* source)
	{
		throw null;
	}

	public unsafe static Vector256<double> BroadcastVector128ToVector256(double* address)
	{
		throw null;
	}

	public unsafe static Vector256<float> BroadcastVector128ToVector256(float* address)
	{
		throw null;
	}

	public static Vector256<double> Ceiling(Vector256<double> value)
	{
		throw null;
	}

	public static Vector256<float> Ceiling(Vector256<float> value)
	{
		throw null;
	}

	public static Vector128<double> Compare(Vector128<double> left, Vector128<double> right, [ConstantExpected(Max = FloatComparisonMode.UnorderedTrueSignaling)] FloatComparisonMode mode)
	{
		throw null;
	}

	public static Vector128<float> Compare(Vector128<float> left, Vector128<float> right, [ConstantExpected(Max = FloatComparisonMode.UnorderedTrueSignaling)] FloatComparisonMode mode)
	{
		throw null;
	}

	public static Vector256<double> Compare(Vector256<double> left, Vector256<double> right, [ConstantExpected(Max = FloatComparisonMode.UnorderedTrueSignaling)] FloatComparisonMode mode)
	{
		throw null;
	}

	public static Vector256<float> Compare(Vector256<float> left, Vector256<float> right, [ConstantExpected(Max = FloatComparisonMode.UnorderedTrueSignaling)] FloatComparisonMode mode)
	{
		throw null;
	}

	public static Vector256<double> CompareEqual(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> CompareEqual(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<double> CompareGreaterThan(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> CompareGreaterThan(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<double> CompareGreaterThanOrEqual(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> CompareGreaterThanOrEqual(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<double> CompareLessThan(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> CompareLessThan(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<double> CompareLessThanOrEqual(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> CompareLessThanOrEqual(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<double> CompareNotEqual(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> CompareNotEqual(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<double> CompareNotGreaterThan(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> CompareNotGreaterThan(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<double> CompareNotGreaterThanOrEqual(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> CompareNotGreaterThanOrEqual(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<double> CompareNotLessThan(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> CompareNotLessThan(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<double> CompareNotLessThanOrEqual(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> CompareNotLessThanOrEqual(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<double> CompareOrdered(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> CompareOrdered(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector128<double> CompareScalar(Vector128<double> left, Vector128<double> right, [ConstantExpected(Max = FloatComparisonMode.UnorderedTrueSignaling)] FloatComparisonMode mode)
	{
		throw null;
	}

	public static Vector128<float> CompareScalar(Vector128<float> left, Vector128<float> right, [ConstantExpected(Max = FloatComparisonMode.UnorderedTrueSignaling)] FloatComparisonMode mode)
	{
		throw null;
	}

	public static Vector256<double> CompareUnordered(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> CompareUnordered(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector128<int> ConvertToVector128Int32(Vector256<double> value)
	{
		throw null;
	}

	public static Vector128<int> ConvertToVector128Int32WithTruncation(Vector256<double> value)
	{
		throw null;
	}

	public static Vector128<float> ConvertToVector128Single(Vector256<double> value)
	{
		throw null;
	}

	public static Vector256<double> ConvertToVector256Double(Vector128<int> value)
	{
		throw null;
	}

	public static Vector256<double> ConvertToVector256Double(Vector128<float> value)
	{
		throw null;
	}

	public static Vector256<int> ConvertToVector256Int32(Vector256<float> value)
	{
		throw null;
	}

	public static Vector256<int> ConvertToVector256Int32WithTruncation(Vector256<float> value)
	{
		throw null;
	}

	public static Vector256<float> ConvertToVector256Single(Vector256<int> value)
	{
		throw null;
	}

	public static Vector256<double> Divide(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> Divide(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<float> DotProduct(Vector256<float> left, Vector256<float> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<double> DuplicateEvenIndexed(Vector256<double> value)
	{
		throw null;
	}

	public static Vector256<float> DuplicateEvenIndexed(Vector256<float> value)
	{
		throw null;
	}

	public static Vector256<float> DuplicateOddIndexed(Vector256<float> value)
	{
		throw null;
	}

	public static Vector128<byte> ExtractVector128(Vector256<byte> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<double> ExtractVector128(Vector256<double> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<short> ExtractVector128(Vector256<short> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<int> ExtractVector128(Vector256<int> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<long> ExtractVector128(Vector256<long> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<sbyte> ExtractVector128(Vector256<sbyte> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<float> ExtractVector128(Vector256<float> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<ushort> ExtractVector128(Vector256<ushort> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<uint> ExtractVector128(Vector256<uint> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<ulong> ExtractVector128(Vector256<ulong> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector256<double> Floor(Vector256<double> value)
	{
		throw null;
	}

	public static Vector256<float> Floor(Vector256<float> value)
	{
		throw null;
	}

	public static Vector256<double> HorizontalAdd(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> HorizontalAdd(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<double> HorizontalSubtract(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> HorizontalSubtract(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<byte> InsertVector128(Vector256<byte> value, Vector128<byte> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector256<double> InsertVector128(Vector256<double> value, Vector128<double> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector256<short> InsertVector128(Vector256<short> value, Vector128<short> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector256<int> InsertVector128(Vector256<int> value, Vector128<int> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector256<long> InsertVector128(Vector256<long> value, Vector128<long> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector256<sbyte> InsertVector128(Vector256<sbyte> value, Vector128<sbyte> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector256<float> InsertVector128(Vector256<float> value, Vector128<float> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector256<ushort> InsertVector128(Vector256<ushort> value, Vector128<ushort> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector256<uint> InsertVector128(Vector256<uint> value, Vector128<uint> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector256<ulong> InsertVector128(Vector256<ulong> value, Vector128<ulong> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public unsafe static Vector256<byte> LoadAlignedVector256(byte* address)
	{
		throw null;
	}

	public unsafe static Vector256<double> LoadAlignedVector256(double* address)
	{
		throw null;
	}

	public unsafe static Vector256<short> LoadAlignedVector256(short* address)
	{
		throw null;
	}

	public unsafe static Vector256<int> LoadAlignedVector256(int* address)
	{
		throw null;
	}

	public unsafe static Vector256<long> LoadAlignedVector256(long* address)
	{
		throw null;
	}

	public unsafe static Vector256<sbyte> LoadAlignedVector256(sbyte* address)
	{
		throw null;
	}

	public unsafe static Vector256<float> LoadAlignedVector256(float* address)
	{
		throw null;
	}

	public unsafe static Vector256<ushort> LoadAlignedVector256(ushort* address)
	{
		throw null;
	}

	public unsafe static Vector256<uint> LoadAlignedVector256(uint* address)
	{
		throw null;
	}

	public unsafe static Vector256<ulong> LoadAlignedVector256(ulong* address)
	{
		throw null;
	}

	public unsafe static Vector256<byte> LoadDquVector256(byte* address)
	{
		throw null;
	}

	public unsafe static Vector256<short> LoadDquVector256(short* address)
	{
		throw null;
	}

	public unsafe static Vector256<int> LoadDquVector256(int* address)
	{
		throw null;
	}

	public unsafe static Vector256<long> LoadDquVector256(long* address)
	{
		throw null;
	}

	public unsafe static Vector256<sbyte> LoadDquVector256(sbyte* address)
	{
		throw null;
	}

	public unsafe static Vector256<ushort> LoadDquVector256(ushort* address)
	{
		throw null;
	}

	public unsafe static Vector256<uint> LoadDquVector256(uint* address)
	{
		throw null;
	}

	public unsafe static Vector256<ulong> LoadDquVector256(ulong* address)
	{
		throw null;
	}

	public unsafe static Vector256<byte> LoadVector256(byte* address)
	{
		throw null;
	}

	public unsafe static Vector256<double> LoadVector256(double* address)
	{
		throw null;
	}

	public unsafe static Vector256<short> LoadVector256(short* address)
	{
		throw null;
	}

	public unsafe static Vector256<int> LoadVector256(int* address)
	{
		throw null;
	}

	public unsafe static Vector256<long> LoadVector256(long* address)
	{
		throw null;
	}

	public unsafe static Vector256<sbyte> LoadVector256(sbyte* address)
	{
		throw null;
	}

	public unsafe static Vector256<float> LoadVector256(float* address)
	{
		throw null;
	}

	public unsafe static Vector256<ushort> LoadVector256(ushort* address)
	{
		throw null;
	}

	public unsafe static Vector256<uint> LoadVector256(uint* address)
	{
		throw null;
	}

	public unsafe static Vector256<ulong> LoadVector256(ulong* address)
	{
		throw null;
	}

	public unsafe static Vector128<double> MaskLoad(double* address, Vector128<double> mask)
	{
		throw null;
	}

	public unsafe static Vector256<double> MaskLoad(double* address, Vector256<double> mask)
	{
		throw null;
	}

	public unsafe static Vector128<float> MaskLoad(float* address, Vector128<float> mask)
	{
		throw null;
	}

	public unsafe static Vector256<float> MaskLoad(float* address, Vector256<float> mask)
	{
		throw null;
	}

	public unsafe static void MaskStore(double* address, Vector128<double> mask, Vector128<double> source)
	{
	}

	public unsafe static void MaskStore(double* address, Vector256<double> mask, Vector256<double> source)
	{
	}

	public unsafe static void MaskStore(float* address, Vector128<float> mask, Vector128<float> source)
	{
	}

	public unsafe static void MaskStore(float* address, Vector256<float> mask, Vector256<float> source)
	{
	}

	public static Vector256<double> Max(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> Max(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<double> Min(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> Min(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static int MoveMask(Vector256<double> value)
	{
		throw null;
	}

	public static int MoveMask(Vector256<float> value)
	{
		throw null;
	}

	public static Vector256<double> Multiply(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> Multiply(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<double> Or(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> Or(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector128<double> Permute(Vector128<double> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<float> Permute(Vector128<float> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<double> Permute(Vector256<double> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<float> Permute(Vector256<float> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<byte> Permute2x128(Vector256<byte> left, Vector256<byte> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<double> Permute2x128(Vector256<double> left, Vector256<double> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<short> Permute2x128(Vector256<short> left, Vector256<short> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<int> Permute2x128(Vector256<int> left, Vector256<int> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<long> Permute2x128(Vector256<long> left, Vector256<long> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<sbyte> Permute2x128(Vector256<sbyte> left, Vector256<sbyte> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<float> Permute2x128(Vector256<float> left, Vector256<float> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<ushort> Permute2x128(Vector256<ushort> left, Vector256<ushort> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<uint> Permute2x128(Vector256<uint> left, Vector256<uint> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<ulong> Permute2x128(Vector256<ulong> left, Vector256<ulong> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<double> PermuteVar(Vector128<double> left, Vector128<long> control)
	{
		throw null;
	}

	public static Vector128<float> PermuteVar(Vector128<float> left, Vector128<int> control)
	{
		throw null;
	}

	public static Vector256<double> PermuteVar(Vector256<double> left, Vector256<long> control)
	{
		throw null;
	}

	public static Vector256<float> PermuteVar(Vector256<float> left, Vector256<int> control)
	{
		throw null;
	}

	public static Vector256<float> Reciprocal(Vector256<float> value)
	{
		throw null;
	}

	public static Vector256<float> ReciprocalSqrt(Vector256<float> value)
	{
		throw null;
	}

	public static Vector256<double> RoundCurrentDirection(Vector256<double> value)
	{
		throw null;
	}

	public static Vector256<float> RoundCurrentDirection(Vector256<float> value)
	{
		throw null;
	}

	public static Vector256<double> RoundToNearestInteger(Vector256<double> value)
	{
		throw null;
	}

	public static Vector256<float> RoundToNearestInteger(Vector256<float> value)
	{
		throw null;
	}

	public static Vector256<double> RoundToNegativeInfinity(Vector256<double> value)
	{
		throw null;
	}

	public static Vector256<float> RoundToNegativeInfinity(Vector256<float> value)
	{
		throw null;
	}

	public static Vector256<double> RoundToPositiveInfinity(Vector256<double> value)
	{
		throw null;
	}

	public static Vector256<float> RoundToPositiveInfinity(Vector256<float> value)
	{
		throw null;
	}

	public static Vector256<double> RoundToZero(Vector256<double> value)
	{
		throw null;
	}

	public static Vector256<float> RoundToZero(Vector256<float> value)
	{
		throw null;
	}

	public static Vector256<double> Shuffle(Vector256<double> value, Vector256<double> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<float> Shuffle(Vector256<float> value, Vector256<float> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<double> Sqrt(Vector256<double> value)
	{
		throw null;
	}

	public static Vector256<float> Sqrt(Vector256<float> value)
	{
		throw null;
	}

	public unsafe static void Store(byte* address, Vector256<byte> source)
	{
	}

	public unsafe static void Store(double* address, Vector256<double> source)
	{
	}

	public unsafe static void Store(short* address, Vector256<short> source)
	{
	}

	public unsafe static void Store(int* address, Vector256<int> source)
	{
	}

	public unsafe static void Store(long* address, Vector256<long> source)
	{
	}

	public unsafe static void Store(sbyte* address, Vector256<sbyte> source)
	{
	}

	public unsafe static void Store(float* address, Vector256<float> source)
	{
	}

	public unsafe static void Store(ushort* address, Vector256<ushort> source)
	{
	}

	public unsafe static void Store(uint* address, Vector256<uint> source)
	{
	}

	public unsafe static void Store(ulong* address, Vector256<ulong> source)
	{
	}

	public unsafe static void StoreAligned(byte* address, Vector256<byte> source)
	{
	}

	public unsafe static void StoreAligned(double* address, Vector256<double> source)
	{
	}

	public unsafe static void StoreAligned(short* address, Vector256<short> source)
	{
	}

	public unsafe static void StoreAligned(int* address, Vector256<int> source)
	{
	}

	public unsafe static void StoreAligned(long* address, Vector256<long> source)
	{
	}

	public unsafe static void StoreAligned(sbyte* address, Vector256<sbyte> source)
	{
	}

	public unsafe static void StoreAligned(float* address, Vector256<float> source)
	{
	}

	public unsafe static void StoreAligned(ushort* address, Vector256<ushort> source)
	{
	}

	public unsafe static void StoreAligned(uint* address, Vector256<uint> source)
	{
	}

	public unsafe static void StoreAligned(ulong* address, Vector256<ulong> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(byte* address, Vector256<byte> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(double* address, Vector256<double> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(short* address, Vector256<short> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(int* address, Vector256<int> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(long* address, Vector256<long> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(sbyte* address, Vector256<sbyte> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(float* address, Vector256<float> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(ushort* address, Vector256<ushort> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(uint* address, Vector256<uint> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(ulong* address, Vector256<ulong> source)
	{
	}

	public static Vector256<double> Subtract(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> Subtract(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static bool TestC(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static bool TestC(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static bool TestC(Vector256<byte> left, Vector256<byte> right)
	{
		throw null;
	}

	public static bool TestC(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static bool TestC(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static bool TestC(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static bool TestC(Vector256<long> left, Vector256<long> right)
	{
		throw null;
	}

	public static bool TestC(Vector256<sbyte> left, Vector256<sbyte> right)
	{
		throw null;
	}

	public static bool TestC(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static bool TestC(Vector256<ushort> left, Vector256<ushort> right)
	{
		throw null;
	}

	public static bool TestC(Vector256<uint> left, Vector256<uint> right)
	{
		throw null;
	}

	public static bool TestC(Vector256<ulong> left, Vector256<ulong> right)
	{
		throw null;
	}

	public static bool TestNotZAndNotC(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static bool TestNotZAndNotC(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static bool TestNotZAndNotC(Vector256<byte> left, Vector256<byte> right)
	{
		throw null;
	}

	public static bool TestNotZAndNotC(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static bool TestNotZAndNotC(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static bool TestNotZAndNotC(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static bool TestNotZAndNotC(Vector256<long> left, Vector256<long> right)
	{
		throw null;
	}

	public static bool TestNotZAndNotC(Vector256<sbyte> left, Vector256<sbyte> right)
	{
		throw null;
	}

	public static bool TestNotZAndNotC(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static bool TestNotZAndNotC(Vector256<ushort> left, Vector256<ushort> right)
	{
		throw null;
	}

	public static bool TestNotZAndNotC(Vector256<uint> left, Vector256<uint> right)
	{
		throw null;
	}

	public static bool TestNotZAndNotC(Vector256<ulong> left, Vector256<ulong> right)
	{
		throw null;
	}

	public static bool TestZ(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static bool TestZ(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static bool TestZ(Vector256<byte> left, Vector256<byte> right)
	{
		throw null;
	}

	public static bool TestZ(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static bool TestZ(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static bool TestZ(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static bool TestZ(Vector256<long> left, Vector256<long> right)
	{
		throw null;
	}

	public static bool TestZ(Vector256<sbyte> left, Vector256<sbyte> right)
	{
		throw null;
	}

	public static bool TestZ(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static bool TestZ(Vector256<ushort> left, Vector256<ushort> right)
	{
		throw null;
	}

	public static bool TestZ(Vector256<uint> left, Vector256<uint> right)
	{
		throw null;
	}

	public static bool TestZ(Vector256<ulong> left, Vector256<ulong> right)
	{
		throw null;
	}

	public static Vector256<double> UnpackHigh(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> UnpackHigh(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<double> UnpackLow(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> UnpackLow(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}

	public static Vector256<double> Xor(Vector256<double> left, Vector256<double> right)
	{
		throw null;
	}

	public static Vector256<float> Xor(Vector256<float> left, Vector256<float> right)
	{
		throw null;
	}
}
