using System.Xml.XPath;

namespace System.Xml.Schema;

public sealed class XmlAtomicValue : XPathItem, ICloneable
{
	public override bool IsNode
	{
		get
		{
			throw null;
		}
	}

	public override object TypedValue
	{
		get
		{
			throw null;
		}
	}

	public override string Value
	{
		get
		{
			throw null;
		}
	}

	public override bool ValueAsBoolean
	{
		get
		{
			throw null;
		}
	}

	public override DateTime ValueAsDateTime
	{
		get
		{
			throw null;
		}
	}

	public override double ValueAsDouble
	{
		get
		{
			throw null;
		}
	}

	public override int ValueAsInt
	{
		get
		{
			throw null;
		}
	}

	public override long ValueAsLong
	{
		get
		{
			throw null;
		}
	}

	public override Type ValueType
	{
		get
		{
			throw null;
		}
	}

	public override XmlSchemaType XmlType
	{
		get
		{
			throw null;
		}
	}

	internal XmlAtomicValue()
	{
	}

	public XmlAtomicValue Clone()
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

	public override object ValueAs(Type type, IXmlNamespaceResolver? nsResolver)
	{
		throw null;
	}
}
