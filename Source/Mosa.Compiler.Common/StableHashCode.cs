// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Common;

public struct StableHashCode
{
	private const uint OffsetBasis = 2166136261;
	private const uint Prime = 16777619;
	private uint hash = OffsetBasis;

	public StableHashCode()
	{
		hash = OffsetBasis;
	}

	public void Add(bool value) => Add(value ? 1 : 0);

	public void Add(byte value) => Add((uint)value);

	public void Add(short value) => Add(unchecked((uint)value));

	public void Add(ushort value) => Add((uint)value);

	public void Add(int value) => Add(unchecked((uint)value));

	public void Add(uint value)
	{
		hash ^= value;
		hash *= Prime;
	}

	public void Add(long value)
	{
		Add(unchecked((uint)value));
		Add(unchecked((uint)(value >> 32)));
	}

	public void Add(ulong value)
	{
		Add(unchecked((uint)value));
		Add(unchecked((uint)(value >> 32)));
	}

	public void Add(char value) => Add((ushort)value);

	public void Add(string value)
	{
		if (value is null)
		{
			Add(0u);
			return;
		}

		Add(value.Length);

		foreach (var ch in value)
		{
			Add(ch);
		}
	}

	public void Add<T>(T value)
	{
		if (value is null)
		{
			Add(0u);
			return;
		}

		switch (value)
		{
			case int intValue:
				Add(intValue);
				return;

			case uint uintValue:
				Add(uintValue);
				return;

			case long longValue:
				Add(longValue);
				return;

			case ulong ulongValue:
				Add(ulongValue);
				return;

			case short shortValue:
				Add(shortValue);
				return;

			case ushort ushortValue:
				Add(ushortValue);
				return;

			case byte byteValue:
				Add(byteValue);
				return;

			case bool boolValue:
				Add(boolValue);
				return;

			case char charValue:
				Add(charValue);
				return;

			case string stringValue:
				Add(stringValue);
				return;
		}

		Add(EqualityComparer<T>.Default.GetHashCode(value));
	}

	public void Add<T>(T value, IEqualityComparer<T>? comparer)
	{
		if (value is null)
		{
			Add(0u);
			return;
		}

		var hashCode = (comparer ?? EqualityComparer<T>.Default).GetHashCode(value);
		Add(hashCode);
	}

	public int ToHashCode() => unchecked((int)hash);

	public static int Combine<T1>(T1 value1)
	{
		var hash = new StableHashCode();
		hash.Add(value1);
		return hash.ToHashCode();
	}

	public static int Combine<T1, T2>(T1 value1, T2 value2)
	{
		var hash = new StableHashCode();
		hash.Add(value1);
		hash.Add(value2);
		return hash.ToHashCode();
	}

	public static int Combine<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
	{
		var hash = new StableHashCode();
		hash.Add(value1);
		hash.Add(value2);
		hash.Add(value3);
		return hash.ToHashCode();
	}

	public static int Combine<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4)
	{
		var hash = new StableHashCode();
		hash.Add(value1);
		hash.Add(value2);
		hash.Add(value3);
		hash.Add(value4);
		return hash.ToHashCode();
	}

	public static int Combine<T1, T2, T3, T4, T5>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5)
	{
		var hash = new StableHashCode();
		hash.Add(value1);
		hash.Add(value2);
		hash.Add(value3);
		hash.Add(value4);
		hash.Add(value5);
		return hash.ToHashCode();
	}

	public static int Combine<T1, T2, T3, T4, T5, T6>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6)
	{
		var hash = new StableHashCode();
		hash.Add(value1);
		hash.Add(value2);
		hash.Add(value3);
		hash.Add(value4);
		hash.Add(value5);
		hash.Add(value6);
		return hash.ToHashCode();
	}

	public static int Combine<T1, T2, T3, T4, T5, T6, T7>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7)
	{
		var hash = new StableHashCode();
		hash.Add(value1);
		hash.Add(value2);
		hash.Add(value3);
		hash.Add(value4);
		hash.Add(value5);
		hash.Add(value6);
		hash.Add(value7);
		return hash.ToHashCode();
	}

	public static int Combine<T1, T2, T3, T4, T5, T6, T7, T8>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8)
	{
		var hash = new StableHashCode();
		hash.Add(value1);
		hash.Add(value2);
		hash.Add(value3);
		hash.Add(value4);
		hash.Add(value5);
		hash.Add(value6);
		hash.Add(value7);
		hash.Add(value8);
		return hash.ToHashCode();
	}
}
