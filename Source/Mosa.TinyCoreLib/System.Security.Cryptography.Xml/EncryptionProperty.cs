using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace System.Security.Cryptography.Xml;

public sealed class EncryptionProperty
{
	public string? Id
	{
		get
		{
			throw null;
		}
	}

	public XmlElement? PropertyElement
	{
		get
		{
			throw null;
		}
		[param: DisallowNull]
		set
		{
		}
	}

	public string? Target
	{
		get
		{
			throw null;
		}
	}

	public EncryptionProperty()
	{
	}

	public EncryptionProperty(XmlElement elementProperty)
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
