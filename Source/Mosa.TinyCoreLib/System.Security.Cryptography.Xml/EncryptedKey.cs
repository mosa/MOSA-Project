using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace System.Security.Cryptography.Xml;

public sealed class EncryptedKey : EncryptedType
{
	public string? CarriedKeyName
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

	public ReferenceList ReferenceList
	{
		get
		{
			throw null;
		}
	}

	public void AddReference(DataReference dataReference)
	{
	}

	public void AddReference(KeyReference keyReference)
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
