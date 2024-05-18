using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace System.Configuration;

public sealed class ConfigurationSectionCollection : NameObjectCollectionBase
{
	public override int Count
	{
		get
		{
			throw null;
		}
	}

	public ConfigurationSection this[int index]
	{
		get
		{
			throw null;
		}
	}

	public ConfigurationSection this[string name]
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

	internal ConfigurationSectionCollection()
	{
	}

	public void Add(string name, ConfigurationSection section)
	{
	}

	public void Clear()
	{
	}

	public void CopyTo(ConfigurationSection[] array, int index)
	{
	}

	public ConfigurationSection Get(int index)
	{
		throw null;
	}

	public ConfigurationSection Get(string name)
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
