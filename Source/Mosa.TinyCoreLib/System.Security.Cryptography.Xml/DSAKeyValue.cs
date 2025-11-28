using System.Runtime.Versioning;
using System.Xml;

namespace System.Security.Cryptography.Xml;

public class DSAKeyValue : KeyInfoClause
{
	public DSA Key
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public DSAKeyValue()
	{
	}

	public DSAKeyValue(DSA key)
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
