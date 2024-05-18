using System.Diagnostics.CodeAnalysis;

namespace System.Globalization;

public sealed class SortVersion : IEquatable<SortVersion?>
{
	public int FullVersion
	{
		get
		{
			throw null;
		}
	}

	public Guid SortId
	{
		get
		{
			throw null;
		}
	}

	public SortVersion(int fullVersion, Guid sortId)
	{
	}

	public bool Equals([NotNullWhen(true)] SortVersion? other)
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

	public static bool operator ==(SortVersion? left, SortVersion? right)
	{
		throw null;
	}

	public static bool operator !=(SortVersion? left, SortVersion? right)
	{
		throw null;
	}
}
