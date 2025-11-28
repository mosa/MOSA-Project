using System.Xml;

namespace System.Security.Cryptography.Xml;

public class KeyInfoName : KeyInfoClause
{
	public string? Value
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public KeyInfoName()
	{
	}

	public KeyInfoName(string? keyName)
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
