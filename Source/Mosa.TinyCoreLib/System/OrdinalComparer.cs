using System.Diagnostics.CodeAnalysis;

namespace System;

public class OrdinalComparer : StringComparer
{
	internal OrdinalComparer()
	{
	}

	public override int Compare(string? x, string? y)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override bool Equals(string? x, string? y)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override int GetHashCode(string obj)
	{
		throw null;
	}
}
