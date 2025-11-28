using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public class ProductHeaderValue : ICloneable
{
	public string Name
	{
		get
		{
			throw null;
		}
	}

	public string? Version
	{
		get
		{
			throw null;
		}
	}

	public ProductHeaderValue(string name)
	{
	}

	public ProductHeaderValue(string name, string? version)
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

	public static ProductHeaderValue Parse(string input)
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

	public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out ProductHeaderValue? parsedValue)
	{
		throw null;
	}
}
