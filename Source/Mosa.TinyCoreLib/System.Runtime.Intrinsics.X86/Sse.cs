using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class Sse : X86Base
{
	public new abstract class X64 : X86Base.X64
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

		public static Vector128<float> ConvertScalarToVector128Single(Vector128<float> upper, long value)
		{
			throw null;
		}

		public static long ConvertToInt64(Vector128<float> value)
		{
			throw null;
		}

		public static long ConvertToInt64WithTruncation(Vector128<float> value)
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

	internal Sse()
	{
	}

	public static Vector128<float> Add(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> AddScalar(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> And(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> AndNot(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareEqual(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareGreaterThan(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareGreaterThanOrEqual(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareLessThan(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareLessThanOrEqual(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareNotEqual(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareNotGreaterThan(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareNotGreaterThanOrEqual(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareNotLessThan(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareNotLessThanOrEqual(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareOrdered(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareScalarEqual(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareScalarGreaterThan(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareScalarGreaterThanOrEqual(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareScalarLessThan(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareScalarLessThanOrEqual(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareScalarNotEqual(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareScalarNotGreaterThan(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareScalarNotGreaterThanOrEqual(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareScalarNotLessThan(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareScalarNotLessThanOrEqual(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareScalarOrdered(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static bool CompareScalarOrderedEqual(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static bool CompareScalarOrderedGreaterThan(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static bool CompareScalarOrderedGreaterThanOrEqual(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static bool CompareScalarOrderedLessThan(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static bool CompareScalarOrderedLessThanOrEqual(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static bool CompareScalarOrderedNotEqual(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareScalarUnordered(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static bool CompareScalarUnorderedEqual(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static bool CompareScalarUnorderedGreaterThan(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static bool CompareScalarUnorderedGreaterThanOrEqual(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static bool CompareScalarUnorderedLessThan(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static bool CompareScalarUnorderedLessThanOrEqual(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static bool CompareScalarUnorderedNotEqual(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> CompareUnordered(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> ConvertScalarToVector128Single(Vector128<float> upper, int value)
	{
		throw null;
	}

	public static int ConvertToInt32(Vector128<float> value)
	{
		throw null;
	}

	public static int ConvertToInt32WithTruncation(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<float> Divide(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> DivideScalar(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public unsafe static Vector128<float> LoadAlignedVector128(float* address)
	{
		throw null;
	}

	public unsafe static Vector128<float> LoadHigh(Vector128<float> lower, float* address)
	{
		throw null;
	}

	public unsafe static Vector128<float> LoadLow(Vector128<float> upper, float* address)
	{
		throw null;
	}

	public unsafe static Vector128<float> LoadScalarVector128(float* address)
	{
		throw null;
	}

	public unsafe static Vector128<float> LoadVector128(float* address)
	{
		throw null;
	}

	public static Vector128<float> Max(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> MaxScalar(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> Min(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> MinScalar(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> MoveHighToLow(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> MoveLowToHigh(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static int MoveMask(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<float> MoveScalar(Vector128<float> upper, Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<float> Multiply(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> MultiplyScalar(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> Or(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public unsafe static void Prefetch0(void* address)
	{
	}

	public unsafe static void Prefetch1(void* address)
	{
	}

	public unsafe static void Prefetch2(void* address)
	{
	}

	public unsafe static void PrefetchNonTemporal(void* address)
	{
	}

	public static Vector128<float> Reciprocal(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<float> ReciprocalScalar(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<float> ReciprocalScalar(Vector128<float> upper, Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<float> ReciprocalSqrt(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<float> ReciprocalSqrtScalar(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<float> ReciprocalSqrtScalar(Vector128<float> upper, Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<float> Shuffle(Vector128<float> left, Vector128<float> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<float> Sqrt(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<float> SqrtScalar(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<float> SqrtScalar(Vector128<float> upper, Vector128<float> value)
	{
		throw null;
	}

	public unsafe static void Store(float* address, Vector128<float> source)
	{
	}

	public unsafe static void StoreAligned(float* address, Vector128<float> source)
	{
	}

	public unsafe static void StoreAlignedNonTemporal(float* address, Vector128<float> source)
	{
	}

	public static void StoreFence()
	{
	}

	public unsafe static void StoreHigh(float* address, Vector128<float> source)
	{
	}

	public unsafe static void StoreLow(float* address, Vector128<float> source)
	{
	}

	public unsafe static void StoreScalar(float* address, Vector128<float> source)
	{
	}

	public static Vector128<float> Subtract(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> SubtractScalar(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> UnpackHigh(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> UnpackLow(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}

	public static Vector128<float> Xor(Vector128<float> left, Vector128<float> right)
	{
		throw null;
	}
}
