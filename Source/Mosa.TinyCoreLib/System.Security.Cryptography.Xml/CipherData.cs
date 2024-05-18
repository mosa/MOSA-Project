using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace System.Security.Cryptography.Xml;

public sealed class CipherData
{
	public CipherReference? CipherReference
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

	public byte[]? CipherValue
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

	public CipherData()
	{
	}

	public CipherData(byte[] cipherValue)
	{
	}

	public CipherData(CipherReference cipherReference)
	{
	}

	public XmlElement GetXml()
	{
		throw null;
	}

	[RequiresDynamicCode("XmlDsigXsltTransform uses XslCompiledTransform which requires dynamic code.")]
	[RequiresUnreferencedCode("The algorithm implementations referenced in the XML payload might be removed. Ensure the required algorithm implementations are preserved in your application.")]
	public void LoadXml(XmlElement value)
	{
	}
}
