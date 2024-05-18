using System.Diagnostics.CodeAnalysis;

namespace System.Runtime.Versioning;

public sealed class FrameworkName : IEquatable<FrameworkName?>
{
	public string FullName
	{
		get
		{
			throw null;
		}
	}

	public string Identifier
	{
		get
		{
			throw null;
		}
	}

	public string Profile
	{
		get
		{
			throw null;
		}
	}

	public Version Version
	{
		get
		{
			throw null;
		}
	}

	public FrameworkName(string frameworkName)
	{
	}

	public FrameworkName(string identifier, Version version)
	{
	}

	public FrameworkName(string identifier, Version version, string? profile)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals([NotNullWhen(true)] FrameworkName? other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(FrameworkName? left, FrameworkName? right)
	{
		throw null;
	}

	public static bool operator !=(FrameworkName? left, FrameworkName? right)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
