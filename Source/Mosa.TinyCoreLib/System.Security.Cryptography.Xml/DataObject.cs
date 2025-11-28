using System.Xml;

namespace System.Security.Cryptography.Xml;

public class DataObject
{
	public XmlNodeList Data
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Encoding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Id
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? MimeType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public DataObject()
	{
	}

	public DataObject(string id, string mimeType, string encoding, XmlElement data)
	{
	}

	public XmlElement GetXml()
	{
		throw null;
	}

	public void LoadXml(XmlElement value)
	{
	}
}
