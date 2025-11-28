using System.Reflection;

namespace System.Runtime.InteropServices;

public static class RuntimeEnvironment
{
	[Obsolete("RuntimeEnvironment members SystemConfigurationFile, GetRuntimeInterfaceAsIntPtr, and GetRuntimeInterfaceAsObject are not supported and throw PlatformNotSupportedException.", DiagnosticId = "SYSLIB0019", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static string SystemConfigurationFile
	{
		get
		{
			throw null;
		}
	}

	public static bool FromGlobalAccessCache(Assembly a)
	{
		throw null;
	}

	public static string GetRuntimeDirectory()
	{
		throw null;
	}

	[Obsolete("RuntimeEnvironment members SystemConfigurationFile, GetRuntimeInterfaceAsIntPtr, and GetRuntimeInterfaceAsObject are not supported and throw PlatformNotSupportedException.", DiagnosticId = "SYSLIB0019", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static IntPtr GetRuntimeInterfaceAsIntPtr(Guid clsid, Guid riid)
	{
		throw null;
	}

	[Obsolete("RuntimeEnvironment members SystemConfigurationFile, GetRuntimeInterfaceAsIntPtr, and GetRuntimeInterfaceAsObject are not supported and throw PlatformNotSupportedException.", DiagnosticId = "SYSLIB0019", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static object GetRuntimeInterfaceAsObject(Guid clsid, Guid riid)
	{
		throw null;
	}

	public static string GetSystemVersion()
	{
		throw null;
	}
}
