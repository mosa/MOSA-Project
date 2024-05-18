using System.Diagnostics.CodeAnalysis;

namespace System.Globalization;

public sealed class SortKey
{
	public byte[] KeyData
	{
		get
		{
			throw null;
		}
	}

	public string OriginalString
	{
		get
		{
			throw null;
		}
	}

	internal SortKey()
	{
	}

	public static int Compare(SortKey sortkey1, SortKey sortkey2)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
