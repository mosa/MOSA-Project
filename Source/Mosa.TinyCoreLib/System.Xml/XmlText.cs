namespace System.Xml;

public class XmlText : XmlCharacterData
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

	public override XmlNode? ParentNode
	{
		get
		{
			throw null;
		}
	}

	public override XmlNode? PreviousText
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

	protected internal XmlText(string? strData, XmlDocument doc)
		: base(null, null)
	{
	}

	public override XmlNode CloneNode(bool deep)
	{
		throw null;
	}

	public virtual XmlText SplitText(int offset)
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
