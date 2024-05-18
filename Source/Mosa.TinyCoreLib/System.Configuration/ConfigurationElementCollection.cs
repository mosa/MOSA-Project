using System.Collections;
using System.Xml;

namespace System.Configuration;

public abstract class ConfigurationElementCollection : ConfigurationElement, ICollection, IEnumerable
{
	protected string AddElementName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected string ClearElementName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual ConfigurationElementCollectionType CollectionType
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

	protected virtual string ElementName
	{
		get
		{
			throw null;
		}
	}

	public bool EmitClear
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	protected string RemoveElementName
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

	protected virtual bool ThrowOnDuplicate
	{
		get
		{
			throw null;
		}
	}

	protected ConfigurationElementCollection()
	{
	}

	protected ConfigurationElementCollection(IComparer comparer)
	{
	}

	protected virtual void BaseAdd(ConfigurationElement element)
	{
	}

	protected void BaseAdd(ConfigurationElement element, bool throwIfExists)
	{
	}

	protected virtual void BaseAdd(int index, ConfigurationElement element)
	{
	}

	protected void BaseClear()
	{
	}

	protected ConfigurationElement BaseGet(int index)
	{
		throw null;
	}

	protected ConfigurationElement BaseGet(object key)
	{
		throw null;
	}

	protected object[] BaseGetAllKeys()
	{
		throw null;
	}

	protected object BaseGetKey(int index)
	{
		throw null;
	}

	protected int BaseIndexOf(ConfigurationElement element)
	{
		throw null;
	}

	protected bool BaseIsRemoved(object key)
	{
		throw null;
	}

	protected void BaseRemove(object key)
	{
	}

	protected void BaseRemoveAt(int index)
	{
	}

	public void CopyTo(ConfigurationElement[] array, int index)
	{
	}

	protected abstract ConfigurationElement CreateNewElement();

	protected virtual ConfigurationElement CreateNewElement(string elementName)
	{
		throw null;
	}

	public override bool Equals(object compareTo)
	{
		throw null;
	}

	protected abstract object GetElementKey(ConfigurationElement element);

	public IEnumerator GetEnumerator()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	protected virtual bool IsElementName(string elementName)
	{
		throw null;
	}

	protected virtual bool IsElementRemovable(ConfigurationElement element)
	{
		throw null;
	}

	protected override bool IsModified()
	{
		throw null;
	}

	public override bool IsReadOnly()
	{
		throw null;
	}

	protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
	{
		throw null;
	}

	protected override void Reset(ConfigurationElement parentElement)
	{
	}

	protected override void ResetModified()
	{
	}

	protected override bool SerializeElement(XmlWriter writer, bool serializeCollectionKey)
	{
		throw null;
	}

	protected override void SetReadOnly()
	{
	}

	void ICollection.CopyTo(Array arr, int index)
	{
	}

	protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
	{
	}
}
