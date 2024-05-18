using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace System.Collections.Generic;

public sealed class EnumEqualityComparer<T> : EqualityComparer<T>, ISerializable where T : struct
{
	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override bool Equals(T x, T y)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override int GetHashCode(T obj)
	{
		throw null;
	}

	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
