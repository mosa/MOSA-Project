using System.Configuration.Provider;

namespace System.Configuration;

public class ProtectedConfigurationProviderCollection : ProviderCollection
{
	public new ProtectedConfigurationProvider this[string name]
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
