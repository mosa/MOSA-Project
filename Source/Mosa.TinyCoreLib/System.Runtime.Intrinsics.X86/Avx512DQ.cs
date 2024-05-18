using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class Avx512DQ : Avx512F
{
	public new abstract class VL : Avx512F.VL
	{
		public new static bool IsSupported
		{
			get
			{
				throw null;
			}
		}

		internal VL()
		{
		}

		public static Vector128<int> BroadcastPairScalarToVector128(Vector128<int> value)
		{
			throw null;
		}

		public static Vector128<uint> BroadcastPairScalarToVector128(Vector128<uint> value)
		{
			throw null;
		}

		public static Vector256<int> BroadcastPairScalarToVector256(Vector128<int> value)
		{
			throw null;
		}

		public static Vector256<float> BroadcastPairScalarToVector256(Vector128<float> value)
		{
			throw null;
		}

		public static Vector256<uint> BroadcastPairScalarToVector256(Vector128<uint> value)
		{
			throw null;
		}

		public static Vector128<double> ConvertToVector128Double(Vector128<long> value)
		{
			throw null;
		}

		public static Vector128<double> ConvertToVector128Double(Vector128<ulong> value)
		{
			throw null;
		}

		public static Vector128<long> ConvertToVector128Int64(Vector128<double> value)
		{
			throw null;
		}

		public static Vector128<long> ConvertToVector128Int64(Vector128<float> value)
		{
			throw null;
		}

		public static Vector128<long> ConvertToVector128Int64WithTruncation(Vector128<double> value)
		{
			throw null;
		}

		public static Vector128<long> ConvertToVector128Int64WithTruncation(Vector128<float> value)
		{
			throw null;
		}

		public static Vector128<float> ConvertToVector128Single(Vector128<long> value)
		{
			throw null;
		}

		public static Vector128<float> ConvertToVector128Single(Vector256<long> value)
		{
			throw null;
		}

		public static Vector128<float> ConvertToVector128Single(Vector128<ulong> value)
		{
			throw null;
		}

		public static Vector128<float> ConvertToVector128Single(Vector256<ulong> value)
		{
			throw null;
		}

		public static Vector128<ulong> ConvertToVector128UInt64(Vector128<double> value)
		{
			throw null;
		}

		public static Vector128<ulong> ConvertToVector128UInt64(Vector128<float> value)
		{
			throw null;
		}

		public static Vector128<ulong> ConvertToVector128UInt64WithTruncation(Vector128<double> value)
		{
			throw null;
		}

		public static Vector128<ulong> ConvertToVector128UInt64WithTruncation(Vector128<float> value)
		{
			throw null;
		}

		public static Vector256<double> ConvertToVector256Double(Vector256<long> value)
		{
			throw null;
		}

		public static Vector256<double> ConvertToVector256Double(Vector256<ulong> value)
		{
			throw null;
		}

		public static Vector256<long> ConvertToVector256Int64(Vector256<double> value)
		{
			throw null;
		}

		public static Vector256<long> ConvertToVector256Int64(Vector128<float> value)
		{
			throw null;
		}

		public static Vector256<long> ConvertToVector256Int64WithTruncation(Vector256<double> value)
		{
			throw null;
		}

		public static Vector256<long> ConvertToVector256Int64WithTruncation(Vector128<float> value)
		{
			throw null;
		}

		public static Vector256<ulong> ConvertToVector256UInt64(Vector256<double> value)
		{
			throw null;
		}

		public static Vector256<ulong> ConvertToVector256UInt64(Vector128<float> value)
		{
			throw null;
		}

		public static Vector256<ulong> ConvertToVector256UInt64WithTruncation(Vector256<double> value)
		{
			throw null;
		}

		public static Vector256<ulong> ConvertToVector256UInt64WithTruncation(Vector128<float> value)
		{
			throw null;
		}

		public static Vector128<long> MultiplyLow(Vector128<long> left, Vector128<long> right)
		{
			throw null;
		}

		public static Vector128<ulong> MultiplyLow(Vector128<ulong> left, Vector128<ulong> right)
		{
			throw null;
		}

		public static Vector256<long> MultiplyLow(Vector256<long> left, Vector256<long> right)
		{
			throw null;
		}

		public static Vector256<ulong> MultiplyLow(Vector256<ulong> left, Vector256<ulong> right)
		{
			throw null;
		}

		public static Vector128<double> Range(Vector128<double> left, Vector128<double> right, [ConstantExpected(Max = 15)] byte control)
		{
			throw null;
		}

		public static Vector128<float> Range(Vector128<float> left, Vector128<float> right, [ConstantExpected(Max = 15)] byte control)
		{
			throw null;
		}

		public static Vector256<double> Range(Vector256<double> left, Vector256<double> right, [ConstantExpected(Max = 15)] byte control)
		{
			throw null;
		}

		public static Vector256<float> Range(Vector256<float> left, Vector256<float> right, [ConstantExpected(Max = 15)] byte control)
		{
			throw null;
		}

		public static Vector128<double> Reduce(Vector128<double> value, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector128<float> Reduce(Vector128<float> value, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<double> Reduce(Vector256<double> value, [ConstantExpected] byte control)
		{
			throw null;
		}

		public static Vector256<float> Reduce(Vector256<float> value, [ConstantExpected] byte control)
		{
			throw null;
		}
	}

	public new abstract class X64 : Avx512F.X64
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

	internal Avx512DQ()
	{
	}

	public static Vector512<double> And(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<float> And(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<double> AndNot(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<float> AndNot(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<int> BroadcastPairScalarToVector512(Vector128<int> value)
	{
		throw null;
	}

	public static Vector512<uint> BroadcastPairScalarToVector512(Vector128<uint> value)
	{
		throw null;
	}

	public static Vector512<float> BroadcastPairScalarToVector512(Vector128<float> value)
	{
		throw null;
	}

	public unsafe static Vector512<double> BroadcastVector128ToVector512(double* address)
	{
		throw null;
	}

	public unsafe static Vector512<long> BroadcastVector128ToVector512(long* address)
	{
		throw null;
	}

	public unsafe static Vector512<ulong> BroadcastVector128ToVector512(ulong* address)
	{
		throw null;
	}

	public unsafe static Vector512<int> BroadcastVector256ToVector512(int* address)
	{
		throw null;
	}

	public unsafe static Vector512<float> BroadcastVector256ToVector512(float* address)
	{
		throw null;
	}

	public unsafe static Vector512<uint> BroadcastVector256ToVector512(uint* address)
	{
		throw null;
	}

	public static Vector256<float> ConvertToVector256Single(Vector512<long> value)
	{
		throw null;
	}

	public static Vector256<float> ConvertToVector256Single(Vector512<ulong> value)
	{
		throw null;
	}

	public static Vector512<double> ConvertToVector512Double(Vector512<long> value)
	{
		throw null;
	}

	public static Vector512<double> ConvertToVector512Double(Vector512<ulong> value)
	{
		throw null;
	}

	public static Vector512<long> ConvertToVector512Int64(Vector512<double> value)
	{
		throw null;
	}

	public static Vector512<long> ConvertToVector512Int64(Vector256<float> value)
	{
		throw null;
	}

	public static Vector512<long> ConvertToVector512Int64WithTruncation(Vector512<double> value)
	{
		throw null;
	}

	public static Vector512<long> ConvertToVector512Int64WithTruncation(Vector256<float> value)
	{
		throw null;
	}

	public static Vector512<ulong> ConvertToVector512UInt64(Vector512<double> value)
	{
		throw null;
	}

	public static Vector512<ulong> ConvertToVector512UInt64(Vector256<float> value)
	{
		throw null;
	}

	public static Vector512<ulong> ConvertToVector512UInt64WithTruncation(Vector512<double> value)
	{
		throw null;
	}

	public static Vector512<ulong> ConvertToVector512UInt64WithTruncation(Vector256<float> value)
	{
		throw null;
	}

	public new static Vector128<double> ExtractVector128(Vector512<double> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector128<long> ExtractVector128(Vector512<long> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector128<ulong> ExtractVector128(Vector512<ulong> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector256<int> ExtractVector256(Vector512<int> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector256<float> ExtractVector256(Vector512<float> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector256<uint> ExtractVector256(Vector512<uint> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector512<double> InsertVector128(Vector512<double> value, Vector128<double> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector512<long> InsertVector128(Vector512<long> value, Vector128<long> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector512<ulong> InsertVector128(Vector512<ulong> value, Vector128<ulong> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector512<int> InsertVector256(Vector512<int> value, Vector256<int> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector512<float> InsertVector256(Vector512<float> value, Vector256<float> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector512<uint> InsertVector256(Vector512<uint> value, Vector256<uint> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector512<long> MultiplyLow(Vector512<long> left, Vector512<long> right)
	{
		throw null;
	}

	public static Vector512<ulong> MultiplyLow(Vector512<ulong> left, Vector512<ulong> right)
	{
		throw null;
	}

	public static Vector512<double> Or(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<float> Or(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}

	public static Vector512<double> Range(Vector512<double> left, Vector512<double> right, [ConstantExpected(Max = 15)] byte control)
	{
		throw null;
	}

	public static Vector512<float> Range(Vector512<float> left, Vector512<float> right, [ConstantExpected(Max = 15)] byte control)
	{
		throw null;
	}

	public static Vector128<double> RangeScalar(Vector128<double> left, Vector128<double> right, [ConstantExpected(Max = 15)] byte control)
	{
		throw null;
	}

	public static Vector128<float> RangeScalar(Vector128<float> left, Vector128<float> right, [ConstantExpected(Max = 15)] byte control)
	{
		throw null;
	}

	public static Vector512<double> Reduce(Vector512<double> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<float> Reduce(Vector512<float> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<double> ReduceScalar(Vector128<double> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<float> ReduceScalar(Vector128<float> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<double> ReduceScalar(Vector128<double> upper, Vector128<double> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<float> ReduceScalar(Vector128<float> upper, Vector128<float> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector512<double> Xor(Vector512<double> left, Vector512<double> right)
	{
		throw null;
	}

	public static Vector512<float> Xor(Vector512<float> left, Vector512<float> right)
	{
		throw null;
	}
}
