using System.Xml;

namespace System.Security.Cryptography.Xml;

public class KeyInfoNode : KeyInfoClause
{
	public XmlElement? Value
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public KeyInfoNode()
	{
	}

	public KeyInfoNode(XmlElement node)
	{
	}

	public override XmlElement GetXml()
	{
		throw null;
	}

	public override void LoadXml(XmlElement value)
	{
	}
}
