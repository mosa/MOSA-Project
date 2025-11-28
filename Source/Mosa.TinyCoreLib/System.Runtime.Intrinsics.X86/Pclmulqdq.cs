using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class Pclmulqdq : Sse2
{
	public new abstract class X64 : Sse2.X64
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

	internal Pclmulqdq()
	{
	}

	public static Vector128<long> CarrylessMultiply(Vector128<long> left, Vector128<long> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<ulong> CarrylessMultiply(Vector128<ulong> left, Vector128<ulong> right, [ConstantExpected] byte control)
	{
		throw null;
	}
}
