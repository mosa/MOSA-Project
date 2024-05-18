using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace System.Security.Cryptography.Xml;

public abstract class EncryptedType
{
	public virtual CipherData CipherData
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual string? Encoding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual EncryptionMethod? EncryptionMethod
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual EncryptionPropertyCollection EncryptionProperties
	{
		get
		{
			throw null;
		}
	}

	public virtual string? Id
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
		[param: AllowNull]
		set
		{
		}
	}

	public virtual string? MimeType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual string? Type
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public void AddProperty(EncryptionProperty ep)
	{
	}

	public abstract XmlElement GetXml();

	[RequiresDynamicCode("XmlDsigXsltTransform uses XslCompiledTransform which requires dynamic code.")]
	[RequiresUnreferencedCode("The algorithm implementations referenced in the XML payload might be removed. Ensure the required algorithm implementations are preserved in your application.")]
	public abstract void LoadXml(XmlElement value);
}
