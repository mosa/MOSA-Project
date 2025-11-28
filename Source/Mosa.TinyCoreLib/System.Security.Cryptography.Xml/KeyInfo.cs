using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace System.Security.Cryptography.Xml;

public class KeyInfo : IEnumerable
{
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

	public void AddClause(KeyInfoClause clause)
	{
	}

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	public IEnumerator GetEnumerator(Type requestedObjectType)
	{
		throw null;
	}

	public XmlElement GetXml()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The algorithm implementations referenced in the XML payload might be removed. Ensure the required algorithm implementations are preserved in your application.")]
	public void LoadXml(XmlElement value)
	{
	}
}
