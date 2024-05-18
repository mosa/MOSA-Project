namespace System.Configuration;

[ConfigurationCollection(typeof(ConnectionStringSettings))]
public sealed class ConnectionStringSettingsCollection : ConfigurationElementCollection
{
	public ConnectionStringSettings this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public new ConnectionStringSettings this[string name]
	{
		get
		{
			throw null;
		}
	}

	protected override ConfigurationPropertyCollection Properties
	{
		get
		{
			throw null;
		}
	}

	public void Add(ConnectionStringSettings settings)
	{
	}

	protected override void BaseAdd(int index, ConfigurationElement element)
	{
	}

	public void Clear()
	{
	}

	protected override ConfigurationElement CreateNewElement()
	{
		throw null;
	}

	protected override object GetElementKey(ConfigurationElement element)
	{
		throw null;
	}

	public int IndexOf(ConnectionStringSettings settings)
	{
		throw null;
	}

	public void Remove(ConnectionStringSettings settings)
	{
	}

	public void Remove(string name)
	{
	}

	public void RemoveAt(int index)
	{
	}
}
