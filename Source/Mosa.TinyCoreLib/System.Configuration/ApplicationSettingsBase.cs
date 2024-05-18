using System.ComponentModel;

namespace System.Configuration;

public abstract class ApplicationSettingsBase : SettingsBase, INotifyPropertyChanged
{
	[Browsable(false)]
	public override SettingsContext Context
	{
		get
		{
			throw null;
		}
	}

	public override object this[string propertyName]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Browsable(false)]
	public override SettingsPropertyCollection Properties
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	public override SettingsPropertyValueCollection PropertyValues
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	public override SettingsProviderCollection Providers
	{
		get
		{
			throw null;
		}
	}

	[Browsable(false)]
	public string SettingsKey
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public event PropertyChangedEventHandler PropertyChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	public event SettingChangingEventHandler SettingChanging
	{
		add
		{
		}
		remove
		{
		}
	}

	public event SettingsLoadedEventHandler SettingsLoaded
	{
		add
		{
		}
		remove
		{
		}
	}

	public event SettingsSavingEventHandler SettingsSaving
	{
		add
		{
		}
		remove
		{
		}
	}

	protected ApplicationSettingsBase()
	{
	}

	protected ApplicationSettingsBase(IComponent owner)
	{
	}

	protected ApplicationSettingsBase(IComponent owner, string settingsKey)
	{
	}

	protected ApplicationSettingsBase(string settingsKey)
	{
	}

	public object GetPreviousVersion(string propertyName)
	{
		throw null;
	}

	protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
	}

	protected virtual void OnSettingChanging(object sender, SettingChangingEventArgs e)
	{
	}

	protected virtual void OnSettingsLoaded(object sender, SettingsLoadedEventArgs e)
	{
	}

	protected virtual void OnSettingsSaving(object sender, CancelEventArgs e)
	{
	}

	public void Reload()
	{
	}

	public void Reset()
	{
	}

	public override void Save()
	{
	}

	public virtual void Upgrade()
	{
	}
}
