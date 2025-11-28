using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public class EntityTagHeaderValue : ICloneable
{
	public static EntityTagHeaderValue Any
	{
		get
		{
			throw null;
		}
	}

	public bool IsWeak
	{
		get
		{
			throw null;
		}
	}

	public string Tag
	{
		get
		{
			throw null;
		}
	}

	public EntityTagHeaderValue(string tag)
	{
	}

	public EntityTagHeaderValue(string tag, bool isWeak)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static EntityTagHeaderValue Parse(string input)
	{
		throw null;
	}

	object ICloneable.Clone()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out EntityTagHeaderValue? parsedValue)
	{
		throw null;
	}
}
