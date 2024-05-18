namespace System.Configuration;

public sealed class SettingElement : ConfigurationElement
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

	protected override ConfigurationPropertyCollection Properties
	{
		get
		{
			throw null;
		}
	}

	public SettingsSerializeAs SerializeAs
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SettingValueElement Value
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SettingElement()
	{
	}

	public SettingElement(string name, SettingsSerializeAs serializeAs)
	{
	}

	public override bool Equals(object settings)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}
}
