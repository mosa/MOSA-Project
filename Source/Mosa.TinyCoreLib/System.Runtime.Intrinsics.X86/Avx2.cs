using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Intrinsics.X86;

[CLSCompliant(false)]
public abstract class Avx2 : Avx
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

	internal Avx2()
	{
	}

	public static Vector256<ushort> Abs(Vector256<short> value)
	{
		throw null;
	}

	public static Vector256<uint> Abs(Vector256<int> value)
	{
		throw null;
	}

	public static Vector256<byte> Abs(Vector256<sbyte> value)
	{
		throw null;
	}

	public static Vector256<byte> Add(Vector256<byte> left, Vector256<byte> right)
	{
		throw null;
	}

	public static Vector256<short> Add(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<int> Add(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static Vector256<long> Add(Vector256<long> left, Vector256<long> right)
	{
		throw null;
	}

	public static Vector256<sbyte> Add(Vector256<sbyte> left, Vector256<sbyte> right)
	{
		throw null;
	}

	public static Vector256<ushort> Add(Vector256<ushort> left, Vector256<ushort> right)
	{
		throw null;
	}

	public static Vector256<uint> Add(Vector256<uint> left, Vector256<uint> right)
	{
		throw null;
	}

	public static Vector256<ulong> Add(Vector256<ulong> left, Vector256<ulong> right)
	{
		throw null;
	}

	public static Vector256<byte> AddSaturate(Vector256<byte> left, Vector256<byte> right)
	{
		throw null;
	}

	public static Vector256<short> AddSaturate(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<sbyte> AddSaturate(Vector256<sbyte> left, Vector256<sbyte> right)
	{
		throw null;
	}

	public static Vector256<ushort> AddSaturate(Vector256<ushort> left, Vector256<ushort> right)
	{
		throw null;
	}

	public static Vector256<byte> AlignRight(Vector256<byte> left, Vector256<byte> right, [ConstantExpected] byte mask)
	{
		throw null;
	}

	public static Vector256<short> AlignRight(Vector256<short> left, Vector256<short> right, [ConstantExpected] byte mask)
	{
		throw null;
	}

	public static Vector256<int> AlignRight(Vector256<int> left, Vector256<int> right, [ConstantExpected] byte mask)
	{
		throw null;
	}

	public static Vector256<long> AlignRight(Vector256<long> left, Vector256<long> right, [ConstantExpected] byte mask)
	{
		throw null;
	}

	public static Vector256<sbyte> AlignRight(Vector256<sbyte> left, Vector256<sbyte> right, [ConstantExpected] byte mask)
	{
		throw null;
	}

	public static Vector256<ushort> AlignRight(Vector256<ushort> left, Vector256<ushort> right, [ConstantExpected] byte mask)
	{
		throw null;
	}

	public static Vector256<uint> AlignRight(Vector256<uint> left, Vector256<uint> right, [ConstantExpected] byte mask)
	{
		throw null;
	}

	public static Vector256<ulong> AlignRight(Vector256<ulong> left, Vector256<ulong> right, [ConstantExpected] byte mask)
	{
		throw null;
	}

	public static Vector256<byte> And(Vector256<byte> left, Vector256<byte> right)
	{
		throw null;
	}

	public static Vector256<short> And(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<int> And(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static Vector256<long> And(Vector256<long> left, Vector256<long> right)
	{
		throw null;
	}

	public static Vector256<sbyte> And(Vector256<sbyte> left, Vector256<sbyte> right)
	{
		throw null;
	}

	public static Vector256<ushort> And(Vector256<ushort> left, Vector256<ushort> right)
	{
		throw null;
	}

	public static Vector256<uint> And(Vector256<uint> left, Vector256<uint> right)
	{
		throw null;
	}

	public static Vector256<ulong> And(Vector256<ulong> left, Vector256<ulong> right)
	{
		throw null;
	}

	public static Vector256<byte> AndNot(Vector256<byte> left, Vector256<byte> right)
	{
		throw null;
	}

	public static Vector256<short> AndNot(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<int> AndNot(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static Vector256<long> AndNot(Vector256<long> left, Vector256<long> right)
	{
		throw null;
	}

	public static Vector256<sbyte> AndNot(Vector256<sbyte> left, Vector256<sbyte> right)
	{
		throw null;
	}

	public static Vector256<ushort> AndNot(Vector256<ushort> left, Vector256<ushort> right)
	{
		throw null;
	}

	public static Vector256<uint> AndNot(Vector256<uint> left, Vector256<uint> right)
	{
		throw null;
	}

	public static Vector256<ulong> AndNot(Vector256<ulong> left, Vector256<ulong> right)
	{
		throw null;
	}

	public static Vector256<byte> Average(Vector256<byte> left, Vector256<byte> right)
	{
		throw null;
	}

	public static Vector256<ushort> Average(Vector256<ushort> left, Vector256<ushort> right)
	{
		throw null;
	}

	public static Vector128<int> Blend(Vector128<int> left, Vector128<int> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector128<uint> Blend(Vector128<uint> left, Vector128<uint> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<short> Blend(Vector256<short> left, Vector256<short> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<int> Blend(Vector256<int> left, Vector256<int> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<ushort> Blend(Vector256<ushort> left, Vector256<ushort> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<uint> Blend(Vector256<uint> left, Vector256<uint> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<byte> BlendVariable(Vector256<byte> left, Vector256<byte> right, Vector256<byte> mask)
	{
		throw null;
	}

	public static Vector256<short> BlendVariable(Vector256<short> left, Vector256<short> right, Vector256<short> mask)
	{
		throw null;
	}

	public static Vector256<int> BlendVariable(Vector256<int> left, Vector256<int> right, Vector256<int> mask)
	{
		throw null;
	}

	public static Vector256<long> BlendVariable(Vector256<long> left, Vector256<long> right, Vector256<long> mask)
	{
		throw null;
	}

	public static Vector256<sbyte> BlendVariable(Vector256<sbyte> left, Vector256<sbyte> right, Vector256<sbyte> mask)
	{
		throw null;
	}

	public static Vector256<ushort> BlendVariable(Vector256<ushort> left, Vector256<ushort> right, Vector256<ushort> mask)
	{
		throw null;
	}

	public static Vector256<uint> BlendVariable(Vector256<uint> left, Vector256<uint> right, Vector256<uint> mask)
	{
		throw null;
	}

	public static Vector256<ulong> BlendVariable(Vector256<ulong> left, Vector256<ulong> right, Vector256<ulong> mask)
	{
		throw null;
	}

	public unsafe static Vector128<byte> BroadcastScalarToVector128(byte* source)
	{
		throw null;
	}

	public unsafe static Vector128<short> BroadcastScalarToVector128(short* source)
	{
		throw null;
	}

	public unsafe static Vector128<int> BroadcastScalarToVector128(int* source)
	{
		throw null;
	}

	public unsafe static Vector128<long> BroadcastScalarToVector128(long* source)
	{
		throw null;
	}

	public static Vector128<byte> BroadcastScalarToVector128(Vector128<byte> value)
	{
		throw null;
	}

	public static Vector128<double> BroadcastScalarToVector128(Vector128<double> value)
	{
		throw null;
	}

	public static Vector128<short> BroadcastScalarToVector128(Vector128<short> value)
	{
		throw null;
	}

	public static Vector128<int> BroadcastScalarToVector128(Vector128<int> value)
	{
		throw null;
	}

	public static Vector128<long> BroadcastScalarToVector128(Vector128<long> value)
	{
		throw null;
	}

	public static Vector128<sbyte> BroadcastScalarToVector128(Vector128<sbyte> value)
	{
		throw null;
	}

	public static Vector128<float> BroadcastScalarToVector128(Vector128<float> value)
	{
		throw null;
	}

	public static Vector128<ushort> BroadcastScalarToVector128(Vector128<ushort> value)
	{
		throw null;
	}

	public static Vector128<uint> BroadcastScalarToVector128(Vector128<uint> value)
	{
		throw null;
	}

	public static Vector128<ulong> BroadcastScalarToVector128(Vector128<ulong> value)
	{
		throw null;
	}

	public unsafe static Vector128<sbyte> BroadcastScalarToVector128(sbyte* source)
	{
		throw null;
	}

	public unsafe static Vector128<ushort> BroadcastScalarToVector128(ushort* source)
	{
		throw null;
	}

	public unsafe static Vector128<uint> BroadcastScalarToVector128(uint* source)
	{
		throw null;
	}

	public unsafe static Vector128<ulong> BroadcastScalarToVector128(ulong* source)
	{
		throw null;
	}

	public unsafe static Vector256<byte> BroadcastScalarToVector256(byte* source)
	{
		throw null;
	}

	public unsafe static Vector256<short> BroadcastScalarToVector256(short* source)
	{
		throw null;
	}

	public unsafe static Vector256<int> BroadcastScalarToVector256(int* source)
	{
		throw null;
	}

	public unsafe static Vector256<long> BroadcastScalarToVector256(long* source)
	{
		throw null;
	}

	public static Vector256<byte> BroadcastScalarToVector256(Vector128<byte> value)
	{
		throw null;
	}

	public static Vector256<double> BroadcastScalarToVector256(Vector128<double> value)
	{
		throw null;
	}

	public static Vector256<short> BroadcastScalarToVector256(Vector128<short> value)
	{
		throw null;
	}

	public static Vector256<int> BroadcastScalarToVector256(Vector128<int> value)
	{
		throw null;
	}

	public static Vector256<long> BroadcastScalarToVector256(Vector128<long> value)
	{
		throw null;
	}

	public static Vector256<sbyte> BroadcastScalarToVector256(Vector128<sbyte> value)
	{
		throw null;
	}

	public static Vector256<float> BroadcastScalarToVector256(Vector128<float> value)
	{
		throw null;
	}

	public static Vector256<ushort> BroadcastScalarToVector256(Vector128<ushort> value)
	{
		throw null;
	}

	public static Vector256<uint> BroadcastScalarToVector256(Vector128<uint> value)
	{
		throw null;
	}

	public static Vector256<ulong> BroadcastScalarToVector256(Vector128<ulong> value)
	{
		throw null;
	}

	public unsafe static Vector256<sbyte> BroadcastScalarToVector256(sbyte* source)
	{
		throw null;
	}

	public unsafe static Vector256<ushort> BroadcastScalarToVector256(ushort* source)
	{
		throw null;
	}

	public unsafe static Vector256<uint> BroadcastScalarToVector256(uint* source)
	{
		throw null;
	}

	public unsafe static Vector256<ulong> BroadcastScalarToVector256(ulong* source)
	{
		throw null;
	}

	public unsafe static Vector256<byte> BroadcastVector128ToVector256(byte* address)
	{
		throw null;
	}

	public unsafe static Vector256<short> BroadcastVector128ToVector256(short* address)
	{
		throw null;
	}

	public unsafe static Vector256<int> BroadcastVector128ToVector256(int* address)
	{
		throw null;
	}

	public unsafe static Vector256<long> BroadcastVector128ToVector256(long* address)
	{
		throw null;
	}

	public unsafe static Vector256<sbyte> BroadcastVector128ToVector256(sbyte* address)
	{
		throw null;
	}

	public unsafe static Vector256<ushort> BroadcastVector128ToVector256(ushort* address)
	{
		throw null;
	}

	public unsafe static Vector256<uint> BroadcastVector128ToVector256(uint* address)
	{
		throw null;
	}

	public unsafe static Vector256<ulong> BroadcastVector128ToVector256(ulong* address)
	{
		throw null;
	}

	public static Vector256<byte> CompareEqual(Vector256<byte> left, Vector256<byte> right)
	{
		throw null;
	}

	public static Vector256<short> CompareEqual(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<int> CompareEqual(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static Vector256<long> CompareEqual(Vector256<long> left, Vector256<long> right)
	{
		throw null;
	}

	public static Vector256<sbyte> CompareEqual(Vector256<sbyte> left, Vector256<sbyte> right)
	{
		throw null;
	}

	public static Vector256<ushort> CompareEqual(Vector256<ushort> left, Vector256<ushort> right)
	{
		throw null;
	}

	public static Vector256<uint> CompareEqual(Vector256<uint> left, Vector256<uint> right)
	{
		throw null;
	}

	public static Vector256<ulong> CompareEqual(Vector256<ulong> left, Vector256<ulong> right)
	{
		throw null;
	}

	public static Vector256<short> CompareGreaterThan(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<int> CompareGreaterThan(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static Vector256<long> CompareGreaterThan(Vector256<long> left, Vector256<long> right)
	{
		throw null;
	}

	public static Vector256<sbyte> CompareGreaterThan(Vector256<sbyte> left, Vector256<sbyte> right)
	{
		throw null;
	}

	public static int ConvertToInt32(Vector256<int> value)
	{
		throw null;
	}

	public static uint ConvertToUInt32(Vector256<uint> value)
	{
		throw null;
	}

	public unsafe static Vector256<short> ConvertToVector256Int16(byte* address)
	{
		throw null;
	}

	public static Vector256<short> ConvertToVector256Int16(Vector128<byte> value)
	{
		throw null;
	}

	public static Vector256<short> ConvertToVector256Int16(Vector128<sbyte> value)
	{
		throw null;
	}

	public unsafe static Vector256<short> ConvertToVector256Int16(sbyte* address)
	{
		throw null;
	}

	public unsafe static Vector256<int> ConvertToVector256Int32(byte* address)
	{
		throw null;
	}

	public unsafe static Vector256<int> ConvertToVector256Int32(short* address)
	{
		throw null;
	}

	public static Vector256<int> ConvertToVector256Int32(Vector128<byte> value)
	{
		throw null;
	}

	public static Vector256<int> ConvertToVector256Int32(Vector128<short> value)
	{
		throw null;
	}

	public static Vector256<int> ConvertToVector256Int32(Vector128<sbyte> value)
	{
		throw null;
	}

	public static Vector256<int> ConvertToVector256Int32(Vector128<ushort> value)
	{
		throw null;
	}

	public unsafe static Vector256<int> ConvertToVector256Int32(sbyte* address)
	{
		throw null;
	}

	public unsafe static Vector256<int> ConvertToVector256Int32(ushort* address)
	{
		throw null;
	}

	public unsafe static Vector256<long> ConvertToVector256Int64(byte* address)
	{
		throw null;
	}

	public unsafe static Vector256<long> ConvertToVector256Int64(short* address)
	{
		throw null;
	}

	public unsafe static Vector256<long> ConvertToVector256Int64(int* address)
	{
		throw null;
	}

	public static Vector256<long> ConvertToVector256Int64(Vector128<byte> value)
	{
		throw null;
	}

	public static Vector256<long> ConvertToVector256Int64(Vector128<short> value)
	{
		throw null;
	}

	public static Vector256<long> ConvertToVector256Int64(Vector128<int> value)
	{
		throw null;
	}

	public static Vector256<long> ConvertToVector256Int64(Vector128<sbyte> value)
	{
		throw null;
	}

	public static Vector256<long> ConvertToVector256Int64(Vector128<ushort> value)
	{
		throw null;
	}

	public static Vector256<long> ConvertToVector256Int64(Vector128<uint> value)
	{
		throw null;
	}

	public unsafe static Vector256<long> ConvertToVector256Int64(sbyte* address)
	{
		throw null;
	}

	public unsafe static Vector256<long> ConvertToVector256Int64(ushort* address)
	{
		throw null;
	}

	public unsafe static Vector256<long> ConvertToVector256Int64(uint* address)
	{
		throw null;
	}

	public new static Vector128<byte> ExtractVector128(Vector256<byte> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector128<short> ExtractVector128(Vector256<short> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector128<int> ExtractVector128(Vector256<int> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector128<long> ExtractVector128(Vector256<long> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector128<sbyte> ExtractVector128(Vector256<sbyte> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector128<ushort> ExtractVector128(Vector256<ushort> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector128<uint> ExtractVector128(Vector256<uint> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector128<ulong> ExtractVector128(Vector256<ulong> value, [ConstantExpected] byte index)
	{
		throw null;
	}

	public unsafe static Vector128<double> GatherMaskVector128(Vector128<double> source, double* baseAddress, Vector128<int> index, Vector128<double> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<double> GatherMaskVector128(Vector128<double> source, double* baseAddress, Vector128<long> index, Vector128<double> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<int> GatherMaskVector128(Vector128<int> source, int* baseAddress, Vector128<int> index, Vector128<int> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<int> GatherMaskVector128(Vector128<int> source, int* baseAddress, Vector128<long> index, Vector128<int> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<int> GatherMaskVector128(Vector128<int> source, int* baseAddress, Vector256<long> index, Vector128<int> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<long> GatherMaskVector128(Vector128<long> source, long* baseAddress, Vector128<int> index, Vector128<long> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<long> GatherMaskVector128(Vector128<long> source, long* baseAddress, Vector128<long> index, Vector128<long> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<float> GatherMaskVector128(Vector128<float> source, float* baseAddress, Vector128<int> index, Vector128<float> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<float> GatherMaskVector128(Vector128<float> source, float* baseAddress, Vector128<long> index, Vector128<float> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<float> GatherMaskVector128(Vector128<float> source, float* baseAddress, Vector256<long> index, Vector128<float> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<uint> GatherMaskVector128(Vector128<uint> source, uint* baseAddress, Vector128<int> index, Vector128<uint> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<uint> GatherMaskVector128(Vector128<uint> source, uint* baseAddress, Vector128<long> index, Vector128<uint> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<uint> GatherMaskVector128(Vector128<uint> source, uint* baseAddress, Vector256<long> index, Vector128<uint> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<ulong> GatherMaskVector128(Vector128<ulong> source, ulong* baseAddress, Vector128<int> index, Vector128<ulong> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<ulong> GatherMaskVector128(Vector128<ulong> source, ulong* baseAddress, Vector128<long> index, Vector128<ulong> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector256<double> GatherMaskVector256(Vector256<double> source, double* baseAddress, Vector128<int> index, Vector256<double> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector256<double> GatherMaskVector256(Vector256<double> source, double* baseAddress, Vector256<long> index, Vector256<double> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector256<int> GatherMaskVector256(Vector256<int> source, int* baseAddress, Vector256<int> index, Vector256<int> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector256<long> GatherMaskVector256(Vector256<long> source, long* baseAddress, Vector128<int> index, Vector256<long> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector256<long> GatherMaskVector256(Vector256<long> source, long* baseAddress, Vector256<long> index, Vector256<long> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector256<float> GatherMaskVector256(Vector256<float> source, float* baseAddress, Vector256<int> index, Vector256<float> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector256<uint> GatherMaskVector256(Vector256<uint> source, uint* baseAddress, Vector256<int> index, Vector256<uint> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector256<ulong> GatherMaskVector256(Vector256<ulong> source, ulong* baseAddress, Vector128<int> index, Vector256<ulong> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector256<ulong> GatherMaskVector256(Vector256<ulong> source, ulong* baseAddress, Vector256<long> index, Vector256<ulong> mask, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<double> GatherVector128(double* baseAddress, Vector128<int> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<double> GatherVector128(double* baseAddress, Vector128<long> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<int> GatherVector128(int* baseAddress, Vector128<int> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<int> GatherVector128(int* baseAddress, Vector128<long> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<int> GatherVector128(int* baseAddress, Vector256<long> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<long> GatherVector128(long* baseAddress, Vector128<int> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<long> GatherVector128(long* baseAddress, Vector128<long> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<float> GatherVector128(float* baseAddress, Vector128<int> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<float> GatherVector128(float* baseAddress, Vector128<long> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<float> GatherVector128(float* baseAddress, Vector256<long> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<uint> GatherVector128(uint* baseAddress, Vector128<int> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<uint> GatherVector128(uint* baseAddress, Vector128<long> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<uint> GatherVector128(uint* baseAddress, Vector256<long> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<ulong> GatherVector128(ulong* baseAddress, Vector128<int> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector128<ulong> GatherVector128(ulong* baseAddress, Vector128<long> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector256<double> GatherVector256(double* baseAddress, Vector128<int> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector256<double> GatherVector256(double* baseAddress, Vector256<long> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector256<int> GatherVector256(int* baseAddress, Vector256<int> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector256<long> GatherVector256(long* baseAddress, Vector128<int> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector256<long> GatherVector256(long* baseAddress, Vector256<long> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector256<float> GatherVector256(float* baseAddress, Vector256<int> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector256<uint> GatherVector256(uint* baseAddress, Vector256<int> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector256<ulong> GatherVector256(ulong* baseAddress, Vector128<int> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public unsafe static Vector256<ulong> GatherVector256(ulong* baseAddress, Vector256<long> index, [ConstantExpected(Min = 1, Max = 8)] byte scale)
	{
		throw null;
	}

	public static Vector256<short> HorizontalAdd(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<int> HorizontalAdd(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static Vector256<short> HorizontalAddSaturate(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<short> HorizontalSubtract(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<int> HorizontalSubtract(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static Vector256<short> HorizontalSubtractSaturate(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public new static Vector256<byte> InsertVector128(Vector256<byte> value, Vector128<byte> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector256<short> InsertVector128(Vector256<short> value, Vector128<short> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector256<int> InsertVector128(Vector256<int> value, Vector128<int> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector256<long> InsertVector128(Vector256<long> value, Vector128<long> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector256<sbyte> InsertVector128(Vector256<sbyte> value, Vector128<sbyte> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector256<ushort> InsertVector128(Vector256<ushort> value, Vector128<ushort> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector256<uint> InsertVector128(Vector256<uint> value, Vector128<uint> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public new static Vector256<ulong> InsertVector128(Vector256<ulong> value, Vector128<ulong> data, [ConstantExpected] byte index)
	{
		throw null;
	}

	public unsafe static Vector256<byte> LoadAlignedVector256NonTemporal(byte* address)
	{
		throw null;
	}

	public unsafe static Vector256<short> LoadAlignedVector256NonTemporal(short* address)
	{
		throw null;
	}

	public unsafe static Vector256<int> LoadAlignedVector256NonTemporal(int* address)
	{
		throw null;
	}

	public unsafe static Vector256<long> LoadAlignedVector256NonTemporal(long* address)
	{
		throw null;
	}

	public unsafe static Vector256<sbyte> LoadAlignedVector256NonTemporal(sbyte* address)
	{
		throw null;
	}

	public unsafe static Vector256<ushort> LoadAlignedVector256NonTemporal(ushort* address)
	{
		throw null;
	}

	public unsafe static Vector256<uint> LoadAlignedVector256NonTemporal(uint* address)
	{
		throw null;
	}

	public unsafe static Vector256<ulong> LoadAlignedVector256NonTemporal(ulong* address)
	{
		throw null;
	}

	public unsafe static Vector128<int> MaskLoad(int* address, Vector128<int> mask)
	{
		throw null;
	}

	public unsafe static Vector256<int> MaskLoad(int* address, Vector256<int> mask)
	{
		throw null;
	}

	public unsafe static Vector128<long> MaskLoad(long* address, Vector128<long> mask)
	{
		throw null;
	}

	public unsafe static Vector256<long> MaskLoad(long* address, Vector256<long> mask)
	{
		throw null;
	}

	public unsafe static Vector128<uint> MaskLoad(uint* address, Vector128<uint> mask)
	{
		throw null;
	}

	public unsafe static Vector256<uint> MaskLoad(uint* address, Vector256<uint> mask)
	{
		throw null;
	}

	public unsafe static Vector128<ulong> MaskLoad(ulong* address, Vector128<ulong> mask)
	{
		throw null;
	}

	public unsafe static Vector256<ulong> MaskLoad(ulong* address, Vector256<ulong> mask)
	{
		throw null;
	}

	public unsafe static void MaskStore(int* address, Vector128<int> mask, Vector128<int> source)
	{
	}

	public unsafe static void MaskStore(int* address, Vector256<int> mask, Vector256<int> source)
	{
	}

	public unsafe static void MaskStore(long* address, Vector128<long> mask, Vector128<long> source)
	{
	}

	public unsafe static void MaskStore(long* address, Vector256<long> mask, Vector256<long> source)
	{
	}

	public unsafe static void MaskStore(uint* address, Vector128<uint> mask, Vector128<uint> source)
	{
	}

	public unsafe static void MaskStore(uint* address, Vector256<uint> mask, Vector256<uint> source)
	{
	}

	public unsafe static void MaskStore(ulong* address, Vector128<ulong> mask, Vector128<ulong> source)
	{
	}

	public unsafe static void MaskStore(ulong* address, Vector256<ulong> mask, Vector256<ulong> source)
	{
	}

	public static Vector256<byte> Max(Vector256<byte> left, Vector256<byte> right)
	{
		throw null;
	}

	public static Vector256<short> Max(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<int> Max(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static Vector256<sbyte> Max(Vector256<sbyte> left, Vector256<sbyte> right)
	{
		throw null;
	}

	public static Vector256<ushort> Max(Vector256<ushort> left, Vector256<ushort> right)
	{
		throw null;
	}

	public static Vector256<uint> Max(Vector256<uint> left, Vector256<uint> right)
	{
		throw null;
	}

	public static Vector256<byte> Min(Vector256<byte> left, Vector256<byte> right)
	{
		throw null;
	}

	public static Vector256<short> Min(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<int> Min(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static Vector256<sbyte> Min(Vector256<sbyte> left, Vector256<sbyte> right)
	{
		throw null;
	}

	public static Vector256<ushort> Min(Vector256<ushort> left, Vector256<ushort> right)
	{
		throw null;
	}

	public static Vector256<uint> Min(Vector256<uint> left, Vector256<uint> right)
	{
		throw null;
	}

	public static int MoveMask(Vector256<byte> value)
	{
		throw null;
	}

	public static int MoveMask(Vector256<sbyte> value)
	{
		throw null;
	}

	public static Vector256<ushort> MultipleSumAbsoluteDifferences(Vector256<byte> left, Vector256<byte> right, [ConstantExpected] byte mask)
	{
		throw null;
	}

	public static Vector256<long> Multiply(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static Vector256<ulong> Multiply(Vector256<uint> left, Vector256<uint> right)
	{
		throw null;
	}

	public static Vector256<short> MultiplyAddAdjacent(Vector256<byte> left, Vector256<sbyte> right)
	{
		throw null;
	}

	public static Vector256<int> MultiplyAddAdjacent(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<short> MultiplyHigh(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<ushort> MultiplyHigh(Vector256<ushort> left, Vector256<ushort> right)
	{
		throw null;
	}

	public static Vector256<short> MultiplyHighRoundScale(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<short> MultiplyLow(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<int> MultiplyLow(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static Vector256<ushort> MultiplyLow(Vector256<ushort> left, Vector256<ushort> right)
	{
		throw null;
	}

	public static Vector256<uint> MultiplyLow(Vector256<uint> left, Vector256<uint> right)
	{
		throw null;
	}

	public static Vector256<byte> Or(Vector256<byte> left, Vector256<byte> right)
	{
		throw null;
	}

	public static Vector256<short> Or(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<int> Or(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static Vector256<long> Or(Vector256<long> left, Vector256<long> right)
	{
		throw null;
	}

	public static Vector256<sbyte> Or(Vector256<sbyte> left, Vector256<sbyte> right)
	{
		throw null;
	}

	public static Vector256<ushort> Or(Vector256<ushort> left, Vector256<ushort> right)
	{
		throw null;
	}

	public static Vector256<uint> Or(Vector256<uint> left, Vector256<uint> right)
	{
		throw null;
	}

	public static Vector256<ulong> Or(Vector256<ulong> left, Vector256<ulong> right)
	{
		throw null;
	}

	public static Vector256<sbyte> PackSignedSaturate(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<short> PackSignedSaturate(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static Vector256<byte> PackUnsignedSaturate(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<ushort> PackUnsignedSaturate(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public new static Vector256<byte> Permute2x128(Vector256<byte> left, Vector256<byte> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public new static Vector256<short> Permute2x128(Vector256<short> left, Vector256<short> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public new static Vector256<int> Permute2x128(Vector256<int> left, Vector256<int> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public new static Vector256<long> Permute2x128(Vector256<long> left, Vector256<long> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public new static Vector256<sbyte> Permute2x128(Vector256<sbyte> left, Vector256<sbyte> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public new static Vector256<ushort> Permute2x128(Vector256<ushort> left, Vector256<ushort> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public new static Vector256<uint> Permute2x128(Vector256<uint> left, Vector256<uint> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public new static Vector256<ulong> Permute2x128(Vector256<ulong> left, Vector256<ulong> right, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<double> Permute4x64(Vector256<double> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<long> Permute4x64(Vector256<long> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<ulong> Permute4x64(Vector256<ulong> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<int> PermuteVar8x32(Vector256<int> left, Vector256<int> control)
	{
		throw null;
	}

	public static Vector256<float> PermuteVar8x32(Vector256<float> left, Vector256<int> control)
	{
		throw null;
	}

	public static Vector256<uint> PermuteVar8x32(Vector256<uint> left, Vector256<uint> control)
	{
		throw null;
	}

	public static Vector256<short> ShiftLeftLogical(Vector256<short> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector256<short> ShiftLeftLogical(Vector256<short> value, Vector128<short> count)
	{
		throw null;
	}

	public static Vector256<int> ShiftLeftLogical(Vector256<int> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector256<int> ShiftLeftLogical(Vector256<int> value, Vector128<int> count)
	{
		throw null;
	}

	public static Vector256<long> ShiftLeftLogical(Vector256<long> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector256<long> ShiftLeftLogical(Vector256<long> value, Vector128<long> count)
	{
		throw null;
	}

	public static Vector256<ushort> ShiftLeftLogical(Vector256<ushort> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector256<ushort> ShiftLeftLogical(Vector256<ushort> value, Vector128<ushort> count)
	{
		throw null;
	}

	public static Vector256<uint> ShiftLeftLogical(Vector256<uint> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector256<uint> ShiftLeftLogical(Vector256<uint> value, Vector128<uint> count)
	{
		throw null;
	}

	public static Vector256<ulong> ShiftLeftLogical(Vector256<ulong> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector256<ulong> ShiftLeftLogical(Vector256<ulong> value, Vector128<ulong> count)
	{
		throw null;
	}

	public static Vector256<byte> ShiftLeftLogical128BitLane(Vector256<byte> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector256<short> ShiftLeftLogical128BitLane(Vector256<short> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector256<int> ShiftLeftLogical128BitLane(Vector256<int> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector256<long> ShiftLeftLogical128BitLane(Vector256<long> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector256<sbyte> ShiftLeftLogical128BitLane(Vector256<sbyte> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector256<ushort> ShiftLeftLogical128BitLane(Vector256<ushort> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector256<uint> ShiftLeftLogical128BitLane(Vector256<uint> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector256<ulong> ShiftLeftLogical128BitLane(Vector256<ulong> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector128<int> ShiftLeftLogicalVariable(Vector128<int> value, Vector128<uint> count)
	{
		throw null;
	}

	public static Vector128<long> ShiftLeftLogicalVariable(Vector128<long> value, Vector128<ulong> count)
	{
		throw null;
	}

	public static Vector128<uint> ShiftLeftLogicalVariable(Vector128<uint> value, Vector128<uint> count)
	{
		throw null;
	}

	public static Vector128<ulong> ShiftLeftLogicalVariable(Vector128<ulong> value, Vector128<ulong> count)
	{
		throw null;
	}

	public static Vector256<int> ShiftLeftLogicalVariable(Vector256<int> value, Vector256<uint> count)
	{
		throw null;
	}

	public static Vector256<long> ShiftLeftLogicalVariable(Vector256<long> value, Vector256<ulong> count)
	{
		throw null;
	}

	public static Vector256<uint> ShiftLeftLogicalVariable(Vector256<uint> value, Vector256<uint> count)
	{
		throw null;
	}

	public static Vector256<ulong> ShiftLeftLogicalVariable(Vector256<ulong> value, Vector256<ulong> count)
	{
		throw null;
	}

	public static Vector256<short> ShiftRightArithmetic(Vector256<short> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector256<short> ShiftRightArithmetic(Vector256<short> value, Vector128<short> count)
	{
		throw null;
	}

	public static Vector256<int> ShiftRightArithmetic(Vector256<int> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector256<int> ShiftRightArithmetic(Vector256<int> value, Vector128<int> count)
	{
		throw null;
	}

	public static Vector128<int> ShiftRightArithmeticVariable(Vector128<int> value, Vector128<uint> count)
	{
		throw null;
	}

	public static Vector256<int> ShiftRightArithmeticVariable(Vector256<int> value, Vector256<uint> count)
	{
		throw null;
	}

	public static Vector256<short> ShiftRightLogical(Vector256<short> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector256<short> ShiftRightLogical(Vector256<short> value, Vector128<short> count)
	{
		throw null;
	}

	public static Vector256<int> ShiftRightLogical(Vector256<int> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector256<int> ShiftRightLogical(Vector256<int> value, Vector128<int> count)
	{
		throw null;
	}

	public static Vector256<long> ShiftRightLogical(Vector256<long> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector256<long> ShiftRightLogical(Vector256<long> value, Vector128<long> count)
	{
		throw null;
	}

	public static Vector256<ushort> ShiftRightLogical(Vector256<ushort> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector256<ushort> ShiftRightLogical(Vector256<ushort> value, Vector128<ushort> count)
	{
		throw null;
	}

	public static Vector256<uint> ShiftRightLogical(Vector256<uint> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector256<uint> ShiftRightLogical(Vector256<uint> value, Vector128<uint> count)
	{
		throw null;
	}

	public static Vector256<ulong> ShiftRightLogical(Vector256<ulong> value, [ConstantExpected] byte count)
	{
		throw null;
	}

	public static Vector256<ulong> ShiftRightLogical(Vector256<ulong> value, Vector128<ulong> count)
	{
		throw null;
	}

	public static Vector256<byte> ShiftRightLogical128BitLane(Vector256<byte> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector256<short> ShiftRightLogical128BitLane(Vector256<short> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector256<int> ShiftRightLogical128BitLane(Vector256<int> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector256<long> ShiftRightLogical128BitLane(Vector256<long> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector256<sbyte> ShiftRightLogical128BitLane(Vector256<sbyte> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector256<ushort> ShiftRightLogical128BitLane(Vector256<ushort> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector256<uint> ShiftRightLogical128BitLane(Vector256<uint> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector256<ulong> ShiftRightLogical128BitLane(Vector256<ulong> value, [ConstantExpected] byte numBytes)
	{
		throw null;
	}

	public static Vector128<int> ShiftRightLogicalVariable(Vector128<int> value, Vector128<uint> count)
	{
		throw null;
	}

	public static Vector128<long> ShiftRightLogicalVariable(Vector128<long> value, Vector128<ulong> count)
	{
		throw null;
	}

	public static Vector128<uint> ShiftRightLogicalVariable(Vector128<uint> value, Vector128<uint> count)
	{
		throw null;
	}

	public static Vector128<ulong> ShiftRightLogicalVariable(Vector128<ulong> value, Vector128<ulong> count)
	{
		throw null;
	}

	public static Vector256<int> ShiftRightLogicalVariable(Vector256<int> value, Vector256<uint> count)
	{
		throw null;
	}

	public static Vector256<long> ShiftRightLogicalVariable(Vector256<long> value, Vector256<ulong> count)
	{
		throw null;
	}

	public static Vector256<uint> ShiftRightLogicalVariable(Vector256<uint> value, Vector256<uint> count)
	{
		throw null;
	}

	public static Vector256<ulong> ShiftRightLogicalVariable(Vector256<ulong> value, Vector256<ulong> count)
	{
		throw null;
	}

	public static Vector256<byte> Shuffle(Vector256<byte> value, Vector256<byte> mask)
	{
		throw null;
	}

	public static Vector256<int> Shuffle(Vector256<int> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<sbyte> Shuffle(Vector256<sbyte> value, Vector256<sbyte> mask)
	{
		throw null;
	}

	public static Vector256<uint> Shuffle(Vector256<uint> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<short> ShuffleHigh(Vector256<short> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<ushort> ShuffleHigh(Vector256<ushort> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<short> ShuffleLow(Vector256<short> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<ushort> ShuffleLow(Vector256<ushort> value, [ConstantExpected] byte control)
	{
		throw null;
	}

	public static Vector256<short> Sign(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<int> Sign(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static Vector256<sbyte> Sign(Vector256<sbyte> left, Vector256<sbyte> right)
	{
		throw null;
	}

	public static Vector256<byte> Subtract(Vector256<byte> left, Vector256<byte> right)
	{
		throw null;
	}

	public static Vector256<short> Subtract(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<int> Subtract(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static Vector256<long> Subtract(Vector256<long> left, Vector256<long> right)
	{
		throw null;
	}

	public static Vector256<sbyte> Subtract(Vector256<sbyte> left, Vector256<sbyte> right)
	{
		throw null;
	}

	public static Vector256<ushort> Subtract(Vector256<ushort> left, Vector256<ushort> right)
	{
		throw null;
	}

	public static Vector256<uint> Subtract(Vector256<uint> left, Vector256<uint> right)
	{
		throw null;
	}

	public static Vector256<ulong> Subtract(Vector256<ulong> left, Vector256<ulong> right)
	{
		throw null;
	}

	public static Vector256<byte> SubtractSaturate(Vector256<byte> left, Vector256<byte> right)
	{
		throw null;
	}

	public static Vector256<short> SubtractSaturate(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<sbyte> SubtractSaturate(Vector256<sbyte> left, Vector256<sbyte> right)
	{
		throw null;
	}

	public static Vector256<ushort> SubtractSaturate(Vector256<ushort> left, Vector256<ushort> right)
	{
		throw null;
	}

	public static Vector256<ushort> SumAbsoluteDifferences(Vector256<byte> left, Vector256<byte> right)
	{
		throw null;
	}

	public static Vector256<byte> UnpackHigh(Vector256<byte> left, Vector256<byte> right)
	{
		throw null;
	}

	public static Vector256<short> UnpackHigh(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<int> UnpackHigh(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static Vector256<long> UnpackHigh(Vector256<long> left, Vector256<long> right)
	{
		throw null;
	}

	public static Vector256<sbyte> UnpackHigh(Vector256<sbyte> left, Vector256<sbyte> right)
	{
		throw null;
	}

	public static Vector256<ushort> UnpackHigh(Vector256<ushort> left, Vector256<ushort> right)
	{
		throw null;
	}

	public static Vector256<uint> UnpackHigh(Vector256<uint> left, Vector256<uint> right)
	{
		throw null;
	}

	public static Vector256<ulong> UnpackHigh(Vector256<ulong> left, Vector256<ulong> right)
	{
		throw null;
	}

	public static Vector256<byte> UnpackLow(Vector256<byte> left, Vector256<byte> right)
	{
		throw null;
	}

	public static Vector256<short> UnpackLow(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<int> UnpackLow(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static Vector256<long> UnpackLow(Vector256<long> left, Vector256<long> right)
	{
		throw null;
	}

	public static Vector256<sbyte> UnpackLow(Vector256<sbyte> left, Vector256<sbyte> right)
	{
		throw null;
	}

	public static Vector256<ushort> UnpackLow(Vector256<ushort> left, Vector256<ushort> right)
	{
		throw null;
	}

	public static Vector256<uint> UnpackLow(Vector256<uint> left, Vector256<uint> right)
	{
		throw null;
	}

	public static Vector256<ulong> UnpackLow(Vector256<ulong> left, Vector256<ulong> right)
	{
		throw null;
	}

	public static Vector256<byte> Xor(Vector256<byte> left, Vector256<byte> right)
	{
		throw null;
	}

	public static Vector256<short> Xor(Vector256<short> left, Vector256<short> right)
	{
		throw null;
	}

	public static Vector256<int> Xor(Vector256<int> left, Vector256<int> right)
	{
		throw null;
	}

	public static Vector256<long> Xor(Vector256<long> left, Vector256<long> right)
	{
		throw null;
	}

	public static Vector256<sbyte> Xor(Vector256<sbyte> left, Vector256<sbyte> right)
	{
		throw null;
	}

	public static Vector256<ushort> Xor(Vector256<ushort> left, Vector256<ushort> right)
	{
		throw null;
	}

	public static Vector256<uint> Xor(Vector256<uint> left, Vector256<uint> right)
	{
		throw null;
	}

	public static Vector256<ulong> Xor(Vector256<ulong> left, Vector256<ulong> right)
	{
		throw null;
	}
}
