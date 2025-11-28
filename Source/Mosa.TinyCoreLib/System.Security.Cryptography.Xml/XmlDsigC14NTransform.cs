using System.Xml;

namespace System.Security.Cryptography.Xml;

public class XmlDsigC14NTransform : Transform
{
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

	public XmlDsigC14NTransform()
	{
	}

	public XmlDsigC14NTransform(bool includeComments)
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
