using System.Collections;

namespace System.Xml.Schema;

public sealed class XmlSchemaCollectionEnumerator : IEnumerator
{
	public XmlSchema? Current
	{
		get
		{
			throw null;
		}
	}

	object? IEnumerator.Current
	{
		get
		{
			throw null;
		}
	}

	internal XmlSchemaCollectionEnumerator()
	{
	}

	public bool MoveNext()
	{
		throw null;
	}

	bool IEnumerator.MoveNext()
	{
		throw null;
	}

	void IEnumerator.Reset()
	{
	}
}
