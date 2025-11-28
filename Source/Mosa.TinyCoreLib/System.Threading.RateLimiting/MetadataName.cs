using System.Diagnostics.CodeAnalysis;

namespace System.Threading.RateLimiting;

public static class MetadataName
{
	public static MetadataName<string> ReasonPhrase
	{
		get
		{
			throw null;
		}
	}

	public static MetadataName<TimeSpan> RetryAfter
	{
		get
		{
			throw null;
		}
	}

	public static MetadataName<T> Create<T>(string name)
	{
		throw null;
	}
}
public sealed class MetadataName<T> : IEquatable<MetadataName<T>>
{
	public string Name
	{
		get
		{
			throw null;
		}
	}

	public MetadataName(string name)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals(MetadataName<T>? other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(MetadataName<T> left, MetadataName<T> right)
	{
		throw null;
	}

	public static bool operator !=(MetadataName<T> left, MetadataName<T> right)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
