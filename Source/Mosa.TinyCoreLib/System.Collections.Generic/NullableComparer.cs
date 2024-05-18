using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace System.Collections.Generic;

public sealed class NullableComparer<T> : Comparer<T?>, ISerializable where T : struct
{
	public override int Compare(T? x, T? y)
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

	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
