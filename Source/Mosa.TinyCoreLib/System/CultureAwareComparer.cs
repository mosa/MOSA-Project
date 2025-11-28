using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace System;

public sealed class CultureAwareComparer : StringComparer, ISerializable
{
	internal CultureAwareComparer()
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

	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
