using System.Diagnostics.CodeAnalysis;

namespace System.Xml.Linq;

public sealed class XNamespace
{
	public string NamespaceName
	{
		get
		{
			throw null;
		}
	}

	public static XNamespace None
	{
		get
		{
			throw null;
		}
	}

	public static XNamespace Xml
	{
		get
		{
			throw null;
		}
	}

	public static XNamespace Xmlns
	{
		get
		{
			throw null;
		}
	}

	internal XNamespace()
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public static XNamespace Get(string namespaceName)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public XName GetName(string localName)
	{
		throw null;
	}

	public static XName operator +(XNamespace ns, string localName)
	{
		throw null;
	}

	public static bool operator ==(XNamespace? left, XNamespace? right)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("namespaceName")]
	public static implicit operator XNamespace?(string? namespaceName)
	{
		throw null;
	}

	public static bool operator !=(XNamespace? left, XNamespace? right)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
