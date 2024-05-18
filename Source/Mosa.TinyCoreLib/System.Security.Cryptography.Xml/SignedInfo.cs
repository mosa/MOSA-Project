using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace System.Security.Cryptography.Xml;

[RequiresDynamicCode("XmlDsigXsltTransform uses XslCompiledTransform which requires dynamic code.")]
[RequiresUnreferencedCode("The algorithm implementations referenced in the XML payload might be removed. Ensure the required algorithm implementations are preserved in your application.")]
public class SignedInfo : ICollection, IEnumerable
{
	public string CanonicalizationMethod
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

	public Transform CanonicalizationMethodObject
	{
		get
		{
			throw null;
		}
	}

	public int Count
	{
		get
		{
			throw null;
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

	public bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public bool IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	public ArrayList References
	{
		get
		{
			throw null;
		}
	}

	public string? SignatureLength
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? SignatureMethod
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public object SyncRoot
	{
		get
		{
			throw null;
		}
	}

	public void AddReference(Reference reference)
	{
	}

	public void CopyTo(Array array, int index)
	{
	}

	public IEnumerator GetEnumerator()
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
