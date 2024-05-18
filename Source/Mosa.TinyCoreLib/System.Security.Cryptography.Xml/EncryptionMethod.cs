using System.Xml;

namespace System.Security.Cryptography.Xml;

public class EncryptionMethod
{
	public string? KeyAlgorithm
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int KeySize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public EncryptionMethod()
	{
	}

	public EncryptionMethod(string? algorithm)
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
