using System.Xml;

namespace System.Security.Cryptography.Xml;

public class KeyInfoRetrievalMethod : KeyInfoClause
{
	public string? Type
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Uri
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public KeyInfoRetrievalMethod()
	{
	}

	public KeyInfoRetrievalMethod(string? strUri)
	{
	}

	public KeyInfoRetrievalMethod(string strUri, string typeName)
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
