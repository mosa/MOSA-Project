using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace System.Security.Cryptography.Xml;

[RequiresDynamicCode("XmlDsigXsltTransform uses XslCompiledTransform which requires dynamic code.")]
[RequiresUnreferencedCode("The algorithm implementations referenced in the XML payload might be removed. Ensure the required algorithm implementations are preserved in your application.")]
public class KeyInfoEncryptedKey : KeyInfoClause
{
	public EncryptedKey? EncryptedKey
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public KeyInfoEncryptedKey()
	{
	}

	public KeyInfoEncryptedKey(EncryptedKey encryptedKey)
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
