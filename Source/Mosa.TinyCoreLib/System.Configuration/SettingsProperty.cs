namespace System.Configuration;

public class SettingsProperty
{
	public virtual SettingsAttributeDictionary Attributes
	{
		get
		{
			throw null;
		}
	}

	public virtual object DefaultValue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual bool IsReadOnly
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual string Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual Type PropertyType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual SettingsProvider Provider
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual SettingsSerializeAs SerializeAs
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool ThrowOnErrorDeserializing
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool ThrowOnErrorSerializing
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SettingsProperty(SettingsProperty propertyToCopy)
	{
	}

	public SettingsProperty(string name)
	{
	}

	public SettingsProperty(string name, Type propertyType, SettingsProvider provider, bool isReadOnly, object defaultValue, SettingsSerializeAs serializeAs, SettingsAttributeDictionary attributes, bool throwOnErrorDeserializing, bool throwOnErrorSerializing)
	{
	}
}
