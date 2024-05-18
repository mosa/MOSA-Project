using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace System.ComponentModel;

public sealed class LicenseManager
{
	public static LicenseContext CurrentContext
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public static LicenseUsageMode UsageMode
	{
		get
		{
			throw null;
		}
	}

	internal LicenseManager()
	{
	}

	[UnsupportedOSPlatform("browser")]
	public static object? CreateWithContext([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, LicenseContext creationContext)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static object? CreateWithContext([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type type, LicenseContext creationContext, object[] args)
	{
		throw null;
	}

	public static bool IsLicensed(Type type)
	{
		throw null;
	}

	public static bool IsValid(Type type)
	{
		throw null;
	}

	public static bool IsValid(Type type, object? instance, out License? license)
	{
		throw null;
	}

	public static void LockContext(object contextUser)
	{
	}

	public static void UnlockContext(object contextUser)
	{
	}

	public static void Validate(Type type)
	{
	}

	public static License? Validate(Type type, object? instance)
	{
		throw null;
	}
}
