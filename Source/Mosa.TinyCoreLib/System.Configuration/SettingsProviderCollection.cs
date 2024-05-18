using System.Configuration.Provider;

namespace System.Configuration;

public class SettingsProviderCollection : ProviderCollection
{
	public new SettingsProvider this[string name]
	{
		get
		{
			throw null;
		}
	}

	public override void Add(ProviderBase provider)
	{
	}
}
