using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System;

public abstract class StringComparer : IComparer<string?>, IEqualityComparer<string?>, IComparer, IEqualityComparer
{
	public static StringComparer CurrentCulture
	{
		get
		{
			throw null;
		}
	}

	public static StringComparer CurrentCultureIgnoreCase
	{
		get
		{
			throw null;
		}
	}

	public static StringComparer InvariantCulture
	{
		get
		{
			throw null;
		}
	}

	public static StringComparer InvariantCultureIgnoreCase
	{
		get
		{
			throw null;
		}
	}

	public static StringComparer Ordinal
	{
		get
		{
			throw null;
		}
	}

	public static StringComparer OrdinalIgnoreCase
	{
		get
		{
			throw null;
		}
	}

	public int Compare(object? x, object? y)
	{
		throw null;
	}

	public abstract int Compare(string? x, string? y);

	public static StringComparer Create(CultureInfo culture, bool ignoreCase)
	{
		throw null;
	}

	public static StringComparer Create(CultureInfo culture, CompareOptions options)
	{
		throw null;
	}

	public new bool Equals(object? x, object? y)
	{
		throw null;
	}

	public abstract bool Equals(string? x, string? y);

	public static StringComparer FromComparison(StringComparison comparisonType)
	{
		throw null;
	}

	public int GetHashCode(object obj)
	{
		throw null;
	}

	public abstract int GetHashCode(string obj);

	public static bool IsWellKnownCultureAwareComparer(IEqualityComparer<string?>? comparer, [NotNullWhen(true)] out CompareInfo? compareInfo, out CompareOptions compareOptions)
	{
		throw null;
	}

	public static bool IsWellKnownOrdinalComparer(IEqualityComparer<string?>? comparer, out bool ignoreCase)
	{
		throw null;
	}
}
