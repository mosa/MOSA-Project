using System.Reflection;

namespace System.ComponentModel;

public class LicenseContext : IServiceProvider
{
	public virtual LicenseUsageMode UsageMode
	{
		get
		{
			throw null;
		}
	}

	public virtual string? GetSavedLicenseKey(Type type, Assembly? resourceAssembly)
	{
		throw null;
	}

	public virtual object? GetService(Type type)
	{
		throw null;
	}

	public virtual void SetSavedLicenseKey(Type type, string key)
	{
	}
}
