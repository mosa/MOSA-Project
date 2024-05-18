namespace System.Configuration;

[ConfigurationCollection(typeof(NameValueConfigurationElement))]
public sealed class NameValueConfigurationCollection : ConfigurationElementCollection
{
	public string[] AllKeys
	{
		get
		{
			throw null;
		}
	}

	public new NameValueConfigurationElement this[string name]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected override ConfigurationPropertyCollection Properties
	{
		get
		{
			throw null;
		}
	}

	public void Add(NameValueConfigurationElement nameValue)
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

	public void Remove(NameValueConfigurationElement nameValue)
	{
	}

	public void Remove(string name)
	{
	}
}
