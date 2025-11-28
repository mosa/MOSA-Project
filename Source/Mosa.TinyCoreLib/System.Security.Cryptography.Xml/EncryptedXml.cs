using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Xml;

namespace System.Security.Cryptography.Xml;

public class EncryptedXml
{
	public const string XmlEncAES128KeyWrapUrl = "http://www.w3.org/2001/04/xmlenc#kw-aes128";

	public const string XmlEncAES128Url = "http://www.w3.org/2001/04/xmlenc#aes128-cbc";

	public const string XmlEncAES192KeyWrapUrl = "http://www.w3.org/2001/04/xmlenc#kw-aes192";

	public const string XmlEncAES192Url = "http://www.w3.org/2001/04/xmlenc#aes192-cbc";

	public const string XmlEncAES256KeyWrapUrl = "http://www.w3.org/2001/04/xmlenc#kw-aes256";

	public const string XmlEncAES256Url = "http://www.w3.org/2001/04/xmlenc#aes256-cbc";

	public const string XmlEncDESUrl = "http://www.w3.org/2001/04/xmlenc#des-cbc";

	public const string XmlEncElementContentUrl = "http://www.w3.org/2001/04/xmlenc#Content";

	public const string XmlEncElementUrl = "http://www.w3.org/2001/04/xmlenc#Element";

	public const string XmlEncEncryptedKeyUrl = "http://www.w3.org/2001/04/xmlenc#EncryptedKey";

	public const string XmlEncNamespaceUrl = "http://www.w3.org/2001/04/xmlenc#";

	public const string XmlEncRSA15Url = "http://www.w3.org/2001/04/xmlenc#rsa-1_5";

	public const string XmlEncRSAOAEPUrl = "http://www.w3.org/2001/04/xmlenc#rsa-oaep-mgf1p";

	public const string XmlEncSHA256Url = "http://www.w3.org/2001/04/xmlenc#sha256";

	public const string XmlEncSHA512Url = "http://www.w3.org/2001/04/xmlenc#sha512";

	public const string XmlEncTripleDESKeyWrapUrl = "http://www.w3.org/2001/04/xmlenc#kw-tripledes";

	public const string XmlEncTripleDESUrl = "http://www.w3.org/2001/04/xmlenc#tripledes-cbc";

	public Evidence? DocumentEvidence
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Encoding Encoding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CipherMode Mode
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public PaddingMode Padding
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string Recipient
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

	public XmlResolver? Resolver
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int XmlDSigSearchDepth
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
	public EncryptedXml()
	{
	}

	[RequiresDynamicCode("XmlDsigXsltTransform uses XslCompiledTransform which requires dynamic code.")]
	[RequiresUnreferencedCode("The algorithm implementations referenced in the XML payload might be removed. Ensure the required algorithm implementations are preserved in your application.")]
	public EncryptedXml(XmlDocument document)
	{
	}

	[RequiresDynamicCode("XmlDsigXsltTransform uses XslCompiledTransform which requires dynamic code.")]
	[RequiresUnreferencedCode("The algorithm implementations referenced in the XML payload might be removed. Ensure the required algorithm implementations are preserved in your application.")]
	public EncryptedXml(XmlDocument document, Evidence? evidence)
	{
	}

	public void AddKeyNameMapping(string keyName, object keyObject)
	{
	}

	public void ClearKeyNameMappings()
	{
	}

	public byte[] DecryptData(EncryptedData encryptedData, SymmetricAlgorithm symmetricAlgorithm)
	{
		throw null;
	}

	public void DecryptDocument()
	{
	}

	public virtual byte[]? DecryptEncryptedKey(EncryptedKey encryptedKey)
	{
		throw null;
	}

	public static byte[] DecryptKey(byte[] keyData, RSA rsa, bool useOAEP)
	{
		throw null;
	}

	public static byte[] DecryptKey(byte[] keyData, SymmetricAlgorithm symmetricAlgorithm)
	{
		throw null;
	}

	public EncryptedData Encrypt(XmlElement inputElement, X509Certificate2 certificate)
	{
		throw null;
	}

	public EncryptedData Encrypt(XmlElement inputElement, string keyName)
	{
		throw null;
	}

	public byte[] EncryptData(byte[] plaintext, SymmetricAlgorithm symmetricAlgorithm)
	{
		throw null;
	}

	public byte[] EncryptData(XmlElement inputElement, SymmetricAlgorithm symmetricAlgorithm, bool content)
	{
		throw null;
	}

	public static byte[] EncryptKey(byte[] keyData, RSA rsa, bool useOAEP)
	{
		throw null;
	}

	public static byte[] EncryptKey(byte[] keyData, SymmetricAlgorithm symmetricAlgorithm)
	{
		throw null;
	}

	public virtual byte[] GetDecryptionIV(EncryptedData encryptedData, string? symmetricAlgorithmUri)
	{
		throw null;
	}

	public virtual SymmetricAlgorithm? GetDecryptionKey(EncryptedData encryptedData, string? symmetricAlgorithmUri)
	{
		throw null;
	}

	public virtual XmlElement? GetIdElement(XmlDocument document, string idValue)
	{
		throw null;
	}

	public void ReplaceData(XmlElement inputElement, byte[] decryptedData)
	{
	}

	public static void ReplaceElement(XmlElement inputElement, EncryptedData encryptedData, bool content)
	{
	}
}
