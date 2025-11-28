using System.Xml;

namespace System.Security.Cryptography.Xml;

public class XmlDsigExcC14NTransform : Transform
{
	public string? InclusiveNamespacesPrefixList
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override Type[] InputTypes
	{
		get
		{
			throw null;
		}
	}

	public override Type[] OutputTypes
	{
		get
		{
			throw null;
		}
	}

	public XmlDsigExcC14NTransform()
	{
	}

	public XmlDsigExcC14NTransform(bool includeComments)
	{
	}

	public XmlDsigExcC14NTransform(bool includeComments, string? inclusiveNamespacesPrefixList)
	{
	}

	public XmlDsigExcC14NTransform(string inclusiveNamespacesPrefixList)
	{
	}

	public override byte[] GetDigestedOutput(HashAlgorithm hash)
	{
		throw null;
	}

	protected override XmlNodeList? GetInnerXml()
	{
		throw null;
	}

	public override object GetOutput()
	{
		throw null;
	}

	public override object GetOutput(Type type)
	{
		throw null;
	}

	public override void LoadInnerXml(XmlNodeList nodeList)
	{
	}

	public override void LoadInput(object obj)
	{
	}
}
