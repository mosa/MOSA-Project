using System.Runtime.Versioning;
using System.Xml;

namespace System.Configuration;

public abstract class ConfigurationSection : ConfigurationElement
{
	public SectionInformation SectionInformation
	{
		get
		{
			throw null;
		}
	}

	protected virtual void DeserializeSection(XmlReader reader)
	{
	}

	protected virtual object GetRuntimeObject()
	{
		throw null;
	}

	protected override bool IsModified()
	{
		throw null;
	}

	protected override void ResetModified()
	{
	}

	protected virtual string SerializeSection(ConfigurationElement parentElement, string name, ConfigurationSaveMode saveMode)
	{
		throw null;
	}

	protected virtual bool ShouldSerializeElementInTargetVersion(ConfigurationElement element, string elementName, FrameworkName targetFramework)
	{
		throw null;
	}

	protected virtual bool ShouldSerializePropertyInTargetVersion(ConfigurationProperty property, string propertyName, FrameworkName targetFramework, ConfigurationElement parentConfigurationElement)
	{
		throw null;
	}

	protected virtual bool ShouldSerializeSectionInTargetVersion(FrameworkName targetFramework)
	{
		throw null;
	}
}
