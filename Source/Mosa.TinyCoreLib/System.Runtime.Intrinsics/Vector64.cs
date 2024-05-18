using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Runtime.Intrinsics;

public static class Vector64
{
	public static bool IsHardwareAccelerated
	{
		get
		{
			throw null;
		}
	}

	public static Vector64<T> Abs<T>(Vector64<T> vector)
	{
		throw null;
	}

	public static Vector64<T> Add<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<T> AndNot<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<byte> AsByte<T>(this Vector64<T> vector)
	{
		throw null;
	}

	public static Vector64<double> AsDouble<T>(this Vector64<T> vector)
	{
		throw null;
	}

	public static Vector64<short> AsInt16<T>(this Vector64<T> vector)
	{
		throw null;
	}

	public static Vector64<int> AsInt32<T>(this Vector64<T> vector)
	{
		throw null;
	}

	public static Vector64<long> AsInt64<T>(this Vector64<T> vector)
	{
		throw null;
	}

	public static Vector64<IntPtr> AsNInt<T>(this Vector64<T> vector)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<UIntPtr> AsNUInt<T>(this Vector64<T> vector)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<sbyte> AsSByte<T>(this Vector64<T> vector)
	{
		throw null;
	}

	public static Vector64<float> AsSingle<T>(this Vector64<T> vector)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<ushort> AsUInt16<T>(this Vector64<T> vector)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<uint> AsUInt32<T>(this Vector64<T> vector)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<ulong> AsUInt64<T>(this Vector64<T> vector)
	{
		throw null;
	}

	public static Vector64<TTo> As<TFrom, TTo>(this Vector64<TFrom> vector)
	{
		throw null;
	}

	public static Vector64<T> BitwiseAnd<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<T> BitwiseOr<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<double> Ceiling(Vector64<double> vector)
	{
		throw null;
	}

	public static Vector64<float> Ceiling(Vector64<float> vector)
	{
		throw null;
	}

