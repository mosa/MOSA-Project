using System.Diagnostics.CodeAnalysis;

namespace System.Xml;

public class XmlProcessingInstruction : XmlLinkedNode
{
	public string Data
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
		}
	}

	public override string InnerText
	{
		get
		{
			throw null;
		}
		[param: AllowNull]
		set
		{
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

	public string Target
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
		[param: AllowNull]
		set
		{
		}
	}

	protected internal XmlProcessingInstruction(string target, string? data, XmlDocument doc)
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
