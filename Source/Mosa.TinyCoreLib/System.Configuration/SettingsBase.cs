using System.ComponentModel;

namespace System.Configuration;

public abstract class SettingsBase
{
	public virtual SettingsContext Context
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	public bool IsSynchronized
	{
		get
		{
			throw null;
		}
	}

	public virtual object this[string propertyName]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual SettingsPropertyCollection Properties
	{
		get
		{
			throw null;
		}
	}

	public virtual SettingsPropertyValueCollection PropertyValues
	{
		get
		{
			throw null;
		}
	}

	public virtual SettingsProviderCollection Providers
	{
		get
		{
			throw null;
		}
	}

	public void Initialize(SettingsContext context, SettingsPropertyCollection properties, SettingsProviderCollection providers)
	{
	}

	public virtual void Save()
	{
	}

	public static SettingsBase Synchronized(SettingsBase settingsBase)
	{
		throw null;
	}
}
