using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Schema;

namespace System.Xml.Serialization;

public class XmlSchemas : CollectionBase, IEnumerable<XmlSchema>, IEnumerable
{
	public bool IsCompiled
	{
		get
		{
			throw null;
		}
	}

	public XmlSchema this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XmlSchema? this[string? ns]
	{
		get
		{
			throw null;
		}
	}

	public int Add(XmlSchema schema)
	{
		throw null;
	}

	public int Add(XmlSchema schema, Uri? baseUri)
	{
		throw null;
	}

	public void Add(XmlSchemas schemas)
	{
	}

	public void AddReference(XmlSchema schema)
	{
	}

	[RequiresUnreferencedCode("Members from serialized types may be trimmed if not referenced directly")]
	public void Compile(ValidationEventHandler? handler, bool fullCompile)
	{
	}

	public bool Contains(string? targetNamespace)
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

	public object? Find(XmlQualifiedName name, Type type)
	{
		throw null;
	}

	public IList GetSchemas(string? ns)
	{
		throw null;
	}

	public int IndexOf(XmlSchema schema)
	{
		throw null;
	}

	public void Insert(int index, XmlSchema schema)
	{
	}

	public static bool IsDataSet(XmlSchema schema)
	{
		throw null;
	}

	protected override void OnClear()
	{
	}

	protected override void OnInsert(int index, object? value)
	{
	}

	protected override void OnRemove(int index, object? value)
	{
	}

	protected override void OnSet(int index, object? oldValue, object? newValue)
	{
	}

	public void Remove(XmlSchema schema)
	{
	}

	IEnumerator<XmlSchema> IEnumerable<XmlSchema>.GetEnumerator()
	{
		throw null;
	}
}
