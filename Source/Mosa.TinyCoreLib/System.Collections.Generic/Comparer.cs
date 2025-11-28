namespace System.Collections.Generic;

public abstract class Comparer<T> : IComparer<T>, IComparer
{
	public static Comparer<T> Default
	{
		get
		{
			throw null;
		}
	}

	public abstract int Compare(T? x, T? y);

	public static Comparer<T> Create(Comparison<T> comparison)
	{
		throw null;
	}

	int IComparer.Compare(object? x, object? y)
	{
		throw null;
	}
}
