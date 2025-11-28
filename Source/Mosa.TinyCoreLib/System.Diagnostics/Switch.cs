using System.Collections.Specialized;

namespace System.Diagnostics;

public abstract class Switch
{
	public StringDictionary Attributes
	{
		get
		{
			throw null;
		}
	}

	public string DefaultValue
	{
		get
		{
			throw null;
		}
	}

	public string Description
	{
		get
		{
			throw null;
		}
	}

	public string DisplayName
	{
		get
		{
			throw null;
		}
	}

	protected int SwitchSetting
	{
		get
		{
			throw null;
		}
		set
		{
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

	public static event EventHandler<InitializingSwitchEventArgs>? Initializing
	{
		add
		{
		}
		remove
		{
		}
	}

	protected Switch(string displayName, string? description)
	{
	}

	protected Switch(string displayName, string? description, string defaultSwitchValue)
	{
	}

	protected virtual string[]? GetSupportedAttributes()
	{
		throw null;
	}

	protected virtual void OnSwitchSettingChanged()
	{
	}

	protected virtual void OnValueChanged()
	{
	}

	public void Refresh()
	{
	}
}
