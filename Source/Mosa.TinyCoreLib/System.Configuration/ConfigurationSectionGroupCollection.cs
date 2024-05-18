using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace System.Configuration;

public sealed class ConfigurationSectionGroupCollection : NameObjectCollectionBase
{
	public override int Count
	{
		get
		{
			throw null;
		}
	}

	public ConfigurationSectionGroup this[int index]
	{
		get
		{
			throw null;
		}
	}

	public ConfigurationSectionGroup this[string name]
	{
		get
		{
			throw null;
		}
	}

	public override KeysCollection Keys
	{
		get
		{
			throw null;
		}
	}

	internal ConfigurationSectionGroupCollection()
	{
	}

	public void Add(string name, ConfigurationSectionGroup sectionGroup)
	{
	}

	public void Clear()
	{
	}

	public void CopyTo(ConfigurationSectionGroup[] array, int index)
	{
	}

	public ConfigurationSectionGroup Get(int index)
	{
		throw null;
	}

	public ConfigurationSectionGroup Get(string name)
	{
		throw null;
	}

	public override IEnumerator GetEnumerator()
	{
		throw null;
	}

	public string GetKey(int index)
	{
		throw null;
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public void Remove(string name)
	{
	}

	public void RemoveAt(int index)
	{
	}
}
