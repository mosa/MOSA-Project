using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace System.Security.Cryptography.Xml;

public class Signature
{
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

	public KeyInfo KeyInfo
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public IList ObjectList
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte[]? SignatureValue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SignedInfo? SignedInfo
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public void AddObject(DataObject dataObject)
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
