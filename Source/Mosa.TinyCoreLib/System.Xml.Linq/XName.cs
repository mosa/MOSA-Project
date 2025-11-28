using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace System.Xml.Linq;

public sealed class XName : IEquatable<XName>, ISerializable
{
	public string LocalName
	{
		get
		{
			throw null;
		}
	}

	public XNamespace Namespace
	{
		get
		{
			throw null;
		}
	}

	public string NamespaceName
	{
		get
		{
			throw null;
		}
	}

	internal XName()
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public static XName Get(string expandedName)
	{
		throw null;
	}

	public static XName Get(string localName, string namespaceName)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(XName? left, XName? right)
	{
		throw null;
	}

	[CLSCompliant(false)]
	[return: NotNullIfNotNull("expandedName")]
	public static implicit operator XName?(string? expandedName)
	{
		throw null;
	}

	public static bool operator !=(XName? left, XName? right)
	{
		throw null;
	}

	bool IEquatable<XName>.Equals(XName? other)
	{
		throw null;
	}

	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
