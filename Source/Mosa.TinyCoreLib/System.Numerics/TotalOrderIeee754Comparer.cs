using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace System.Numerics;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly struct TotalOrderIeee754Comparer<T> : IComparer<T>, IEqualityComparer<T>, IEquatable<TotalOrderIeee754Comparer<T>> where T : IFloatingPointIeee754<T>?
{
	public int Compare(T? x, T? y)
	{
		throw null;
	}

	public bool Equals(TotalOrderIeee754Comparer<T> other)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(T? x, T? y)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public int GetHashCode([DisallowNull] T obj)
	{
		throw null;
	}
}
