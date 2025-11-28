namespace System.Collections.Generic;

public sealed class ReferenceEqualityComparer : IEqualityComparer<object?>, IEqualityComparer
{
	public static ReferenceEqualityComparer Instance
	{
		get
		{
			throw null;
		}
	}

	private ReferenceEqualityComparer()
	{
	}

	public new bool Equals(object? x, object? y)
	{
		throw null;
	}

	public int GetHashCode(object? obj)
	{
		throw null;
	}
}
