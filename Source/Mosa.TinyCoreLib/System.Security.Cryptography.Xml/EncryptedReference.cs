using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace System.Security.Cryptography.Xml;

public abstract class EncryptedReference
{
	protected internal bool CacheValid
	{
		get
		{
			throw null;
		}
	}

	protected string? ReferenceType
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

	public string Uri
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected EncryptedReference()
	{
	}

	protected EncryptedReference(string uri)
	{
	}

	protected EncryptedReference(string uri, TransformChain transformChain)
	{
	}

	public void AddTransform(Transform transform)
	{
	}

	public virtual XmlElement GetXml()
	{
		throw null;
	}

	[RequiresDynamicCode("XmlDsigXsltTransform uses XslCompiledTransform which requires dynamic code.")]
	[RequiresUnreferencedCode("The algorithm implementations referenced in the XML payload might be removed. Ensure the required algorithm implementations are preserved in your application.")]
	public virtual void LoadXml(XmlElement value)
	{
	}
}
