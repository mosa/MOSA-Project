using System.Xml;

namespace System.Security.Cryptography.Xml;

public class RSAKeyValue : KeyInfoClause
{
	public RSA Key
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public RSAKeyValue()
	{
	}

	public RSAKeyValue(RSA key)
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
