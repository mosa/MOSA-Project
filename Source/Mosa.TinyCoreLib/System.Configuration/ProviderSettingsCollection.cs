namespace System.Configuration;

[ConfigurationCollection(typeof(ProviderSettings))]
public sealed class ProviderSettingsCollection : ConfigurationElementCollection
{
	public ProviderSettings this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public new ProviderSettings this[string key]
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

	public void Add(ProviderSettings provider)
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

	public void Remove(string name)
	{
	}
}
