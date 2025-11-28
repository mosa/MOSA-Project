using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers;

public class ProductInfoHeaderValue : ICloneable
{
	public string? Comment
	{
		get
		{
			throw null;
		}
	}

	public ProductHeaderValue? Product
	{
		get
		{
			throw null;
		}
	}

	public ProductInfoHeaderValue(ProductHeaderValue product)
	{
	}

	public ProductInfoHeaderValue(string comment)
	{
	}

	public ProductInfoHeaderValue(string productName, string? productVersion)
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

	public static ProductInfoHeaderValue Parse(string input)
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

	public static bool TryParse([NotNullWhen(true)] string input, [NotNullWhen(true)] out ProductInfoHeaderValue? parsedValue)
	{
		throw null;
	}
}
