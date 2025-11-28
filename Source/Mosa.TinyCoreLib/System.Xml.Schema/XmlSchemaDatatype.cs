namespace System.Xml.Schema;

public abstract class XmlSchemaDatatype
{
	public abstract XmlTokenizedType TokenizedType { get; }

	public virtual XmlTypeCode TypeCode
	{
		get
		{
			throw null;
		}
	}

	public abstract Type ValueType { get; }

	public virtual XmlSchemaDatatypeVariety Variety
	{
		get
		{
			throw null;
		}
	}

	internal XmlSchemaDatatype()
	{
	}

	public virtual object ChangeType(object value, Type targetType)
	{
		throw null;
	}

	public virtual object ChangeType(object value, Type targetType, IXmlNamespaceResolver namespaceResolver)
	{
		throw null;
	}

	public virtual bool IsDerivedFrom(XmlSchemaDatatype datatype)
	{
		throw null;
	}

	public abstract object ParseValue(string s, XmlNameTable? nameTable, IXmlNamespaceResolver? nsmgr);
}
