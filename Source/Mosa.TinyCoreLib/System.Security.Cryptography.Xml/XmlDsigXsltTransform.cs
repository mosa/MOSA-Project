using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace System.Security.Cryptography.Xml;

[RequiresDynamicCode("XmlDsigXsltTransform uses XslCompiledTransform which requires dynamic code.")]
public class XmlDsigXsltTransform : Transform
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

	public XmlDsigXsltTransform()
	{
	}

	public XmlDsigXsltTransform(bool includeComments)
	{
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
