using System.Collections;

namespace System.Xml.Schema;

public class XmlSchemaObjectCollection : CollectionBase
{
	public virtual XmlSchemaObject this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public XmlSchemaObjectCollection()
	{
	}

	public XmlSchemaObjectCollection(XmlSchemaObject? parent)
	{
	}

	public int Add(XmlSchemaObject item)
	{
		throw null;
	}

	public bool Contains(XmlSchemaObject item)
	{
		throw null;
	}

	public void CopyTo(XmlSchemaObject[] array, int index)
	{
	}

	public new XmlSchemaObjectEnumerator GetEnumerator()
	{
		throw null;
	}

	public int IndexOf(XmlSchemaObject item)
	{
		throw null;
	}

	public void Insert(int index, XmlSchemaObject item)
	{
	}

	protected override void OnClear()
	{
	}

	protected override void OnInsert(int index, object? item)
	{
	}

	protected override void OnRemove(int index, object? item)
	{
	}

	protected override void OnSet(int index, object? oldValue, object? newValue)
	{
	}

	public void Remove(XmlSchemaObject item)
	{
	}
}
