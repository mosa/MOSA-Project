namespace System.Runtime.Versioning;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
public sealed class UnsupportedOSPlatformGuardAttribute : OSPlatformAttribute
{
	public UnsupportedOSPlatformGuardAttribute(string platformName)
		: base(platformName)
	{
	}
}
