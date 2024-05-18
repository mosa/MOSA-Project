using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace System.Security.Cryptography.Xml;

[RequiresDynamicCode("XmlDsigXsltTransform uses XslCompiledTransform which requires dynamic code.")]
[RequiresUnreferencedCode("The algorithm implementations referenced in the XML payload might be removed. Ensure the required algorithm implementations are preserved in your application.")]
public class XmlLicenseTransform : Transform
{
	public IRelDecryptor? Decryptor
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
