using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace System.Net.Mime;

public class ContentType
{
	public string? Boundary
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? CharSet
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string MediaType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public StringDictionary Parameters
	{
		get
		{
			throw null;
		}
	}

	public ContentType()
	{
	}

	public ContentType(string contentType)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? rparam)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
