using System.Collections.Specialized;

namespace System.Configuration;

public class LocalFileSettingsProvider : SettingsProvider, IApplicationSettingsProvider
{
	public override string ApplicationName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
	{
		throw null;
	}

	public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection properties)
	{
		throw null;
	}

	public override void Initialize(string name, NameValueCollection values)
	{
	}

	public void Reset(SettingsContext context)
	{
	}

	public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection values)
	{
	}

	public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
	{
	}
}
