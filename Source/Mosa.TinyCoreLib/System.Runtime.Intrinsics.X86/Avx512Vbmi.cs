namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class Avx512Vbmi : Avx512BW
{
	public new abstract class VL : Avx512BW.VL
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

		public static Vector128<byte> PermuteVar16x8(Vector128<byte> left, Vector128<byte> control)
		{
			throw null;
		}

		public static Vector128<sbyte> PermuteVar16x8(Vector128<sbyte> left, Vector128<sbyte> control)
		{
			throw null;
		}

		public static Vector128<byte> PermuteVar16x8x2(Vector128<byte> lower, Vector128<byte> indices, Vector128<byte> upper)
		{
			throw null;
		}

		public static Vector128<sbyte> PermuteVar16x8x2(Vector128<sbyte> lower, Vector128<sbyte> indices, Vector128<sbyte> upper)
		{
			throw null;
		}

		public static Vector256<byte> PermuteVar32x8(Vector256<byte> left, Vector256<byte> control)
		{
			throw null;
		}

		public static Vector256<sbyte> PermuteVar32x8(Vector256<sbyte> left, Vector256<sbyte> control)
		{
			throw null;
		}

		public static Vector256<byte> PermuteVar32x8x2(Vector256<byte> lower, Vector256<byte> indices, Vector256<byte> upper)
		{
			throw null;
		}

		public static Vector256<sbyte> PermuteVar32x8x2(Vector256<sbyte> lower, Vector256<sbyte> indices, Vector256<sbyte> upper)
		{
			throw null;
		}
	}

	public new abstract class X64 : Avx512BW.X64
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

	internal Avx512Vbmi()
	{
	}

	public static Vector512<byte> PermuteVar64x8(Vector512<byte> left, Vector512<byte> control)
	{
		throw null;
	}

	public static Vector512<sbyte> PermuteVar64x8(Vector512<sbyte> left, Vector512<sbyte> control)
	{
		throw null;
	}

	public static Vector512<byte> PermuteVar64x8x2(Vector512<byte> lower, Vector512<byte> indices, Vector512<byte> upper)
	{
		throw null;
	}

	public static Vector512<sbyte> PermuteVar64x8x2(Vector512<sbyte> lower, Vector512<sbyte> indices, Vector512<sbyte> upper)
	{
		throw null;
	}
}
