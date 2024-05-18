namespace System.Xml;

public class XmlDocumentType : XmlLinkedNode
{
	public XmlNamedNodeMap Entities
	{
		get
		{
			throw null;
		}
	}

	public string? InternalSubset
	{
		get
		{
			throw null;
		}
	}

	public override bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public override string LocalName
	{
		get
		{
			throw null;
		}
	}

	public override string Name
	{
		get
		{
			throw null;
		}
	}

	public override XmlNodeType NodeType
	{
		get
		{
			throw null;
		}
	}

	public XmlNamedNodeMap Notations
	{
		get
		{
			throw null;
		}
	}

	public string? PublicId
	{
		get
		{
			throw null;
		}
	}

	public string? SystemId
	{
		get
		{
			throw null;
		}
	}

	protected internal XmlDocumentType(string name, string? publicId, string? systemId, string? internalSubset, XmlDocument doc)
	{
	}

	public override XmlNode CloneNode(bool deep)
	{
		throw null;
	}

	public override void WriteContentTo(XmlWriter w)
	{
	}

	public override void WriteTo(XmlWriter w)
	{
	}
}
