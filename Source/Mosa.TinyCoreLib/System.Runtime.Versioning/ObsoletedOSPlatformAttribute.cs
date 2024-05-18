namespace System.Runtime.Versioning;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
public sealed class ObsoletedOSPlatformAttribute : OSPlatformAttribute
{
	public string? Message
	{
		get
		{
			throw null;
		}
	}

	public string? Url
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ObsoletedOSPlatformAttribute(string platformName)
		: base(platformName)
	{
	}

	public ObsoletedOSPlatformAttribute(string platformName, string? message)
		: base(platformName)
	{
	}
}
