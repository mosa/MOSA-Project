using System.Xml;

namespace System.Configuration;

public sealed class SettingValueElement : ConfigurationElement
{
	protected override ConfigurationPropertyCollection Properties
	{
		get
		{
			throw null;
		}
	}

	public XmlNode ValueXml
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
	{
	}

	public override bool Equals(object settingValue)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	protected override bool IsModified()
	{
		throw null;
	}

	protected override void Reset(ConfigurationElement parentElement)
	{
	}

	protected override void ResetModified()
	{
	}

	protected override bool SerializeToXmlElement(XmlWriter writer, string elementName)
	{
		throw null;
	}

	protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
	{
	}
}
