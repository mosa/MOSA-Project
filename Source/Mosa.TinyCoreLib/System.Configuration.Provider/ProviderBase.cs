using System.Collections.Specialized;

namespace System.Configuration.Provider;

public abstract class ProviderBase
{
	public virtual string Description
	{
		get
		{
			throw null;
		}
	}

	public virtual string Name
	{
		get
		{
			throw null;
		}
	}

	public virtual void Initialize(string name, NameValueCollection config)
	{
	}
}
