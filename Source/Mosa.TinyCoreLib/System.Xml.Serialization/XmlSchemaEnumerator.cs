using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;

namespace System.Xml.Serialization;

public class XmlSchemaEnumerator : IEnumerator<XmlSchema>, IEnumerator, IDisposable
{
	public XmlSchema Current
	{
		get
		{
			throw null;
		}
	}

	object IEnumerator.Current
	{
		get
		{
			throw null;
		}
	}

	public XmlSchemaEnumerator(XmlSchemas list)
	{
	}

	public void Dispose()
	{
	}

	public bool MoveNext()
	{
		throw null;
	}

	void IEnumerator.Reset()
	{
	}
}
