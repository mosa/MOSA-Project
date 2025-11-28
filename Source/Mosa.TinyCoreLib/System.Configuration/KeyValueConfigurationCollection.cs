namespace System.Configuration;

[ConfigurationCollection(typeof(KeyValueConfigurationElement))]
public class KeyValueConfigurationCollection : ConfigurationElementCollection
{
	public string[] AllKeys
	{
		get
		{
			throw null;
		}
	}

	public new KeyValueConfigurationElement this[string key]
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

	protected override bool ThrowOnDuplicate
	{
		get
		{
			throw null;
		}
	}

	public void Add(KeyValueConfigurationElement keyValue)
	{
	}

	public void Add(string key, string value)
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

	public void Remove(string key)
	{
	}
}
