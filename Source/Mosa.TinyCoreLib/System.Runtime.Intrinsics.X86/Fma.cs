namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class Fma : Avx
{
	public new abstract class X64 : Avx.X64
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

	internal Fma()
	{
	}

	public static Vector128<double> MultiplyAdd(Vector128<double> a, Vector128<double> b, Vector128<double> c)
	{
		throw null;
	}

	public static Vector128<float> MultiplyAdd(Vector128<float> a, Vector128<float> b, Vector128<float> c)
	{
		throw null;
	}

	public static Vector256<double> MultiplyAdd(Vector256<double> a, Vector256<double> b, Vector256<double> c)
	{
		throw null;
	}

	public static Vector256<float> MultiplyAdd(Vector256<float> a, Vector256<float> b, Vector256<float> c)
	{
		throw null;
	}

	public static Vector128<double> MultiplyAddNegated(Vector128<double> a, Vector128<double> b, Vector128<double> c)
	{
		throw null;
	}

	public static Vector128<float> MultiplyAddNegated(Vector128<float> a, Vector128<float> b, Vector128<float> c)
	{
		throw null;
	}

	public static Vector256<double> MultiplyAddNegated(Vector256<double> a, Vector256<double> b, Vector256<double> c)
	{
		throw null;
	}

	public static Vector256<float> MultiplyAddNegated(Vector256<float> a, Vector256<float> b, Vector256<float> c)
	{
		throw null;
	}

	public static Vector128<double> MultiplyAddNegatedScalar(Vector128<double> a, Vector128<double> b, Vector128<double> c)
	{
		throw null;
	}

	public static Vector128<float> MultiplyAddNegatedScalar(Vector128<float> a, Vector128<float> b, Vector128<float> c)
	{
		throw null;
	}

	public static Vector128<double> MultiplyAddScalar(Vector128<double> a, Vector128<double> b, Vector128<double> c)
	{
		throw null;
	}

	public static Vector128<float> MultiplyAddScalar(Vector128<float> a, Vector128<float> b, Vector128<float> c)
	{
		throw null;
	}

	public static Vector128<double> MultiplyAddSubtract(Vector128<double> a, Vector128<double> b, Vector128<double> c)
	{
		throw null;
	}

	public static Vector128<float> MultiplyAddSubtract(Vector128<float> a, Vector128<float> b, Vector128<float> c)
	{
		throw null;
	}

	public static Vector256<double> MultiplyAddSubtract(Vector256<double> a, Vector256<double> b, Vector256<double> c)
	{
		throw null;
	}

	public static Vector256<float> MultiplyAddSubtract(Vector256<float> a, Vector256<float> b, Vector256<float> c)
	{
		throw null;
	}

	public static Vector128<double> MultiplySubtract(Vector128<double> a, Vector128<double> b, Vector128<double> c)
	{
		throw null;
	}

	public static Vector128<float> MultiplySubtract(Vector128<float> a, Vector128<float> b, Vector128<float> c)
	{
		throw null;
	}

	public static Vector256<double> MultiplySubtract(Vector256<double> a, Vector256<double> b, Vector256<double> c)
	{
		throw null;
	}

	public static Vector256<float> MultiplySubtract(Vector256<float> a, Vector256<float> b, Vector256<float> c)
	{
		throw null;
	}

	public static Vector128<double> MultiplySubtractAdd(Vector128<double> a, Vector128<double> b, Vector128<double> c)
	{
		throw null;
	}

	public static Vector128<float> MultiplySubtractAdd(Vector128<float> a, Vector128<float> b, Vector128<float> c)
	{
		throw null;
	}

	public static Vector256<double> MultiplySubtractAdd(Vector256<double> a, Vector256<double> b, Vector256<double> c)
	{
		throw null;
	}

	public static Vector256<float> MultiplySubtractAdd(Vector256<float> a, Vector256<float> b, Vector256<float> c)
	{
		throw null;
	}

	public static Vector128<double> MultiplySubtractNegated(Vector128<double> a, Vector128<double> b, Vector128<double> c)
	{
		throw null;
	}

	public static Vector128<float> MultiplySubtractNegated(Vector128<float> a, Vector128<float> b, Vector128<float> c)
	{
		throw null;
	}

	public static Vector256<double> MultiplySubtractNegated(Vector256<double> a, Vector256<double> b, Vector256<double> c)
	{
		throw null;
	}

	public static Vector256<float> MultiplySubtractNegated(Vector256<float> a, Vector256<float> b, Vector256<float> c)
	{
		throw null;
	}

	public static Vector128<double> MultiplySubtractNegatedScalar(Vector128<double> a, Vector128<double> b, Vector128<double> c)
	{
		throw null;
	}

	public static Vector128<float> MultiplySubtractNegatedScalar(Vector128<float> a, Vector128<float> b, Vector128<float> c)
	{
		throw null;
	}

	public static Vector128<double> MultiplySubtractScalar(Vector128<double> a, Vector128<double> b, Vector128<double> c)
	{
		throw null;
	}

	public static Vector128<float> MultiplySubtractScalar(Vector128<float> a, Vector128<float> b, Vector128<float> c)
	{
		throw null;
	}
}
