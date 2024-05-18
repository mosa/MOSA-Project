using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class Sse41 : Ssse3
{
	public new abstract class X64 : Ssse3.X64
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

		public static long Extract(Vector128<long> value, [ConstantExpected] byte index)
		{
			throw null;
		}

		public static ulong Extract(Vector128<ulong> value, [ConstantExpected] byte index)
		{
			throw null;
		}

		public static Vector128<long> Insert(Vector128<long> value, long data, [ConstantExpected] byte index)
		{
			throw null;
		}

		public static Vector128<ulong> Insert(Vector128<ulong> value, ulong data, [ConstantExpected] byte index)
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

	internal Sse41()
	{
	}

	public static Vector128<double> Blend(Vector128<double> left, Vector128<double> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<short> Blend(Vector128<short> left, Vector128<short> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<float> Blend(Vector128<float> left, Vector128<float> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<ushort> Blend(Vector128<ushort> left, Vector128<ushort> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<byte> BlendVariable(Vector128<byte> left, Vector128<byte> right, Vector128<byte> mask)
	{
		throw null;
	}

	public static Vector128<double> BlendVariable(Vector128<double> left, Vector128<double> right, Vector128<double> mask)
	{
		throw null;
	}

	public static Vector128<short> BlendVariable(Vector128<short> left, Vector128<short> right, Vector128<short> mask)
	{
		throw null;
	}

	public static Vector128<int> BlendVariable(Vector128<int> left, Vector128<int> right, Vector128<int> mask)
	{
		throw null;
	}

	public static Vector128<long> BlendVariable(Vector128<long> left, Vector128<long> right, Vector128<long> mask)
	{
		throw null;
	}

	public static Vector128<sbyte> BlendVariable(Vector128<sbyte> left, Vector128<sbyte> right, Vector128<sbyte> mask)
	{
		throw null;
	}

	public static Vector128<float> BlendVariable(Vector128<float> left, Vector128<float> right, Vector128<float> mask)
	{
		throw null;
	}

	public static Vector128<ushort> BlendVariable(Vector128<ushort> left, Vector128<ushort> right, Vector128<ushort> mask)
	{
		throw null;
	}

	public static Vector128<uint> BlendVariable(Vector128<uint> left, Vector128<uint> right, Vector128<uint> mask)
	{
		throw null;
	}

	public static Vector128<ulong> BlendVariable(Vector128<ulong> left, Vector128<ulong> right, Vector128<ulong> mask)
	{
		throw null;
	}

	public static Vector128<double> Ceiling(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> Ceiling(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<double> CeilingScalar(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<double> CeilingScalar(Vector128<double> upper, Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> CeilingScalar(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<float> CeilingScalar(Vector128<float> upper, Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<long> CompareEqual(Vector128<long> left, Vector128<long> right)
	{
		throw null;
	}

	public static Vector128<ulong> CompareEqual(Vector128<ulong> left, Vector128<ulong> right)
	{
		throw null;
	}

	public unsafe static Vector128<short> ConvertToVector128Int16(byte* address)
	{
		throw null;
	}

	public static Vector128<short> ConvertToVector128Int16(Vector128<byte> value)
	{
		throw null;
	}

	public static Vector128<short> ConvertToVector128Int16(Vector128<sbyte> value)
	{
		throw null;
	}

	public unsafe static Vector128<short> ConvertToVector128Int16(sbyte* address)
	{
		throw null;
	}

	public unsafe static Vector128<int> ConvertToVector128Int32(byte* address)
	{
		throw null;
	}

	public unsafe static Vector128<int> ConvertToVector128Int32(short* address)
	{
		throw null;
	}

	public static Vector128<int> ConvertToVector128Int32(Vector128<byte> value)
	{
		throw null;
	}

	public static Vector128<int> ConvertToVector128Int32(Vector128<short> value)
	{
		throw null;
	}

	public static Vector128<int> ConvertToVector128Int32(Vector128<sbyte> value)
	{
		throw null;
	}

	public static Vector128<int> ConvertToVector128Int32(Vector128<ushort> value)
	{
		throw null;
	}

	public unsafe static Vector128<int> ConvertToVector128Int32(sbyte* address)
	{
		throw null;
	}

	public unsafe static Vector128<int> ConvertToVector128Int32(ushort* address)
	{
		throw null;
	}

	public unsafe static Vector128<long> ConvertToVector128Int64(byte* address)
	{
		throw null;
	}

	public unsafe static Vector128<long> ConvertToVector128Int64(short* address)
	{
		throw null;
	}

	public unsafe static Vector128<long> ConvertToVector128Int64(int* address)
	{
		throw null;
	}

	public static Vector128<long> ConvertToVector128Int64(Vector128<byte> value)
	{
		throw null;
	}

	public static Vector128<long> ConvertToVector128Int64(Vector128<short> value)
	{
		throw null;
	}

	public static Vector128<long> ConvertToVector128Int64(Vector128<int> value)
	{
		throw null;
	}

	public static Vector128<long> ConvertToVector128Int64(Vector128<sbyte> value)
	{
		throw null;
	}

	public static Vector128<long> ConvertToVector128Int64(Vector128<ushort> value)
	{
		throw null;
	}

	public static Vector128<long> ConvertToVector128Int64(Vector128<uint> value)
	{
		throw null;
	}

	public unsafe static Vector128<long> ConvertToVector128Int64(sbyte* address)
	{
		throw null;
	}

	public unsafe static Vector128<long> ConvertToVector128Int64(ushort* address)
	{
		throw null;
	}

	public unsafe static Vector128<long> ConvertToVector128Int64(uint* address)
	{
		throw null;
	}

	public static Vector128<double> DotProduct(Vector128<double> left, Vector128<double> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<float> DotProduct(Vector128<float> left, Vector128<float> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static byte Extract(Vector128<byte> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static int Extract(Vector128<int> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static float Extract(Vector128<float> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static uint Extract(Vector128<uint> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<double> Floor(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> Floor(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<double> FloorScalar(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<double> FloorScalar(Vector128<double> upper, Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> FloorScalar(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<float> FloorScalar(Vector128<float> upper, Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<byte> Insert(Vector128<byte> value, byte data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<int> Insert(Vector128<int> value, int data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<sbyte> Insert(Vector128<sbyte> value, sbyte data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<float> Insert(Vector128<float> value, Vector128<float> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public static Vector128<uint> Insert(Vector128<uint> value, uint data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public unsafe static Vector128<byte> LoadAlignedVector128NonTemporal(byte* address)
	{
		throw null;
	}

	public unsafe static Vector128<short> LoadAlignedVector128NonTemporal(short* address)
	{
		throw null;
	}

	public unsafe static Vector128<int> LoadAlignedVector128NonTemporal(int* address)
	{
		throw null;
	}

	public unsafe static Vector128<long> LoadAlignedVector128NonTemporal(long* address)
	{
		throw null;
	}

	public unsafe static Vector128<sbyte> LoadAlignedVector128NonTemporal(sbyte* address)
	{
		throw null;
	}

	public unsafe static Vector128<ushort> LoadAlignedVector128NonTemporal(ushort* address)
	{
		throw null;
	}

	public unsafe static Vector128<uint> LoadAlignedVector128NonTemporal(uint* address)
	{
		throw null;
	}

	public unsafe static Vector128<ulong> LoadAlignedVector128NonTemporal(ulong* address)
	{
		throw null;
	}

	public static Vector128<int> Max(Vector128<int> left, Vector128<int> right)
	{
		throw null;
	}

	public static Vector128<sbyte> Max(Vector128<sbyte> left, Vector128<sbyte> right)
	{
		throw null;
	}

	public static Vector128<ushort> Max(Vector128<ushort> left, Vector128<ushort> right)
	{
		throw null;
	}

	public static Vector128<uint> Max(Vector128<uint> left, Vector128<uint> right)
	{
		throw null;
	}

	public static Vector128<int> Min(Vector128<int> left, Vector128<int> right)
	{
		throw null;
	}

	public static Vector128<sbyte> Min(Vector128<sbyte> left, Vector128<sbyte> right)
	{
		throw null;
	}

	public static Vector128<ushort> Min(Vector128<ushort> left, Vector128<ushort> right)
	{
		throw null;
	}

	public static Vector128<uint> Min(Vector128<uint> left, Vector128<uint> right)
	{
		throw null;
	}

	public static Vector128<ushort> MinHorizontal(Vector128<ushort> value)
	{
		throw null;
	}

	public static Vector128<ushort> MultipleSumAbsoluteDifferences(Vector128<byte> left, Vector128<byte> right, [ConstantExpected] byte mask)
	{
		throw null;
	}

	public static Vector128<long> Multiply(Vector128<int> left, Vector128<int> right)
	{
		throw null;
	}

	public static Vector128<int> MultiplyLow(Vector128<int> left, Vector128<int> right)
	{
		throw null;
	}

	public static Vector128<uint> MultiplyLow(Vector128<uint> left, Vector128<uint> right)
	{
		throw null;
	}

	public static Vector128<ushort> PackUnsignedSaturate(Vector128<int> left, Vector128<int> right)
	{
		throw null;
	}

	public static Vector128<double> RoundCurrentDirection(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> RoundCurrentDirection(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<double> RoundCurrentDirectionScalar(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<double> RoundCurrentDirectionScalar(Vector128<double> upper, Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> RoundCurrentDirectionScalar(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<float> RoundCurrentDirectionScalar(Vector128<float> upper, Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<double> RoundToNearestInteger(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> RoundToNearestInteger(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<double> RoundToNearestIntegerScalar(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<double> RoundToNearestIntegerScalar(Vector128<double> upper, Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> RoundToNearestIntegerScalar(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<float> RoundToNearestIntegerScalar(Vector128<float> upper, Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<double> RoundToNegativeInfinity(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> RoundToNegativeInfinity(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<double> RoundToNegativeInfinityScalar(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<double> RoundToNegativeInfinityScalar(Vector128<double> upper, Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> RoundToNegativeInfinityScalar(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<float> RoundToNegativeInfinityScalar(Vector128<float> upper, Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<double> RoundToPositiveInfinity(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> RoundToPositiveInfinity(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<double> RoundToPositiveInfinityScalar(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<double> RoundToPositiveInfinityScalar(Vector128<double> upper, Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> RoundToPositiveInfinityScalar(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<float> RoundToPositiveInfinityScalar(Vector128<float> upper, Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<double> RoundToZero(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> RoundToZero(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<double> RoundToZeroScalar(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<double> RoundToZeroScalar(Vector128<double> upper, Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<float> RoundToZeroScalar(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<float> RoundToZeroScalar(Vector128<float> upper, Vector128<float> value)
	{
		throw null;
	}

	public static bool TestC(Vector128<byte> left, Vector128<byte> right)
	{
		throw null;
	}

	public static bool TestC(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static bool TestC(Vector128<int> left, Vector128<int> right)
	{
		throw null;
	}

	public static bool TestC(Vector128<long> left, Vector128<long> right)
	{
		throw null;
	}

	public static bool TestC(Vector128<sbyte> left, Vector128<sbyte> right)
	{
		throw null;
	}

	public static bool TestC(Vector128<ushort> left, Vector128<ushort> right)
	{
		throw null;
	}

	public static bool TestC(Vector128<uint> left, Vector128<uint> right)
	{
		throw null;
	}

	public static bool TestC(Vector128<ulong> left, Vector128<ulong> right)
	{
		throw null;
	}

	public static bool TestNotZAndNotC(Vector128<byte> left, Vector128<byte> right)
	{
		throw null;
	}

	public static bool TestNotZAndNotC(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static bool TestNotZAndNotC(Vector128<int> left, Vector128<int> right)
	{
		throw null;
	}

	public static bool TestNotZAndNotC(Vector128<long> left, Vector128<long> right)
	{
		throw null;
	}

	public static bool TestNotZAndNotC(Vector128<sbyte> left, Vector128<sbyte> right)
	{
		throw null;
	}

	public static bool TestNotZAndNotC(Vector128<ushort> left, Vector128<ushort> right)
	{
		throw null;
	}

	public static bool TestNotZAndNotC(Vector128<uint> left, Vector128<uint> right)
	{
		throw null;
	}

	public static bool TestNotZAndNotC(Vector128<ulong> left, Vector128<ulong> right)
	{
		throw null;
	}

	public static bool TestZ(Vector128<byte> left, Vector128<byte> right)
	{
		throw null;
	}

	public static bool TestZ(Vector128<short> left, Vector128<short> right)
	{
		throw null;
	}

	public static bool TestZ(Vector128<int> left, Vector128<int> right)
	{
		throw null;
	}

	public static bool TestZ(Vector128<long> left, Vector128<long> right)
	{
		throw null;
	}

	public static bool TestZ(Vector128<sbyte> left, Vector128<sbyte> right)
	{
		throw null;
	}

	public static bool TestZ(Vector128<ushort> left, Vector128<ushort> right)
	{
		throw null;
	}

	public static bool TestZ(Vector128<uint> left, Vector128<uint> right)
	{
		throw null;
	}

	public static bool TestZ(Vector128<ulong> left, Vector128<ulong> right)
	{
		throw null;
	}
}
