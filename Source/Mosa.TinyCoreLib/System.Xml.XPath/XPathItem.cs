using System.Xml.Schema;

namespace System.Xml.XPath;

public abstract class XPathItem
{
	public abstract bool IsNode { get; }

	public abstract object TypedValue { get; }

	public abstract string Value { get; }

	public abstract bool ValueAsBoolean { get; }

	public abstract DateTime ValueAsDateTime { get; }

	public abstract double ValueAsDouble { get; }

	public abstract int ValueAsInt { get; }

	public abstract long ValueAsLong { get; }

	public abstract Type ValueType { get; }

	public abstract XmlSchemaType? XmlType { get; }

	public virtual object ValueAs(Type returnType)
	{
		throw null;
	}

	public abstract object ValueAs(Type returnType, IXmlNamespaceResolver? nsResolver);
}
