using System.Xml;

namespace System.Configuration;

public sealed class IgnoreSection : ConfigurationSection
{
	protected override ConfigurationPropertyCollection Properties
	{
		get
		{
			throw null;
		}
	}

	protected override void DeserializeSection(XmlReader xmlReader)
	{
	}

	protected override bool IsModified()
	{
		throw null;
	}

	protected override void Reset(ConfigurationElement parentSection)
	{
	}

	protected override void ResetModified()
	{
	}

	protected override string SerializeSection(ConfigurationElement parentSection, string name, ConfigurationSaveMode saveMode)
	{
		throw null;
	}
}
