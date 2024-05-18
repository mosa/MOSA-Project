using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace System.Security.Cryptography.Xml;

public sealed class CipherReference : EncryptedReference
{
	public CipherReference()
	{
	}

	public CipherReference(string uri)
	{
	}

	public CipherReference(string uri, TransformChain transformChain)
	{
	}

	public override XmlElement GetXml()
	{
		throw null;
	}

	[RequiresDynamicCode("XmlDsigXsltTransform uses XslCompiledTransform which requires dynamic code.")]
	[RequiresUnreferencedCode("The algorithm implementations referenced in the XML payload might be removed. Ensure the required algorithm implementations are preserved in your application.")]
	public override void LoadXml(XmlElement value)
	{
	}
}
