using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

namespace System.Security.Cryptography.Xml;

public class SignedXml
{
	protected Signature m_signature;

	protected string? m_strSigningKeyName;

	public const string XmlDecryptionTransformUrl = "http://www.w3.org/2002/07/decrypt#XML";

	public const string XmlDsigBase64TransformUrl = "http://www.w3.org/2000/09/xmldsig#base64";

	public const string XmlDsigC14NTransformUrl = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";

	public const string XmlDsigC14NWithCommentsTransformUrl = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315#WithComments";

	public const string XmlDsigCanonicalizationUrl = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";

	public const string XmlDsigCanonicalizationWithCommentsUrl = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315#WithComments";

	public const string XmlDsigDSAUrl = "http://www.w3.org/2000/09/xmldsig#dsa-sha1";

	public const string XmlDsigEnvelopedSignatureTransformUrl = "http://www.w3.org/2000/09/xmldsig#enveloped-signature";

	public const string XmlDsigExcC14NTransformUrl = "http://www.w3.org/2001/10/xml-exc-c14n#";

	public const string XmlDsigExcC14NWithCommentsTransformUrl = "http://www.w3.org/2001/10/xml-exc-c14n#WithComments";

	public const string XmlDsigHMACSHA1Url = "http://www.w3.org/2000/09/xmldsig#hmac-sha1";

	public const string XmlDsigMinimalCanonicalizationUrl = "http://www.w3.org/2000/09/xmldsig#minimal";

	public const string XmlDsigNamespaceUrl = "http://www.w3.org/2000/09/xmldsig#";

	public const string XmlDsigRSASHA1Url = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";

	public const string XmlDsigRSASHA256Url = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";

	public const string XmlDsigRSASHA384Url = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384";

	public const string XmlDsigRSASHA512Url = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512";

	public const string XmlDsigSHA1Url = "http://www.w3.org/2000/09/xmldsig#sha1";

	public const string XmlDsigSHA256Url = "http://www.w3.org/2001/04/xmlenc#sha256";

	public const string XmlDsigSHA384Url = "http://www.w3.org/2001/04/xmldsig-more#sha384";

	public const string XmlDsigSHA512Url = "http://www.w3.org/2001/04/xmlenc#sha512";

	public const string XmlDsigXPathTransformUrl = "http://www.w3.org/TR/1999/REC-xpath-19991116";

	public const string XmlDsigXsltTransformUrl = "http://www.w3.org/TR/1999/REC-xslt-19991116";

	public const string XmlLicenseTransformUrl = "urn:mpeg:mpeg21:2003:01-REL-R-NS:licenseTransform";

	public EncryptedXml EncryptedXml
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

	public XmlResolver Resolver
	{
		set
		{
		}
	}

	public Collection<string> SafeCanonicalizationMethods
	{
		get
		{
			throw null;
		}
	}

	public Signature Signature
	{
		get
		{
			throw null;
		}
	}

	public Func<SignedXml, bool> SignatureFormatValidator
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? SignatureLength
	{
		get
		{
			throw null;
		}
	}

	public string? SignatureMethod
	{
		get
		{
			throw null;
		}
	}

	public byte[]? SignatureValue
	{
		get
		{
			throw null;
		}
	}

	public SignedInfo? SignedInfo
	{
		get
		{
			throw null;
		}
	}

	public AsymmetricAlgorithm? SigningKey
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? SigningKeyName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[RequiresDynamicCode("XmlDsigXsltTransform uses XslCompiledTransform which requires dynamic code.")]
	[RequiresUnreferencedCode("The algorithm implementations referenced in the XML payload might be removed. Ensure the required algorithm implementations are preserved in your application.")]
	public SignedXml()
	{
	}

	[RequiresDynamicCode("XmlDsigXsltTransform uses XslCompiledTransform which requires dynamic code.")]
	[RequiresUnreferencedCode("The algorithm implementations referenced in the XML payload might be removed. Ensure the required algorithm implementations are preserved in your application.")]
	public SignedXml(XmlDocument document)
	{
	}

	[RequiresDynamicCode("XmlDsigXsltTransform uses XslCompiledTransform which requires dynamic code.")]
	[RequiresUnreferencedCode("The algorithm implementations referenced in the XML payload might be removed. Ensure the required algorithm implementations are preserved in your application.")]
	public SignedXml(XmlElement elem)
	{
	}

	public void AddObject(DataObject dataObject)
	{
	}

	public void AddReference(Reference reference)
	{
	}

	public bool CheckSignature()
	{
		throw null;
	}

	public bool CheckSignature(AsymmetricAlgorithm key)
	{
		throw null;
	}

	public bool CheckSignature(KeyedHashAlgorithm macAlg)
	{
		throw null;
	}

	public bool CheckSignature(X509Certificate2 certificate, bool verifySignatureOnly)
	{
		throw null;
	}

	public bool CheckSignatureReturningKey(out AsymmetricAlgorithm? signingKey)
	{
		throw null;
	}

	public void ComputeSignature()
	{
	}

	public void ComputeSignature(KeyedHashAlgorithm macAlg)
	{
	}

	public virtual XmlElement? GetIdElement(XmlDocument? document, string idValue)
	{
		throw null;
	}

	protected virtual AsymmetricAlgorithm? GetPublicKey()
	{
		throw null;
	}

	public XmlElement GetXml()
	{
		throw null;
	}

	public void LoadXml(XmlElement value)
	{
	}
}
