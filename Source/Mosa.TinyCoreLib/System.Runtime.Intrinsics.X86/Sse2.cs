using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class Sse2 : Sse
{
	public new abstract class X64 : Sse.X64
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

		public static Vector128<double> ConvertScalarToVector128Double(Vector128<double> upper, long value)
		{
			throw null;
		}

		public static Vector128<long> ConvertScalarToVector128Int64(long value)
		{
			throw null;
		}

		public static Vector128<ulong> ConvertScalarToVector128UInt64(ulong value)
		{
			throw null;
		}

		public static long ConvertToInt64(Vector128<double> value)
		{
			throw null;
		}

		public static long ConvertToInt64(Vector128<long> value)
		{
			throw null;
		}

		public static long ConvertToInt64WithTruncation(Vector128<double> value)
		{
			throw null;
		}

		public static ulong ConvertToUInt64(Vector128<ulong> value)
		{
			throw null;
		}

		public unsafe static void StoreNonTemporal(long* address, long value)
		{
		}

		public unsafe static void StoreNonTemporal(ulong* address, ulong value)
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

	internal Sse2()
	{
	}

	public static Vector128<byte> Add(Vector128<byte> left, Vector128<byte> right)
	{
		throw null;
	}

	public static Vector128<double> Add(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<short> Add(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static Vector128<int> Add(Vector128<int> left, Vector128<int> right)
	{
		throw null;
	}

	public static Vector128<long> Add(Vector128<long> left, Vector128<long> right)
	{
		throw null;
	}

	public static Vector128<sbyte> Add(Vector128<sbyte> left, Vector128<sbyte> right)
	{
		throw null;
	}

	public static Vector128<ushort> Add(Vector128<ushort> left, Vector128<ushort> right)
	{
		throw null;
	}

	public static Vector128<uint> Add(Vector128<uint> left, Vector128<uint> right)
	{
		throw null;
	}

	public static Vector128<ulong> Add(Vector128<ulong> left, Vector128<ulong> right)
	{
		throw null;
	}

	public static Vector128<byte> AddSaturate(Vector128<byte> left, Vector128<byte> right)
	{
		throw null;
	}

	public static Vector128<short> AddSaturate(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static Vector128<sbyte> AddSaturate(Vector128<sbyte> left, Vector128<sbyte> right)
	{
		throw null;
	}

	public static Vector128<ushort> AddSaturate(Vector128<ushort> left, Vector128<ushort> right)
	{
		throw null;
	}

	public static Vector128<double> AddScalar(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<byte> And(Vector128<byte> left, Vector128<byte> right)
	{
		throw null;
	}

	public static Vector128<double> And(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<short> And(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static Vector128<int> And(Vector128<int> left, Vector128<int> right)
	{
		throw null;
	}

	public static Vector128<long> And(Vector128<long> left, Vector128<long> right)
	{
		throw null;
	}

	public static Vector128<sbyte> And(Vector128<sbyte> left, Vector128<sbyte> right)
	{
		throw null;
	}

	public static Vector128<ushort> And(Vector128<ushort> left, Vector128<ushort> right)
	{
		throw null;
	}

	public static Vector128<uint> And(Vector128<uint> left, Vector128<uint> right)
	{
		throw null;
	}

	public static Vector128<ulong> And(Vector128<ulong> left, Vector128<ulong> right)
	{
		throw null;
	}

	public static Vector128<byte> AndNot(Vector128<byte> left, Vector128<byte> right)
	{
		throw null;
	}

	public static Vector128<double> AndNot(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<short> AndNot(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static Vector128<int> AndNot(Vector128<int> left, Vector128<int> right)
	{
		throw null;
	}

	public static Vector128<long> AndNot(Vector128<long> left, Vector128<long> right)
	{
		throw null;
	}

	public static Vector128<sbyte> AndNot(Vector128<sbyte> left, Vector128<sbyte> right)
	{
		throw null;
	}

	public static Vector128<ushort> AndNot(Vector128<ushort> left, Vector128<ushort> right)
	{
		throw null;
	}

	public static Vector128<uint> AndNot(Vector128<uint> left, Vector128<uint> right)
	{
		throw null;
	}

	public static Vector128<ulong> AndNot(Vector128<ulong> left, Vector128<ulong> right)
	{
		throw null;
	}

	public static Vector128<byte> Average(Vector128<byte> left, Vector128<byte> right)
	{
		throw null;
	}

	public static Vector128<ushort> Average(Vector128<ushort> left, Vector128<ushort> right)
	{
		throw null;
	}

	public static Vector128<byte> CompareEqual(Vector128<byte> left, Vector128<byte> right)
	{
		throw null;
	}

	public static Vector128<double> CompareEqual(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<short> CompareEqual(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static Vector128<int> CompareEqual(Vector128<int> left, Vector128<int> right)
	{
		throw null;
	}

	public static Vector128<sbyte> CompareEqual(Vector128<sbyte> left, Vector128<sbyte> right)
	{
		throw null;
	}

	public static Vector128<ushort> CompareEqual(Vector128<ushort> left, Vector128<ushort> right)
	{
		throw null;
	}

	public static Vector128<uint> CompareEqual(Vector128<uint> left, Vector128<uint> right)
	{
		throw null;
	}

	public static Vector128<double> CompareGreaterThan(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<short> CompareGreaterThan(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static Vector128<int> CompareGreaterThan(Vector128<int> left, Vector128<int> right)
	{
		throw null;
	}

	public static Vector128<sbyte> CompareGreaterThan(Vector128<sbyte> left, Vector128<sbyte> right)
	{
		throw null;
	}

	public static Vector128<double> CompareGreaterThanOrEqual(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> CompareLessThan(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<short> CompareLessThan(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static Vector128<int> CompareLessThan(Vector128<int> left, Vector128<int> right)
	{
		throw null;
	}

	public static Vector128<sbyte> CompareLessThan(Vector128<sbyte> left, Vector128<sbyte> right)
	{
		throw null;
	}

	public static Vector128<double> CompareLessThanOrEqual(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> CompareNotEqual(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> CompareNotGreaterThan(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> CompareNotGreaterThanOrEqual(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> CompareNotLessThan(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> CompareNotLessThanOrEqual(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> CompareOrdered(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> CompareScalarEqual(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> CompareScalarGreaterThan(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> CompareScalarGreaterThanOrEqual(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> CompareScalarLessThan(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> CompareScalarLessThanOrEqual(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> CompareScalarNotEqual(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> CompareScalarNotGreaterThan(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> CompareScalarNotGreaterThanOrEqual(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> CompareScalarNotLessThan(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> CompareScalarNotLessThanOrEqual(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> CompareScalarOrdered(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static bool CompareScalarOrderedEqual(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static bool CompareScalarOrderedGreaterThan(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static bool CompareScalarOrderedGreaterThanOrEqual(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static bool CompareScalarOrderedLessThan(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static bool CompareScalarOrderedLessThanOrEqual(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static bool CompareScalarOrderedNotEqual(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> CompareScalarUnordered(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static bool CompareScalarUnorderedEqual(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static bool CompareScalarUnorderedGreaterThan(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static bool CompareScalarUnorderedGreaterThanOrEqual(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static bool CompareScalarUnorderedLessThan(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static bool CompareScalarUnorderedLessThanOrEqual(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static bool CompareScalarUnorderedNotEqual(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> CompareUnordered(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> ConvertScalarToVector128Double(Vector128<double> upper, int value)
	{
		throw null;
	}

	public static Vector128<double> ConvertScalarToVector128Double(Vector128<double> upper, Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<int> ConvertScalarToVector128Int32(int value)
	{
		throw null;
	}

	public static Vector128<float> ConvertScalarToVector128Single(Vector128<float> upper, Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<uint> ConvertScalarToVector128UInt32(uint value)
	{
		throw null;
	}

	public static int ConvertToInt32(Vector128<double> value)
	{
		throw null;
	}

	public static int ConvertToInt32(Vector128<int> value)
	{
		throw null;
	}

	public static int ConvertToInt32WithTruncation(Vector128<double> value)
	{
		throw null;
	}

	public static uint ConvertToUInt32(Vector128<uint> value)
	{
		throw null;
	}

	public static Vector128<double> ConvertToVector128Double(Vector128<int> value)
	{
		throw null;
	}

	public static Vector128<double> ConvertToVector128Double(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<int> ConvertToVector128Int32(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<int> ConvertToVector128Int32(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<int> ConvertToVector128Int32WithTruncation(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<int> ConvertToVector128Int32WithTruncation(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<float> ConvertToVector128Single(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> ConvertToVector128Single(Vector128<int> value)
	{
		throw null;
	}

	public static Vector128<double> Divide(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<double> DivideScalar(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static ushort Extract(Vector128<ushort> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<short> Insert(Vector128<short> value, short data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<ushort> Insert(Vector128<ushort> value, ushort data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public unsafe static Vector128<byte> LoadAlignedVector128(byte* address)
	{
		throw null;
	}

	public unsafe static Vector128<double> LoadAlignedVector128(double* address)
	{
		throw null;
	}

	public unsafe static Vector128<short> LoadAlignedVector128(short* address)
	{
		throw null;
	}

	public unsafe static Vector128<int> LoadAlignedVector128(int* address)
	{
		throw null;
	}

	public unsafe static Vector128<long> LoadAlignedVector128(long* address)
	{
		throw null;
	}

	public unsafe static Vector128<sbyte> LoadAlignedVector128(sbyte* address)
	{
		throw null;
	}

	public unsafe static Vector128<ushort> LoadAlignedVector128(ushort* address)
	{
		throw null;
	}

	public unsafe static Vector128<uint> LoadAlignedVector128(uint* address)
	{
		throw null;
	}

	public unsafe static Vector128<ulong> LoadAlignedVector128(ulong* address)
	{
		throw null;
	}

	public static void LoadFence()
	{
	}

	public unsafe static Vector128<double> LoadHigh(Vector128<double> lower, double* address)
	{
		throw null;
	}

	public unsafe static Vector128<double> LoadLow(Vector128<double> upper, double* address)
	{
		throw null;
	}

	public unsafe static Vector128<double> LoadScalarVector128(double* address)
	{
		throw null;
	}

	public unsafe static Vector128<int> LoadScalarVector128(int* address)
	{
		throw null;
	}

	public unsafe static Vector128<long> LoadScalarVector128(long* address)
	{
		throw null;
	}

	public unsafe static Vector128<uint> LoadScalarVector128(uint* address)
	{
		throw null;
	}

	public unsafe static Vector128<ulong> LoadScalarVector128(ulong* address)
	{
		throw null;
	}

	public unsafe static Vector128<byte> LoadVector128(byte* address)
	{
		throw null;
	}

	public unsafe static Vector128<double> LoadVector128(double* address)
	{
		throw null;
	}

	public unsafe static Vector128<short> LoadVector128(short* address)
	{
		throw null;
	}

	public unsafe static Vector128<int> LoadVector128(int* address)
	{
		throw null;
	}

	public unsafe static Vector128<long> LoadVector128(long* address)
	{
		throw null;
	}

	public unsafe static Vector128<sbyte> LoadVector128(sbyte* address)
	{
		throw null;
	}

	public unsafe static Vector128<ushort> LoadVector128(ushort* address)
	{
		throw null;
	}

	public unsafe static Vector128<uint> LoadVector128(uint* address)
	{
		throw null;
	}

	public unsafe static Vector128<ulong> LoadVector128(ulong* address)
	{
		throw null;
	}

	public unsafe static void MaskMove(Vector128<byte> source, Vector128<byte> mask, byte* address)
	{
	}

	public unsafe static void MaskMove(Vector128<sbyte> source, Vector128<sbyte> mask, sbyte* address)
	{
	}

	public static Vector128<byte> Max(Vector128<byte> left, Vector128<byte> right)
	{
		throw null;
	}

	public static Vector128<double> Max(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<short> Max(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static Vector128<double> MaxScalar(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static void MemoryFence()
	{
	}

	public static Vector128<byte> Min(Vector128<byte> left, Vector128<byte> right)
	{
		throw null;
	}

	public static Vector128<double> Min(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<short> Min(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static Vector128<double> MinScalar(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static int MoveMask(Vector128<byte> value)
	{
		throw null;
	}

	public static int MoveMask(Vector128<double> value)
	{
		throw null;
	}

	public static int MoveMask(Vector128<sbyte> value)
	{
		throw null;
	}

	public static Vector128<double> MoveScalar(Vector128<double> upper, Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<long> MoveScalar(Vector128<long> value)
	{
		throw null;
	}

	public static Vector128<ulong> MoveScalar(Vector128<ulong> value)
	{
		throw null;
	}

	public static Vector128<double> Multiply(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<ulong> Multiply(Vector128<uint> left, Vector128<uint> right)
	{
		throw null;
	}

	public static Vector128<int> MultiplyAddAdjacent(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static Vector128<short> MultiplyHigh(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static Vector128<ushort> MultiplyHigh(Vector128<ushort> left, Vector128<ushort> right)
	{
		throw null;
	}

	public static Vector128<short> MultiplyLow(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static Vector128<ushort> MultiplyLow(Vector128<ushort> left, Vector128<ushort> right)
	{
		throw null;
	}

	public static Vector128<double> MultiplyScalar(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<byte> Or(Vector128<byte> left, Vector128<byte> right)
	{
		throw null;
	}

	public static Vector128<double> Or(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<short> Or(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static Vector128<int> Or(Vector128<int> left, Vector128<int> right)
	{
		throw null;
	}

	public static Vector128<long> Or(Vector128<long> left, Vector128<long> right)
	{
		throw null;
	}

	public static Vector128<sbyte> Or(Vector128<sbyte> left, Vector128<sbyte> right)
	{
		throw null;
	}

	public static Vector128<ushort> Or(Vector128<ushort> left, Vector128<ushort> right)
	{
		throw null;
	}

	public static Vector128<uint> Or(Vector128<uint> left, Vector128<uint> right)
	{
		throw null;
	}

	public static Vector128<ulong> Or(Vector128<ulong> left, Vector128<ulong> right)
	{
		throw null;
	}

	public static Vector128<sbyte> PackSignedSaturate(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static Vector128<short> PackSignedSaturate(Vector128<int> left, Vector128<int> right)
	{
		throw null;
	}

	public static Vector128<byte> PackUnsignedSaturate(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static Vector128<short> ShiftLeftLogical(Vector128<short> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector128<short> ShiftLeftLogical(Vector128<short> value, Vector128<short> count)
	{
		throw null;
	}

	public static Vector128<int> ShiftLeftLogical(Vector128<int> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector128<int> ShiftLeftLogical(Vector128<int> value, Vector128<int> count)
	{
		throw null;
	}

	public static Vector128<long> ShiftLeftLogical(Vector128<long> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector128<long> ShiftLeftLogical(Vector128<long> value, Vector128<long> count)
	{
		throw null;
	}

	public static Vector128<ushort> ShiftLeftLogical(Vector128<ushort> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector128<ushort> ShiftLeftLogical(Vector128<ushort> value, Vector128<ushort> count)
	{
		throw null;
	}

	public static Vector128<uint> ShiftLeftLogical(Vector128<uint> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector128<uint> ShiftLeftLogical(Vector128<uint> value, Vector128<uint> count)
	{
		throw null;
	}

	public static Vector128<ulong> ShiftLeftLogical(Vector128<ulong> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector128<ulong> ShiftLeftLogical(Vector128<ulong> value, Vector128<ulong> count)
	{
		throw null;
	}

	public static Vector128<byte> ShiftLeftLogical128BitLane(Vector128<byte> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector128<short> ShiftLeftLogical128BitLane(Vector128<short> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector128<int> ShiftLeftLogical128BitLane(Vector128<int> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector128<long> ShiftLeftLogical128BitLane(Vector128<long> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector128<sbyte> ShiftLeftLogical128BitLane(Vector128<sbyte> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector128<ushort> ShiftLeftLogical128BitLane(Vector128<ushort> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector128<uint> ShiftLeftLogical128BitLane(Vector128<uint> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector128<ulong> ShiftLeftLogical128BitLane(Vector128<ulong> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector128<short> ShiftRightArithmetic(Vector128<short> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector128<short> ShiftRightArithmetic(Vector128<short> value, Vector128<short> count)
	{
		throw null;
	}

	public static Vector128<int> ShiftRightArithmetic(Vector128<int> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector128<int> ShiftRightArithmetic(Vector128<int> value, Vector128<int> count)
	{
		throw null;
	}

	public static Vector128<short> ShiftRightLogical(Vector128<short> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector128<short> ShiftRightLogical(Vector128<short> value, Vector128<short> count)
	{
		throw null;
	}

	public static Vector128<int> ShiftRightLogical(Vector128<int> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector128<int> ShiftRightLogical(Vector128<int> value, Vector128<int> count)
	{
		throw null;
	}

	public static Vector128<long> ShiftRightLogical(Vector128<long> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector128<long> ShiftRightLogical(Vector128<long> value, Vector128<long> count)
	{
		throw null;
	}

	public static Vector128<ushort> ShiftRightLogical(Vector128<ushort> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector128<ushort> ShiftRightLogical(Vector128<ushort> value, Vector128<ushort> count)
	{
		throw null;
	}

	public static Vector128<uint> ShiftRightLogical(Vector128<uint> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector128<uint> ShiftRightLogical(Vector128<uint> value, Vector128<uint> count)
	{
		throw null;
	}

	public static Vector128<ulong> ShiftRightLogical(Vector128<ulong> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector128<ulong> ShiftRightLogical(Vector128<ulong> value, Vector128<ulong> count)
	{
		throw null;
	}

	public static Vector128<byte> ShiftRightLogical128BitLane(Vector128<byte> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector128<short> ShiftRightLogical128BitLane(Vector128<short> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector128<int> ShiftRightLogical128BitLane(Vector128<int> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector128<long> ShiftRightLogical128BitLane(Vector128<long> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector128<sbyte> ShiftRightLogical128BitLane(Vector128<sbyte> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector128<ushort> ShiftRightLogical128BitLane(Vector128<ushort> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector128<uint> ShiftRightLogical128BitLane(Vector128<uint> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector128<ulong> ShiftRightLogical128BitLane(Vector128<ulong> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector128<double> Shuffle(Vector128<double> left, Vector128<double> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<int> Shuffle(Vector128<int> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<uint> Shuffle(Vector128<uint> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<short> ShuffleHigh(Vector128<short> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<ushort> ShuffleHigh(Vector128<ushort> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<short> ShuffleLow(Vector128<short> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<ushort> ShuffleLow(Vector128<ushort> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<double> Sqrt(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<double> SqrtScalar(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<double> SqrtScalar(Vector128<double> upper, Vector128<double> value)
	{
		throw null;
	}

	public unsafe static void Store(byte* address, Vector128<byte> source)
	{
	}

	public unsafe static void Store(double* address, Vector128<double> source)
	{
	}

	public unsafe static void Store(short* address, Vector128<short> source)
	{
	}

	public unsafe static void Store(int* address, Vector128<int> source)
	{
	}

	public unsafe static void Store(long* address, Vector128<long> source)
	{
	}

	public unsafe static void Store(sbyte* address, Vector128<sbyte> source)
	{
	}

	public unsafe static void Store(ushort* address, Vector128<ushort> source)
	{
	}

	public unsafe static void Store(uint* address, Vector128<uint> source)
	{
	}

	public unsafe static void Store(ulong* address, Vector128<ulong> source)
	{
	}

	public unsafe static void StoreAligned(byte* address, Vector128<byte> source)
	{
	}

	public unsafe static void StoreAligned(double* address, Vector128<double> source)
	{
	}

	public unsafe static void StoreAligned(short* address, Vector128<short> source)
	{
	}

	public unsafe static void StoreAligned(int* address, Vector128<int> source)
	{
	}

	public unsafe static void StoreAligned(long* address, Vector128<long> source)
	{
	}

	public unsafe static void StoreAligned(sbyte* address, Vector128<sbyte> source)
	{
	}

	public unsafe static void StoreAligned(ushort* address, Vector128<ushort> source)
	{
	}

	public unsafe static void StoreAligned(uint* address, Vector128<uint> source)
	{
	}

	public unsafe static void StoreAligned(ulong* address, Vector128<ulong> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(byte* address, Vector128<byte> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(double* address, Vector128<double> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(short* address, Vector128<short> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(int* address, Vector128<int> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(long* address, Vector128<long> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(sbyte* address, Vector128<sbyte> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(ushort* address, Vector128<ushort> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(uint* address, Vector128<uint> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(ulong* address, Vector128<ulong> source)
	{
	}

	public unsafe static void StoreHigh(double* address, Vector128<double> source)
	{
	}

	public unsafe static void StoreLow(double* address, Vector128<double> source)
	{
	}

	public unsafe static void StoreNonTemporal(int* address, int value)
	{
	}

	public unsafe static void StoreNonTemporal(uint* address, uint value)
	{
	}

	public unsafe static void StoreScalar(double* address, Vector128<double> source)
	{
	}

	public unsafe static void StoreScalar(int* address, Vector128<int> source)
	{
	}

	public unsafe static void StoreScalar(long* address, Vector128<long> source)
	{
	}

	public unsafe static void StoreScalar(uint* address, Vector128<uint> source)
	{
	}

	public unsafe static void StoreScalar(ulong* address, Vector128<ulong> source)
	{
	}

	public static Vector128<byte> Subtract(Vector128<byte> left, Vector128<byte> right)
	{
		throw null;
	}

	public static Vector128<double> Subtract(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<short> Subtract(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static Vector128<int> Subtract(Vector128<int> left, Vector128<int> right)
	{
		throw null;
	}

	public static Vector128<long> Subtract(Vector128<long> left, Vector128<long> right)
	{
		throw null;
	}

	public static Vector128<sbyte> Subtract(Vector128<sbyte> left, Vector128<sbyte> right)
	{
		throw null;
	}

	public static Vector128<ushort> Subtract(Vector128<ushort> left, Vector128<ushort> right)
	{
		throw null;
	}

	public static Vector128<uint> Subtract(Vector128<uint> left, Vector128<uint> right)
	{
		throw null;
	}

	public static Vector128<ulong> Subtract(Vector128<ulong> left, Vector128<ulong> right)
	{
		throw null;
	}

	public static Vector128<byte> SubtractSaturate(Vector128<byte> left, Vector128<byte> right)
	{
		throw null;
	}

	public static Vector128<short> SubtractSaturate(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static Vector128<sbyte> SubtractSaturate(Vector128<sbyte> left, Vector128<sbyte> right)
	{
		throw null;
	}

	public static Vector128<ushort> SubtractSaturate(Vector128<ushort> left, Vector128<ushort> right)
	{
		throw null;
	}

	public static Vector128<double> SubtractScalar(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<ushort> SumAbsoluteDifferences(Vector128<byte> left, Vector128<byte> right)
	{
		throw null;
	}

	public static Vector128<byte> UnpackHigh(Vector128<byte> left, Vector128<byte> right)
	{
		throw null;
	}

	public static Vector128<double> UnpackHigh(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<short> UnpackHigh(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static Vector128<int> UnpackHigh(Vector128<int> left, Vector128<int> right)
	{
		throw null;
	}

	public static Vector128<long> UnpackHigh(Vector128<long> left, Vector128<long> right)
	{
		throw null;
	}

	public static Vector128<sbyte> UnpackHigh(Vector128<sbyte> left, Vector128<sbyte> right)
	{
		throw null;
	}

	public static Vector128<ushort> UnpackHigh(Vector128<ushort> left, Vector128<ushort> right)
	{
		throw null;
	}

	public static Vector128<uint> UnpackHigh(Vector128<uint> left, Vector128<uint> right)
	{
		throw null;
	}

	public static Vector128<ulong> UnpackHigh(Vector128<ulong> left, Vector128<ulong> right)
	{
		throw null;
	}

	public static Vector128<byte> UnpackLow(Vector128<byte> left, Vector128<byte> right)
	{
		throw null;
	}

	public static Vector128<double> UnpackLow(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<short> UnpackLow(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static Vector128<int> UnpackLow(Vector128<int> left, Vector128<int> right)
	{
		throw null;
	}

	public static Vector128<long> UnpackLow(Vector128<long> left, Vector128<long> right)
	{
		throw null;
	}

	public static Vector128<sbyte> UnpackLow(Vector128<sbyte> left, Vector128<sbyte> right)
	{
		throw null;
	}

	public static Vector128<ushort> UnpackLow(Vector128<ushort> left, Vector128<ushort> right)
	{
		throw null;
	}

	public static Vector128<uint> UnpackLow(Vector128<uint> left, Vector128<uint> right)
	{
		throw null;
	}

	public static Vector128<ulong> UnpackLow(Vector128<ulong> left, Vector128<ulong> right)
	{
		throw null;
	}

	public static Vector128<byte> Xor(Vector128<byte> left, Vector128<byte> right)
	{
		throw null;
	}

	public static Vector128<double> Xor(Vector128<double> left, Vector128<double> right)
	{
		throw null;
	}

	public static Vector128<short> Xor(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static Vector128<int> Xor(Vector128<int> left, Vector128<int> right)
	{
		throw null;
	}

	public static Vector128<long> Xor(Vector128<long> left, Vector128<long> right)
	{
		throw null;
	}

	public static Vector128<sbyte> Xor(Vector128<sbyte> left, Vector128<sbyte> right)
	{
		throw null;
	}

	public static Vector128<ushort> Xor(Vector128<ushort> left, Vector128<ushort> right)
	{
		throw null;
	}

	public static Vector128<uint> Xor(Vector128<uint> left, Vector128<uint> right)
	{
		throw null;
	}

	public static Vector128<ulong> Xor(Vector128<ulong> left, Vector128<ulong> right)
	{
		throw null;
	}
}
