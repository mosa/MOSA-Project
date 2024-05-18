using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Collections.Generic;

public class NonRandomizedStringEqualityComparer : IEqualityComparer<string?>, ISerializable
{
	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected NonRandomizedStringEqualityComparer(SerializationInfo information, StreamingContext context)
	{
	}

	public virtual bool Equals(string? x, string? y)
	{
		throw null;
	}

	public virtual int GetHashCode(string? obj)
	{
		throw null;
	}

	public static IEqualityComparer<string>? GetStringComparer(object? comparer)
	{
		throw null;
	}

	public virtual IEqualityComparer<string?> GetUnderlyingEqualityComparer()
	{
		throw null;
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}
}
