namespace System.Configuration;

public class KeyValueConfigurationElement : ConfigurationElement
{
	public string Key
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

	public string Value
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public KeyValueConfigurationElement(string key, string value)
	{
	}

	protected override void Init()
	{
	}
}
