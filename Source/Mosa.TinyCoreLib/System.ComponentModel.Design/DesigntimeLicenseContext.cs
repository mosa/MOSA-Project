using System.Reflection;

namespace System.ComponentModel.Design;

public class DesigntimeLicenseContext : LicenseContext
{
	public override LicenseUsageMode UsageMode
	{
		get
		{
			throw null;
		}
	}

	public override string? GetSavedLicenseKey(Type type, Assembly? resourceAssembly)
	{
		throw null;
	}

	public override void SetSavedLicenseKey(Type type, string key)
	{
	}
}
