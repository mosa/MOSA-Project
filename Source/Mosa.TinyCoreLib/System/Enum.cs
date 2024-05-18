using System.Diagnostics.CodeAnalysis;

namespace System;

public abstract class Enum : ValueType, IComparable, IConvertible, ISpanFormattable, IFormattable
{
	public int CompareTo(object? target)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public static string Format(Type enumType, object value, [StringSyntax("EnumFormat")] string format)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static string? GetName(Type enumType, object value)
	{
		throw null;
	}

	public static string? GetName<TEnum>(TEnum value) where TEnum : struct, Enum
	{
		throw null;
	}

	public static string[] GetNames(Type enumType)
	{
		throw null;
	}

	public static string[] GetNames<TEnum>() where TEnum : struct, Enum
	{
		throw null;
	}

	public TypeCode GetTypeCode()
	{
		throw null;
	}

	public static Type GetUnderlyingType(Type enumType)
	{
		throw null;
	}

	[RequiresDynamicCode("It might not be possible to create an array of the enum type at runtime. Use the GetValues<TEnum> overload or the GetValuesAsUnderlyingType method instead.")]
	public static Array GetValues(Type enumType)
	{
		throw null;
	}

	public static TEnum[] GetValues<TEnum>() where TEnum : struct, Enum
	{
		throw null;
	}

	public static Array GetValuesAsUnderlyingType(Type enumType)
	{
		throw null;
	}

	public static Array GetValuesAsUnderlyingType<TEnum>() where TEnum : struct, Enum
	{
		throw null;
	}

	public bool HasFlag(Enum flag)
	{
		throw null;
	}

	public static bool IsDefined(Type enumType, object value)
	{
		throw null;
	}

	public static bool IsDefined<TEnum>(TEnum value) where TEnum : struct, Enum
	{
		throw null;
	}

	public static object Parse(Type enumType, ReadOnlySpan<char> value)
	{
		throw null;
	}

	public static object Parse(Type enumType, ReadOnlySpan<char> value, bool ignoreCase)
	{
		throw null;
	}

	public static object Parse(Type enumType, string value)
	{
		throw null;
	}

	public static object Parse(Type enumType, string value, bool ignoreCase)
	{
		throw null;
	}

	public static TEnum Parse<TEnum>(ReadOnlySpan<char> value) where TEnum : struct
	{
		throw null;
	}

	public static TEnum Parse<TEnum>(ReadOnlySpan<char> value, bool ignoreCase) where TEnum : struct
	{
		throw null;
	}

	public static TEnum Parse<TEnum>(string value) where TEnum : struct
	{
		throw null;
	}

	public static TEnum Parse<TEnum>(string value, bool ignoreCase) where TEnum : struct
	{
		throw null;
	}

	bool IConvertible.ToBoolean(IFormatProvider? provider)
	{
		throw null;
	}

	byte IConvertible.ToByte(IFormatProvider? provider)
	{
		throw null;
	}

	char IConvertible.ToChar(IFormatProvider? provider)
	{
		throw null;
	}

	DateTime IConvertible.ToDateTime(IFormatProvider? provider)
	{
		throw null;
	}

	decimal IConvertible.ToDecimal(IFormatProvider? provider)
	{
		throw null;
	}

	double IConvertible.ToDouble(IFormatProvider? provider)
	{
		throw null;
	}

	short IConvertible.ToInt16(IFormatProvider? provider)
	{
		throw null;
	}

	int IConvertible.ToInt32(IFormatProvider? provider)
	{
		throw null;
	}

	long IConvertible.ToInt64(IFormatProvider? provider)
	{
		throw null;
	}

	sbyte IConvertible.ToSByte(IFormatProvider? provider)
	{
		throw null;
	}

	float IConvertible.ToSingle(IFormatProvider? provider)
	{
		throw null;
	}

	object IConvertible.ToType(Type type, IFormatProvider? provider)
	{
		throw null;
	}

	ushort IConvertible.ToUInt16(IFormatProvider? provider)
	{
		throw null;
	}

	uint IConvertible.ToUInt32(IFormatProvider? provider)
	{
		throw null;
	}

	ulong IConvertible.ToUInt64(IFormatProvider? provider)
	{
		throw null;
	}

	public static object ToObject(Type enumType, byte value)
	{
		throw null;
	}

	public static object ToObject(Type enumType, short value)
	{
		throw null;
	}

	public static object ToObject(Type enumType, int value)
	{
		throw null;
	}

	public static object ToObject(Type enumType, long value)
	{
		throw null;
	}

	public static object ToObject(Type enumType, object value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static object ToObject(Type enumType, sbyte value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static object ToObject(Type enumType, ushort value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static object ToObject(Type enumType, uint value)
	{
		throw null;
	}

	[CLSCompliant(false)]
	public static object ToObject(Type enumType, ulong value)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	[Obsolete("The provider argument is not used. Use ToString() instead.")]
	public string ToString(IFormatProvider? provider)
	{
		throw null;
	}

	public string ToString([StringSyntax("EnumFormat")] string? format)
	{
		throw null;
	}

	[Obsolete("The provider argument is not used. Use ToString(String) instead.")]
	public string ToString([StringSyntax("EnumFormat")] string? format, IFormatProvider? provider)
	{
		throw null;
	}

	public static bool TryFormat<TEnum>(TEnum value, Span<char> destination, out int charsWritten, [StringSyntax("EnumFormat")] ReadOnlySpan<char> format = default(ReadOnlySpan<char>)) where TEnum : struct
	{
		throw null;
	}

	bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		throw null;
	}

	public static bool TryParse(Type enumType, ReadOnlySpan<char> value, bool ignoreCase, [NotNullWhen(true)] out object? result)
	{
		throw null;
	}

	public static bool TryParse(Type enumType, ReadOnlySpan<char> value, [NotNullWhen(true)] out object? result)
	{
		throw null;
	}

	public static bool TryParse(Type enumType, string? value, bool ignoreCase, [NotNullWhen(true)] out object? result)
	{
		throw null;
	}

	public static bool TryParse(Type enumType, string? value, [NotNullWhen(true)] out object? result)
	{
		throw null;
	}

	public static bool TryParse<TEnum>(ReadOnlySpan<char> value, bool ignoreCase, out TEnum result) where TEnum : struct
	{
		throw null;
	}

	public static bool TryParse<TEnum>(ReadOnlySpan<char> value, out TEnum result) where TEnum : struct
	{
		throw null;
	}

	public static bool TryParse<TEnum>([NotNullWhen(true)] string? value, bool ignoreCase, out TEnum result) where TEnum : struct
	{
		throw null;
	}

	public static bool TryParse<TEnum>([NotNullWhen(true)] string? value, out TEnum result) where TEnum : struct
	{
		throw null;
	}
}
