namespace System.Xml;

public class XmlEntityReference : XmlLinkedNode
{
	public override string BaseURI
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

	public override string? Value
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected internal XmlEntityReference(string name, XmlDocument doc)
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
