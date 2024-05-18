using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;

namespace System.Security.Cryptography.Xml;

public class Reference
{
	public string DigestMethod
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte[]? DigestValue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

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

	public TransformChain TransformChain
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Type
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? Uri
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Reference()
	{
	}

	public Reference(Stream stream)
	{
	}

	public Reference(string? uri)
	{
	}

	public void AddTransform(Transform transform)
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
