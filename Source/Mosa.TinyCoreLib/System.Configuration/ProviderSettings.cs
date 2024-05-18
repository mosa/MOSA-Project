using System.Collections.Specialized;

namespace System.Configuration;

public sealed class ProviderSettings : ConfigurationElement
{
	public string Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public NameValueCollection Parameters
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

	public string Type
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ProviderSettings()
	{
	}

	public ProviderSettings(string name, string type)
	{
	}

	protected override bool IsModified()
	{
		throw null;
	}

	protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
	{
		throw null;
	}

	protected override void Reset(ConfigurationElement parentElement)
	{
	}

	protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
	{
	}
}
