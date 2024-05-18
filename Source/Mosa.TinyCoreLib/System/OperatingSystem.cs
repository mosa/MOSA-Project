using System.ComponentModel;
using System.Runtime.Serialization;
using System.Runtime.Versioning;

namespace System;

public sealed class OperatingSystem : ICloneable, ISerializable
{
	public PlatformID Platform
	{
		get
		{
			throw null;
		}
	}

	public string ServicePack
	{
		get
		{
			throw null;
		}
	}

	public Version Version
	{
		get
		{
			throw null;
		}
	}

	public string VersionString
	{
		get
		{
			throw null;
		}
	}

	public OperatingSystem(PlatformID platform, Version version)
	{
	}

	public object Clone()
	{
		throw null;
	}

	[Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	public static bool IsAndroid()
	{
		throw null;
	}

	public static bool IsAndroidVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
	{
		throw null;
	}

	public static bool IsBrowser()
	{
		throw null;
	}

	public static bool IsWasi()
	{
		throw null;
	}

	public static bool IsFreeBSD()
	{
		throw null;
	}

	public static bool IsFreeBSDVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
	{
		throw null;
	}

	[SupportedOSPlatformGuard("maccatalyst")]
	public static bool IsIOS()
	{
		throw null;
	}

	[SupportedOSPlatformGuard("maccatalyst")]
	public static bool IsIOSVersionAtLeast(int major, int minor = 0, int build = 0)
	{
		throw null;
	}

	public static bool IsLinux()
	{
		throw null;
	}

	public static bool IsMacCatalyst()
	{
		throw null;
	}

	public static bool IsMacCatalystVersionAtLeast(int major, int minor = 0, int build = 0)
	{
		throw null;
	}

	public static bool IsMacOS()
	{
		throw null;
	}

	public static bool IsMacOSVersionAtLeast(int major, int minor = 0, int build = 0)
	{
		throw null;
	}

	public static bool IsOSPlatform(string platform)
	{
		throw null;
	}

	public static bool IsOSPlatformVersionAtLeast(string platform, int major, int minor = 0, int build = 0, int revision = 0)
	{
		throw null;
	}

	public static bool IsTvOS()
	{
		throw null;
	}

	public static bool IsTvOSVersionAtLeast(int major, int minor = 0, int build = 0)
	{
		throw null;
	}

	public static bool IsWatchOS()
	{
		throw null;
	}

	public static bool IsWatchOSVersionAtLeast(int major, int minor = 0, int build = 0)
	{
		throw null;
	}

	public static bool IsWindows()
	{
		throw null;
	}

	public static bool IsWindowsVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
