using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Intrinsics.Arm;

[CLSCompliant(false)]
public abstract class Dp : AdvSimd
{
	public new abstract class Arm64 : AdvSimd.Arm64
	{
		public new static bool IsSupported
		{
			get
			{
				throw null;
			}
		}

		internal Arm64()
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

	internal Dp()
	{
	}

	public static Vector128<int> DotProduct(Vector128<int> addend, Vector128<sbyte> left, Vector128<sbyte> right)
	{
		throw null;
	}

	public static Vector128<uint> DotProduct(Vector128<uint> addend, Vector128<byte> left, Vector128<byte> right)
	{
		throw null;
	}

	public static Vector64<int> DotProduct(Vector64<int> addend, Vector64<sbyte> left, Vector64<sbyte> right)
	{
		throw null;
	}

	public static Vector64<uint> DotProduct(Vector64<uint> addend, Vector64<byte> left, Vector64<byte> right)
	{
		throw null;
	}

	public static Vector128<int> DotProductBySelectedQuadruplet(Vector128<int> addend, Vector128<sbyte> left, Vector128<sbyte> right, [ConstantExpected(Max = 15)] byte rightScaledIndex)
	{
		throw null;
	}

	public static Vector128<int> DotProductBySelectedQuadruplet(Vector128<int> addend, Vector128<sbyte> left, Vector64<sbyte> right, [ConstantExpected(Max = 7)] byte rightScaledIndex)
	{
		throw null;
	}

	public static Vector128<uint> DotProductBySelectedQuadruplet(Vector128<uint> addend, Vector128<byte> left, Vector128<byte> right, [ConstantExpected(Max = 15)] byte rightScaledIndex)
	{
		throw null;
	}

	public static Vector128<uint> DotProductBySelectedQuadruplet(Vector128<uint> addend, Vector128<byte> left, Vector64<byte> right, [ConstantExpected(Max = 7)] byte rightScaledIndex)
	{
		throw null;
	}

	public static Vector64<int> DotProductBySelectedQuadruplet(Vector64<int> addend, Vector64<sbyte> left, Vector128<sbyte> right, [ConstantExpected(Max = 15)] byte rightScaledIndex)
	{
		throw null;
	}

	public static Vector64<int> DotProductBySelectedQuadruplet(Vector64<int> addend, Vector64<sbyte> left, Vector64<sbyte> right, [ConstantExpected(Max = 7)] byte rightScaledIndex)
	{
		throw null;
	}

	public static Vector64<uint> DotProductBySelectedQuadruplet(Vector64<uint> addend, Vector64<byte> left, Vector128<byte> right, [ConstantExpected(Max = 15)] byte rightScaledIndex)
	{
		throw null;
	}

	public static Vector64<uint> DotProductBySelectedQuadruplet(Vector64<uint> addend, Vector64<byte> left, Vector64<byte> right, [ConstantExpected(Max = 7)] byte rightScaledIndex)
	{
		throw null;
	}
}
