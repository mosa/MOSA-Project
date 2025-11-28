using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace System.Xml.Schema;

[Obsolete("XmlSchemaCollection has been deprecated. Use System.Xml.Schema.XmlSchemaSet for schema compilation and validation instead.")]
public sealed class XmlSchemaCollection : ICollection, IEnumerable
{
	public int Count
	{
		get
		{
			throw null;
		}
	}

	public XmlSchema? this[string? ns]
	{
		get
		{
			throw null;
		}
	}

	public XmlNameTable NameTable
	{
		get
		{
			throw null;
		}
	}

	int ICollection.Count
	{
		get
		{
			throw null;
		}
	}

	bool ICollection.IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	object ICollection.SyncRoot
	{
		get
		{
			throw null;
		}
	}

	public event ValidationEventHandler ValidationEventHandler
	{
		add
		{
		}
		remove
		{
		}
	}

	public XmlSchemaCollection()
	{
	}

	public XmlSchemaCollection(XmlNameTable nametable)
	{
	}

	public XmlSchema? Add(string? ns, [StringSyntax("Uri")] string uri)
	{
		throw null;
	}

	public XmlSchema? Add(string? ns, XmlReader reader)
	{
		throw null;
	}

	public XmlSchema? Add(string? ns, XmlReader reader, XmlResolver? resolver)
	{
		throw null;
	}

	public XmlSchema? Add(XmlSchema schema)
	{
		throw null;
	}

	public XmlSchema? Add(XmlSchema schema, XmlResolver? resolver)
	{
		throw null;
	}

	public void Add(XmlSchemaCollection schema)
	{
	}

	public bool Contains(string? ns)
	{
		throw null;
	}

	public bool Contains(XmlSchema schema)
	{
		throw null;
	}

	public void CopyTo(XmlSchema[] array, int index)
	{
	}

	public XmlSchemaCollectionEnumerator GetEnumerator()
	{
		throw null;
	}

	void ICollection.CopyTo(Array array, int index)
	{
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