	public static Vector64<T> ConditionalSelect<T>(Vector64<T> condition, Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<double> ConvertToDouble(Vector64<long> vector)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<double> ConvertToDouble(Vector64<ulong> vector)
	{
		throw null;
	}

	public static Vector64<int> ConvertToInt32(Vector64<float> vector)
	{
		throw null;
	}

	public static Vector64<long> ConvertToInt64(Vector64<double> vector)
	{
		throw null;
	}

	public static Vector64<float> ConvertToSingle(Vector64<int> vector)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<float> ConvertToSingle(Vector64<uint> vector)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<uint> ConvertToUInt32(Vector64<float> vector)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<ulong> ConvertToUInt64(Vector64<double> vector)
	{
		throw null;
	}

	public static void CopyTo<T>(this Vector64<T> vector, Span<T> destination)
	{
	}

	public static void CopyTo<T>(this Vector64<T> vector, T[] destination)
	{
	}

	public static void CopyTo<T>(this Vector64<T> vector, T[] destination, int startIndex)
	{
	}

	public static Vector64<byte> Create(byte value)
	{
		throw null;
	}

	public static Vector64<byte> Create(byte e0, byte e1, byte e2, byte e3, byte e4, byte e5, byte e6, byte e7)
	{
		throw null;
	}

	public static Vector64<double> Create(double value)
	{
		throw null;
	}

	public static Vector64<short> Create(short value)
	{
		throw null;
	}

	public static Vector64<short> Create(short e0, short e1, short e2, short e3)
	{
		throw null;
	}

	public static Vector64<int> Create(int value)
	{
		throw null;
	}

	public static Vector64<int> Create(int e0, int e1)
	{
		throw null;
	}

	public static Vector64<long> Create(long value)
	{
		throw null;
	}

	public static Vector64<IntPtr> Create(IntPtr value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<UIntPtr> Create(UIntPtr value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<sbyte> Create(sbyte value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<sbyte> Create(sbyte e0, sbyte e1, sbyte e2, sbyte e3, sbyte e4, sbyte e5, sbyte e6, sbyte e7)
	{
		throw null;
	}

	public static Vector64<float> Create(float value)
	{
		throw null;
	}

	public static Vector64<float> Create(float e0, float e1)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<ushort> Create(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<ushort> Create(ushort e0, ushort e1, ushort e2, ushort e3)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<uint> Create(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<uint> Create(uint e0, uint e1)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<ulong> Create(ulong value)
	{
		throw null;
	}

	public static Vector64<byte> CreateScalar(byte value)
	{
		throw null;
	}

	public static Vector64<double> CreateScalar(double value)
	{
		throw null;
	}

	public static Vector64<short> CreateScalar(short value)
	{
		throw null;
	}

	public static Vector64<int> CreateScalar(int value)
	{
		throw null;
	}

	public static Vector64<long> CreateScalar(long value)
	{
		throw null;
	}

	public static Vector64<IntPtr> CreateScalar(IntPtr value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<UIntPtr> CreateScalar(UIntPtr value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<sbyte> CreateScalar(sbyte value)
	{
		throw null;
	}

	public static Vector64<float> CreateScalar(float value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<ushort> CreateScalar(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<uint> CreateScalar(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<ulong> CreateScalar(ulong value)
	{
		throw null;
	}

	public static Vector64<T> CreateScalar<T>(T value)
	{
		throw null;
	}

	public static Vector64<byte> CreateScalarUnsafe(byte value)
	{
		throw null;
	}

	public static Vector64<double> CreateScalarUnsafe(double value)
	{
		throw null;
	}

	public static Vector64<short> CreateScalarUnsafe(short value)
	{
		throw null;
	}

	public static Vector64<int> CreateScalarUnsafe(int value)
	{
		throw null;
	}

	public static Vector64<long> CreateScalarUnsafe(long value)
	{
		throw null;
	}

	public static Vector64<IntPtr> CreateScalarUnsafe(IntPtr value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<UIntPtr> CreateScalarUnsafe(UIntPtr value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<sbyte> CreateScalarUnsafe(sbyte value)
	{
		throw null;
	}

	public static Vector64<float> CreateScalarUnsafe(float value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<ushort> CreateScalarUnsafe(ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<uint> CreateScalarUnsafe(uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<ulong> CreateScalarUnsafe(ulong value)
	{
		throw null;
	}

	public static Vector64<T> CreateScalarUnsafe<T>(T value)
	{
		throw null;
	}

	public static Vector64<T> Create<T>(ReadOnlySpan<T> values)
	{
		throw null;
	}

	public static Vector64<T> Create<T>(T value)
	{
		throw null;
	}

	public static Vector64<T> Create<T>(T[] values)
	{
		throw null;
	}

	public static Vector64<T> Create<T>(T[] values, int index)
	{
		throw null;
	}

	public static Vector64<T> Divide<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<T> Divide<T>(Vector64<T> left, T right)
	{
		throw null;
	}

	public static T Dot<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static bool EqualsAll<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static bool EqualsAny<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<T> Equals<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static uint ExtractMostSignificantBits<T>(this Vector64<T> vector)
	{
		throw null;
	}

	public static Vector64<double> Floor(Vector64<double> vector)
	{
		throw null;
	}

	public static Vector64<float> Floor(Vector64<float> vector)
	{
		throw null;
	}

	public static T GetElement<T>(this Vector64<T> vector, int index)
	{
		throw null;
	}

	public static bool GreaterThanAll<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static bool GreaterThanAny<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static bool GreaterThanOrEqualAll<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static bool GreaterThanOrEqualAny<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<T> GreaterThanOrEqual<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<T> GreaterThan<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static bool LessThanAll<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static bool LessThanAny<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static bool LessThanOrEqualAll<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static bool LessThanOrEqualAny<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<T> LessThanOrEqual<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<T> LessThan<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static Vector64<T> Load<T>(T* source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static Vector64<T> LoadAligned<T>(T* source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static Vector64<T> LoadAlignedNonTemporal<T>(T* source)
	{
		throw null;
	}

	public static Vector64<T> LoadUnsafe<T>([In] ref T source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<T> LoadUnsafe<T>([In] ref T source, UIntPtr elementOffset)
	{
		throw null;
	}

	public static Vector64<T> Max<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<T> Min<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<T> Multiply<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<T> Multiply<T>(Vector64<T> left, T right)
	{
		throw null;
	}

	public static Vector64<T> Multiply<T>(T left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<float> Narrow(Vector64<double> lower, Vector64<double> upper)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<sbyte> Narrow(Vector64<short> lower, Vector64<short> upper)
	{
		throw null;
	}

	public static Vector64<short> Narrow(Vector64<int> lower, Vector64<int> upper)
	{
		throw null;
	}

	public static Vector64<int> Narrow(Vector64<long> lower, Vector64<long> upper)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<byte> Narrow(Vector64<ushort> lower, Vector64<ushort> upper)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<ushort> Narrow(Vector64<uint> lower, Vector64<uint> upper)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<uint> Narrow(Vector64<ulong> lower, Vector64<ulong> upper)
	{
		throw null;
	}

	public static Vector64<T> Negate<T>(Vector64<T> vector)
	{
		throw null;
	}

	public static Vector64<T> OnesComplement<T>(Vector64<T> vector)
	{
		throw null;
	}

	public static Vector64<byte> ShiftLeft(Vector64<byte> vector, int shiftCount)
	{
		throw null;
	}

	public static Vector64<short> ShiftLeft(Vector64<short> vector, int shiftCount)
	{
		throw null;
	}

	public static Vector64<int> ShiftLeft(Vector64<int> vector, int shiftCount)
	{
		throw null;
	}

	public static Vector64<long> ShiftLeft(Vector64<long> vector, int shiftCount)
	{
		throw null;
	}

	public static Vector64<IntPtr> ShiftLeft(Vector64<IntPtr> vector, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<UIntPtr> ShiftLeft(Vector64<UIntPtr> vector, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<sbyte> ShiftLeft(Vector64<sbyte> vector, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<ushort> ShiftLeft(Vector64<ushort> vector, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<uint> ShiftLeft(Vector64<uint> vector, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<ulong> ShiftLeft(Vector64<ulong> vector, int shiftCount)
	{
		throw null;
	}

	public static Vector64<short> ShiftRightArithmetic(Vector64<short> vector, int shiftCount)
	{
		throw null;
	}

	public static Vector64<int> ShiftRightArithmetic(Vector64<int> vector, int shiftCount)
	{
		throw null;
	}

	public static Vector64<long> ShiftRightArithmetic(Vector64<long> vector, int shiftCount)
	{
		throw null;
	}

	public static Vector64<IntPtr> ShiftRightArithmetic(Vector64<IntPtr> vector, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<sbyte> ShiftRightArithmetic(Vector64<sbyte> vector, int shiftCount)
	{
		throw null;
	}

	public static Vector64<byte> ShiftRightLogical(Vector64<byte> vector, int shiftCount)
	{
		throw null;
	}

	public static Vector64<short> ShiftRightLogical(Vector64<short> vector, int shiftCount)
	{
		throw null;
	}

	public static Vector64<int> ShiftRightLogical(Vector64<int> vector, int shiftCount)
	{
		throw null;
	}

	public static Vector64<long> ShiftRightLogical(Vector64<long> vector, int shiftCount)
	{
		throw null;
	}

	public static Vector64<IntPtr> ShiftRightLogical(Vector64<IntPtr> vector, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<UIntPtr> ShiftRightLogical(Vector64<UIntPtr> vector, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<sbyte> ShiftRightLogical(Vector64<sbyte> vector, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<ushort> ShiftRightLogical(Vector64<ushort> vector, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<uint> ShiftRightLogical(Vector64<uint> vector, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<ulong> ShiftRightLogical(Vector64<ulong> vector, int shiftCount)
	{
		throw null;
	}

	public static Vector64<byte> Shuffle(Vector64<byte> vector, Vector64<byte> indices)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<sbyte> Shuffle(Vector64<sbyte> vector, Vector64<sbyte> indices)
	{
		throw null;
	}

	public static Vector64<short> Shuffle(Vector64<short> vector, Vector64<short> indices)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<ushort> Shuffle(Vector64<ushort> vector, Vector64<ushort> indices)
	{
		throw null;
	}

	public static Vector64<int> Shuffle(Vector64<int> vector, Vector64<int> indices)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<uint> Shuffle(Vector64<uint> vector, Vector64<uint> indices)
	{
		throw null;
	}

	public static Vector64<float> Shuffle(Vector64<float> vector, Vector64<int> indices)
	{
		throw null;
	}

	public static Vector64<T> Sqrt<T>(Vector64<T> vector)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static void Store<T>(this Vector64<T> source, T* destination)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static void StoreAligned<T>(this Vector64<T> source, T* destination)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static void StoreAlignedNonTemporal<T>(this Vector64<T> source, T* destination)
	{
		throw null;
	}

	public static void StoreUnsafe<T>(this Vector64<T> source, ref T destination)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static void StoreUnsafe<T>(this Vector64<T> source, ref T destination, UIntPtr elementOffset)
	{
		throw null;
	}

	public static Vector64<T> Subtract<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static T Sum<T>(Vector64<T> vector)
	{
		throw null;
	}

	public static T ToScalar<T>(this Vector64<T> vector)
	{
		throw null;
	}

	public static Vector128<T> ToVector128Unsafe<T>(this Vector64<T> vector)
	{
		throw null;
	}

	public static Vector128<T> ToVector128<T>(this Vector64<T> vector)
	{
		throw null;
	}

	public static bool TryCopyTo<T>(this Vector64<T> vector, Span<T> destination)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static (Vector64<ushort> Lower, Vector64<ushort> Upper) Widen(Vector64<byte> source)
	{
		throw null;
	}

	public static (Vector64<int> Lower, Vector64<int> Upper) Widen(Vector64<short> source)
	{
		throw null;
	}

	public static (Vector64<long> Lower, Vector64<long> Upper) Widen(Vector64<int> source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static (Vector64<short> Lower, Vector64<short> Upper) Widen(Vector64<sbyte> source)
	{
		throw null;
	}

	public static (Vector64<double> Lower, Vector64<double> Upper) Widen(Vector64<float> source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static (Vector64<uint> Lower, Vector64<uint> Upper) Widen(Vector64<ushort> source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static (Vector64<ulong> Lower, Vector64<ulong> Upper) Widen(Vector64<uint> source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<ushort> WidenLower(Vector64<byte> source)
	{
		throw null;
	}

	public static Vector64<int> WidenLower(Vector64<short> source)
	{
		throw null;
	}

	public static Vector64<long> WidenLower(Vector64<int> source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<short> WidenLower(Vector64<sbyte> source)
	{
		throw null;
	}

	public static Vector64<double> WidenLower(Vector64<float> source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<uint> WidenLower(Vector64<ushort> source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<ulong> WidenLower(Vector64<uint> source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<ushort> WidenUpper(Vector64<byte> source)
	{
		throw null;
	}

	public static Vector64<int> WidenUpper(Vector64<short> source)
	{
		throw null;
	}

	public static Vector64<long> WidenUpper(Vector64<int> source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<short> WidenUpper(Vector64<sbyte> source)
	{
		throw null;
	}

	public static Vector64<double> WidenUpper(Vector64<float> source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<uint> WidenUpper(Vector64<ushort> source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector64<ulong> WidenUpper(Vector64<uint> source)
	{
		throw null;
	}

	public static Vector64<T> WithElement<T>(this Vector64<T> vector, int index, T value)
	{
		throw null;
	}

	public static Vector64<T> Xor<T>(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}
}
public readonly struct Vector64<T> : IEquatable<Vector64<T>>
{
	private readonly int _dummyPrimitive;

	public static Vector64<T> AllBitsSet
	{
		get
		{
			throw null;
		}
	}

	public static int Count
	{
		get
		{
			throw null;
		}
	}

	public static bool IsSupported
	{
		get
		{
			throw null;
		}
	}

	public static Vector64<T> One
	{
		get
		{
			throw null;
		}
	}

	public static Vector64<T> Zero
	{
		get
		{
			throw null;
		}
	}

	public T this[int index]
	{
		get
		{
			throw null;
		}
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(Vector64<T> other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static Vector64<T> operator +(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<T> operator &(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<T> operator |(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<T> operator /(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<T> operator /(Vector64<T> left, T right)
	{
		throw null;
	}

	public static bool operator ==(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<T> operator ^(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static bool operator !=(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<T> operator <<(Vector64<T> value, int shiftCount)
	{
		throw null;
	}

	public static Vector64<T> operator *(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<T> operator *(Vector64<T> left, T right)
	{
		throw null;
	}

	public static Vector64<T> operator *(T left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<T> operator ~(Vector64<T> vector)
	{
		throw null;
	}

	public static Vector64<T> operator >>(Vector64<T> value, int shiftCount)
	{
		throw null;
	}

	public static Vector64<T> operator -(Vector64<T> left, Vector64<T> right)
	{
		throw null;
	}

	public static Vector64<T> operator -(Vector64<T> vector)
	{
		throw null;
	}

	public static Vector64<T> operator +(Vector64<T> value)
	{
		throw null;
	}

	public static Vector64<T> operator >>>(Vector64<T> value, int shiftCount)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
