using System.Diagnostics.CodeAnalysis;

namespace System.Xml;

public class XmlDeclaration : XmlLinkedNode
{
	public string Encoding
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

	public string Standalone
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

	public string Version
	{
		get
		{
			throw null;
		}
	}

	protected internal XmlDeclaration(string version, string? encoding, string? standalone, XmlDocument doc)
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
