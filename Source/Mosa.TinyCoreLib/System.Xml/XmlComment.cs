namespace System.Xml;

public class XmlComment : XmlCharacterData
{
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

	protected internal XmlComment(string? comment, XmlDocument doc)
		: base(null, null)
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
