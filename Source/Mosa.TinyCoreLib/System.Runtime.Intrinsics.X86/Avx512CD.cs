namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class Avx512CD : Avx512F
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

		public static Vector128<int> DetectConflicts(Vector128<int> value)
		{
			throw null;
		}

		public static Vector128<long> DetectConflicts(Vector128<long> value)
		{
			throw null;
		}

		public static Vector128<uint> DetectConflicts(Vector128<uint> value)
		{
			throw null;
		}

		public static Vector128<ulong> DetectConflicts(Vector128<ulong> value)
		{
			throw null;
		}

		public static Vector256<int> DetectConflicts(Vector256<int> value)
		{
			throw null;
		}

		public static Vector256<long> DetectConflicts(Vector256<long> value)
		{
			throw null;
		}

		public static Vector256<uint> DetectConflicts(Vector256<uint> value)
		{
			throw null;
		}

		public static Vector256<ulong> DetectConflicts(Vector256<ulong> value)
		{
			throw null;
		}

		public static Vector128<int> LeadingZeroCount(Vector128<int> value)
		{
			throw null;
		}

		public static Vector128<long> LeadingZeroCount(Vector128<long> value)
		{
			throw null;
		}

		public static Vector128<uint> LeadingZeroCount(Vector128<uint> value)
		{
			throw null;
		}

		public static Vector128<ulong> LeadingZeroCount(Vector128<ulong> value)
		{
			throw null;
		}

		public static Vector256<int> LeadingZeroCount(Vector256<int> value)
		{
			throw null;
		}

		public static Vector256<long> LeadingZeroCount(Vector256<long> value)
		{
			throw null;
		}

		public static Vector256<uint> LeadingZeroCount(Vector256<uint> value)
		{
			throw null;
		}

		public static Vector256<ulong> LeadingZeroCount(Vector256<ulong> value)
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

	internal Avx512CD()
	{
	}

	public static Vector512<int> DetectConflicts(Vector512<int> value)
	{
		throw null;
	}

	public static Vector512<long> DetectConflicts(Vector512<long> value)
	{
		throw null;
	}

	public static Vector512<uint> DetectConflicts(Vector512<uint> value)
	{
		throw null;
	}

	public static Vector512<ulong> DetectConflicts(Vector512<ulong> value)
	{
		throw null;
	}

	public static Vector512<int> LeadingZeroCount(Vector512<int> value)
	{
		throw null;
	}

	public static Vector512<long> LeadingZeroCount(Vector512<long> value)
	{
		throw null;
	}

	public static Vector512<uint> LeadingZeroCount(Vector512<uint> value)
	{
		throw null;
	}

	public static Vector512<ulong> LeadingZeroCount(Vector512<ulong> value)
	{
		throw null;
	}
}
