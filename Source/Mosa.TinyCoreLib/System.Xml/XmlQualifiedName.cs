using System.Diagnostics.CodeAnalysis;

namespace System.Xml;

public class XmlQualifiedName
{
	public static readonly XmlQualifiedName Empty;

	public bool IsEmpty
	{
		get
		{
			throw null;
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
	}

	public string Namespace
	{
		get
		{
			throw null;
		}
	}

	public XmlQualifiedName()
	{
	}

	public XmlQualifiedName(string? name)
	{
	}

	public XmlQualifiedName(string? name, string? ns)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(XmlQualifiedName? a, XmlQualifiedName? b)
	{
		throw null;
	}

	public static bool operator !=(XmlQualifiedName? a, XmlQualifiedName? b)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public static string ToString(string name, string? ns)
	{
		throw null;
	}
}
