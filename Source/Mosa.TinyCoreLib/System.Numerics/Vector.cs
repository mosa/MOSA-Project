using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Numerics;

public static class Vector
{
	public static bool IsHardwareAccelerated
	{
		get
		{
			throw null;
		}
	}

	public static Vector<T> Abs<T>(Vector<T> value)
	{
		throw null;
	}

	public static Vector<T> Add<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<T> AndNot<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<TTo> As<TFrom, TTo>(this Vector<TFrom> vector)
	{
		throw null;
	}

	public static Vector<byte> AsVectorByte<T>(Vector<T> value)
	{
		throw null;
	}

	public static Vector<double> AsVectorDouble<T>(Vector<T> value)
	{
		throw null;
	}

	public static Vector<short> AsVectorInt16<T>(Vector<T> value)
	{
		throw null;
	}

	public static Vector<int> AsVectorInt32<T>(Vector<T> value)
	{
		throw null;
	}

	public static Vector<long> AsVectorInt64<T>(Vector<T> value)
	{
		throw null;
	}

	public static Vector<IntPtr> AsVectorNInt<T>(Vector<T> value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<UIntPtr> AsVectorNUInt<T>(Vector<T> value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<sbyte> AsVectorSByte<T>(Vector<T> value)
	{
		throw null;
	}

	public static Vector<float> AsVectorSingle<T>(Vector<T> value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<ushort> AsVectorUInt16<T>(Vector<T> value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<uint> AsVectorUInt32<T>(Vector<T> value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<ulong> AsVectorUInt64<T>(Vector<T> value)
	{
		throw null;
	}

	public static Vector<T> BitwiseAnd<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<T> BitwiseOr<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<double> Ceiling(Vector<double> value)
	{
		throw null;
	}

	public static Vector<float> Ceiling(Vector<float> value)
	{
		throw null;
	}

	public static Vector<float> ConditionalSelect(Vector<int> condition, Vector<float> left, Vector<float> right)
	{
		throw null;
	}

	public static Vector<double> ConditionalSelect(Vector<long> condition, Vector<double> left, Vector<double> right)
	{
		throw null;
	}

	public static Vector<T> ConditionalSelect<T>(Vector<T> condition, Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<double> ConvertToDouble(Vector<long> value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<double> ConvertToDouble(Vector<ulong> value)
	{
		throw null;
	}

	public static Vector<int> ConvertToInt32(Vector<float> value)
	{
		throw null;
	}

	public static Vector<long> ConvertToInt64(Vector<double> value)
	{
		throw null;
	}

	public static Vector<float> ConvertToSingle(Vector<int> value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<float> ConvertToSingle(Vector<uint> value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<uint> ConvertToUInt32(Vector<float> value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<ulong> ConvertToUInt64(Vector<double> value)
	{
		throw null;
	}

	public static Vector<T> Divide<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<T> Divide<T>(Vector<T> left, T right)
	{
		throw null;
	}

	public static T Dot<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<long> Equals(Vector<double> left, Vector<double> right)
	{
		throw null;
	}

	public static Vector<int> Equals(Vector<int> left, Vector<int> right)
	{
		throw null;
	}

	public static Vector<long> Equals(Vector<long> left, Vector<long> right)
	{
		throw null;
	}

	public static Vector<int> Equals(Vector<float> left, Vector<float> right)
	{
		throw null;
	}

	public static bool EqualsAll<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static bool EqualsAny<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<T> Equals<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<double> Floor(Vector<double> value)
	{
		throw null;
	}

	public static Vector<float> Floor(Vector<float> value)
	{
		throw null;
	}

	public static T GetElement<T>(this Vector<T> vector, int index)
	{
		throw null;
	}

	public static Vector<long> GreaterThan(Vector<double> left, Vector<double> right)
	{
		throw null;
	}

	public static Vector<int> GreaterThan(Vector<int> left, Vector<int> right)
	{
		throw null;
	}

	public static Vector<long> GreaterThan(Vector<long> left, Vector<long> right)
	{
		throw null;
	}

	public static Vector<int> GreaterThan(Vector<float> left, Vector<float> right)
	{
		throw null;
	}

	public static bool GreaterThanAll<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static bool GreaterThanAny<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<long> GreaterThanOrEqual(Vector<double> left, Vector<double> right)
	{
		throw null;
	}

	public static Vector<int> GreaterThanOrEqual(Vector<int> left, Vector<int> right)
	{
		throw null;
	}

	public static Vector<long> GreaterThanOrEqual(Vector<long> left, Vector<long> right)
	{
		throw null;
	}

	public static Vector<int> GreaterThanOrEqual(Vector<float> left, Vector<float> right)
	{
		throw null;
	}

	public static bool GreaterThanOrEqualAll<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static bool GreaterThanOrEqualAny<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<T> GreaterThanOrEqual<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<T> GreaterThan<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<long> LessThan(Vector<double> left, Vector<double> right)
	{
		throw null;
	}

	public static Vector<int> LessThan(Vector<int> left, Vector<int> right)
	{
		throw null;
	}

	public static Vector<long> LessThan(Vector<long> left, Vector<long> right)
	{
		throw null;
	}

	public static Vector<int> LessThan(Vector<float> left, Vector<float> right)
	{
		throw null;
	}

	public static bool LessThanAll<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static bool LessThanAny<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<long> LessThanOrEqual(Vector<double> left, Vector<double> right)
	{
		throw null;
	}

	public static Vector<int> LessThanOrEqual(Vector<int> left, Vector<int> right)
	{
		throw null;
	}

	public static Vector<long> LessThanOrEqual(Vector<long> left, Vector<long> right)
	{
		throw null;
	}

	public static Vector<int> LessThanOrEqual(Vector<float> left, Vector<float> right)
	{
		throw null;
	}

	public static bool LessThanOrEqualAll<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static bool LessThanOrEqualAny<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<T> LessThanOrEqual<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<T> LessThan<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static Vector<T> Load<T>(T* source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static Vector<T> LoadAligned<T>(T* source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static Vector<T> LoadAlignedNonTemporal<T>(T* source)
	{
		throw null;
	}

	public static Vector<T> LoadUnsafe<T>([In] ref T source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<T> LoadUnsafe<T>([In] ref T source, UIntPtr elementOffset)
	{
		throw null;
	}

	public static Vector<T> Max<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<T> Min<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<T> Multiply<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<T> Multiply<T>(Vector<T> left, T right)
	{
		throw null;
	}

	public static Vector<T> Multiply<T>(T left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<float> Narrow(Vector<double> low, Vector<double> high)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<sbyte> Narrow(Vector<short> low, Vector<short> high)
	{
		throw null;
	}

	public static Vector<short> Narrow(Vector<int> low, Vector<int> high)
	{
		throw null;
	}

	public static Vector<int> Narrow(Vector<long> low, Vector<long> high)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<byte> Narrow(Vector<ushort> low, Vector<ushort> high)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<ushort> Narrow(Vector<uint> low, Vector<uint> high)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<uint> Narrow(Vector<ulong> low, Vector<ulong> high)
	{
		throw null;
	}

	public static Vector<T> Negate<T>(Vector<T> value)
	{
		throw null;
	}

	public static Vector<T> OnesComplement<T>(Vector<T> value)
	{
		throw null;
	}

	public static Vector<byte> ShiftLeft(Vector<byte> value, int shiftCount)
	{
		throw null;
	}

	public static Vector<short> ShiftLeft(Vector<short> value, int shiftCount)
	{
		throw null;
	}

	public static Vector<int> ShiftLeft(Vector<int> value, int shiftCount)
	{
		throw null;
	}

	public static Vector<long> ShiftLeft(Vector<long> value, int shiftCount)
	{
		throw null;
	}

	public static Vector<IntPtr> ShiftLeft(Vector<IntPtr> value, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<UIntPtr> ShiftLeft(Vector<UIntPtr> value, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<sbyte> ShiftLeft(Vector<sbyte> value, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<ushort> ShiftLeft(Vector<ushort> value, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<uint> ShiftLeft(Vector<uint> value, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<ulong> ShiftLeft(Vector<ulong> value, int shiftCount)
	{
		throw null;
	}

	public static Vector<short> ShiftRightArithmetic(Vector<short> value, int shiftCount)
	{
		throw null;
	}

	public static Vector<int> ShiftRightArithmetic(Vector<int> value, int shiftCount)
	{
		throw null;
	}

	public static Vector<long> ShiftRightArithmetic(Vector<long> value, int shiftCount)
	{
		throw null;
	}

	public static Vector<IntPtr> ShiftRightArithmetic(Vector<IntPtr> value, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<sbyte> ShiftRightArithmetic(Vector<sbyte> value, int shiftCount)
	{
		throw null;
	}

	public static Vector<byte> ShiftRightLogical(Vector<byte> value, int shiftCount)
	{
		throw null;
	}

	public static Vector<short> ShiftRightLogical(Vector<short> value, int shiftCount)
	{
		throw null;
	}

	public static Vector<int> ShiftRightLogical(Vector<int> value, int shiftCount)
	{
		throw null;
	}

	public static Vector<long> ShiftRightLogical(Vector<long> value, int shiftCount)
	{
		throw null;
	}

	public static Vector<IntPtr> ShiftRightLogical(Vector<IntPtr> value, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<UIntPtr> ShiftRightLogical(Vector<UIntPtr> value, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<sbyte> ShiftRightLogical(Vector<sbyte> value, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<ushort> ShiftRightLogical(Vector<ushort> value, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<uint> ShiftRightLogical(Vector<uint> value, int shiftCount)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<ulong> ShiftRightLogical(Vector<ulong> value, int shiftCount)
	{
		throw null;
	}

	public static Vector<T> SquareRoot<T>(Vector<T> value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static void Store<T>(this Vector<T> source, T* destination)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static void StoreAligned<T>(this Vector<T> source, T* destination)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public unsafe static void StoreAlignedNonTemporal<T>(this Vector<T> source, T* destination)
	{
		throw null;
	}

	public static void StoreUnsafe<T>(this Vector<T> source, ref T destination)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static void StoreUnsafe<T>(this Vector<T> source, ref T destination, UIntPtr elementOffset)
	{
		throw null;
	}

	public static Vector<T> Subtract<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static T Sum<T>(Vector<T> value)
	{
		throw null;
	}

	public static T ToScalar<T>(this Vector<T> vector)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static void Widen(Vector<byte> source, out Vector<ushort> low, out Vector<ushort> high)
	{
		throw null;
	}

	public static void Widen(Vector<short> source, out Vector<int> low, out Vector<int> high)
	{
		throw null;
	}

	public static void Widen(Vector<int> source, out Vector<long> low, out Vector<long> high)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static void Widen(Vector<sbyte> source, out Vector<short> low, out Vector<short> high)
	{
		throw null;
	}

	public static void Widen(Vector<float> source, out Vector<double> low, out Vector<double> high)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static void Widen(Vector<ushort> source, out Vector<uint> low, out Vector<uint> high)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static void Widen(Vector<uint> source, out Vector<ulong> low, out Vector<ulong> high)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<ushort> WidenLower(Vector<byte> source)
	{
		throw null;
	}

	public static Vector<int> WidenLower(Vector<short> source)
	{
		throw null;
	}

	public static Vector<long> WidenLower(Vector<int> source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<short> WidenLower(Vector<sbyte> source)
	{
		throw null;
	}

	public static Vector<double> WidenLower(Vector<float> source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<uint> WidenLower(Vector<ushort> source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<ulong> WidenLower(Vector<uint> source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<ushort> WidenUpper(Vector<byte> source)
	{
		throw null;
	}

	public static Vector<int> WidenUpper(Vector<short> source)
	{
		throw null;
	}

	public static Vector<long> WidenUpper(Vector<int> source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<short> WidenUpper(Vector<sbyte> source)
	{
		throw null;
	}

	public static Vector<double> WidenUpper(Vector<float> source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<uint> WidenUpper(Vector<ushort> source)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static Vector<ulong> WidenUpper(Vector<uint> source)
	{
		throw null;
	}

	public static Vector<T> WithElement<T>(this Vector<T> vector, int index, T value)
	{
		throw null;
	}

	public static Vector<T> Xor<T>(Vector<T> left, Vector<T> right)
	{
		throw null;
	}
}
public readonly struct Vector<T> : IEquatable<Vector<T>>, IFormattable
{
	private readonly int _dummyPrimitive;

	public static Vector<T> AllBitsSet
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

	public T this[int index]
	{
		get
		{
			throw null;
		}
	}

	public static Vector<T> One
	{
		get
		{
			throw null;
		}
	}

	public static Vector<T> Zero
	{
		get
		{
			throw null;
		}
	}

	public Vector(ReadOnlySpan<byte> values)
	{
		throw null;
	}

	public Vector(ReadOnlySpan<T> values)
	{
		throw null;
	}

	public Vector(Span<T> values)
	{
		throw null;
	}

	public Vector(T value)
	{
		throw null;
	}

	public Vector(T[] values)
	{
		throw null;
	}

	public Vector(T[] values, int index)
	{
		throw null;
	}

	public void CopyTo(Span<byte> destination)
	{
	}

	public void CopyTo(Span<T> destination)
	{
	}

	public void CopyTo(T[] destination)
	{
	}

	public void CopyTo(T[] destination, int startIndex)
	{
	}

	public bool Equals(Vector<T> other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static Vector<T> operator +(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<T> operator &(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<T> operator |(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<T> operator /(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<T> operator /(Vector<T> left, T right)
	{
		throw null;
	}

	public static bool operator ==(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<T> operator ^(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static explicit operator Vector<byte>(Vector<T> value)
	{
		throw null;
	}

	public static explicit operator Vector<double>(Vector<T> value)
	{
		throw null;
	}

	public static explicit operator Vector<short>(Vector<T> value)
	{
		throw null;
	}

	public static explicit operator Vector<int>(Vector<T> value)
	{
		throw null;
	}

	public static explicit operator Vector<long>(Vector<T> value)
	{
		throw null;
	}

	public static explicit operator Vector<IntPtr>(Vector<T> value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator Vector<UIntPtr>(Vector<T> value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator Vector<sbyte>(Vector<T> value)
	{
		throw null;
	}

	public static explicit operator Vector<float>(Vector<T> value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator Vector<ushort>(Vector<T> value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator Vector<uint>(Vector<T> value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static explicit operator Vector<ulong>(Vector<T> value)
	{
		throw null;
	}

	public static bool operator !=(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<T> operator <<(Vector<T> value, int shiftCount)
	{
		throw null;
	}

	public static Vector<T> operator *(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<T> operator *(Vector<T> value, T factor)
	{
		throw null;
	}

	public static Vector<T> operator *(T factor, Vector<T> value)
	{
		throw null;
	}

	public static Vector<T> operator ~(Vector<T> value)
	{
		throw null;
	}

	public static Vector<T> operator >>(Vector<T> value, int shiftCount)
	{
		throw null;
	}

	public static Vector<T> operator -(Vector<T> left, Vector<T> right)
	{
		throw null;
	}

	public static Vector<T> operator -(Vector<T> value)
	{
		throw null;
	}

	public static Vector<T> operator +(Vector<T> value)
	{
		throw null;
	}

	public static Vector<T> operator >>>(Vector<T> value, int shiftCount)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public string ToString([StringSyntax("NumericFormat")] string? format)
	{
		throw null;
	}

	public string ToString([StringSyntax("NumericFormat")] string? format, IFormatProvider? formatProvider)
	{
		throw null;
	}

	public bool TryCopyTo(Span<byte> destination)
	{
		throw null;
	}

	public bool TryCopyTo(Span<T> destination)
	{
		throw null;
	}
}
