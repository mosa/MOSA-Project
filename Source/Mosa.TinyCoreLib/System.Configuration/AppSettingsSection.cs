using System.Xml;

namespace System.Configuration;

public sealed class AppSettingsSection : ConfigurationSection
{
	public string File
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

	public KeyValueConfigurationCollection Settings
	{
		get
		{
			throw null;
		}
	}

	protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
	{
	}

	protected override object GetRuntimeObject()
	{
		throw null;
	}

	protected override bool IsModified()
	{
		throw null;
	}

	protected override void Reset(ConfigurationElement parentSection)
	{
	}

	protected override string SerializeSection(ConfigurationElement parentElement, string name, ConfigurationSaveMode saveMode)
	{
		throw null;
	}
}
