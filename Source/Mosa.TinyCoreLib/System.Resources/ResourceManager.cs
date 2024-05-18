using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace System.Resources;

public class ResourceManager
{
	public static readonly int HeaderVersionNumber;

	public static readonly int MagicNumber;

	protected Assembly? MainAssembly;

	public virtual string BaseName
	{
		get
		{
			throw null;
		}
	}

	protected UltimateResourceFallbackLocation FallbackLocation
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public virtual bool IgnoreCase
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)]
	public virtual Type ResourceSetType
	{
		get
		{
			throw null;
		}
	}

	protected ResourceManager()
	{
	}

	public ResourceManager(string baseName, Assembly assembly)
	{
	}

	public ResourceManager(string baseName, Assembly assembly, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] Type? usingResourceSet)
	{
	}

	public ResourceManager(Type resourceSource)
	{
	}

	public static ResourceManager CreateFileBasedResourceManager(string baseName, string resourceDir, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] Type? usingResourceSet)
	{
		throw null;
	}

	protected static CultureInfo GetNeutralResourcesLanguage(Assembly a)
	{
		throw null;
	}

	public virtual object? GetObject(string name)
	{
		throw null;
	}

	public virtual object? GetObject(string name, CultureInfo? culture)
	{
		throw null;
	}

	protected virtual string GetResourceFileName(CultureInfo culture)
	{
		throw null;
	}

	public virtual ResourceSet? GetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
	{
		throw null;
	}

	protected static Version? GetSatelliteContractVersion(Assembly a)
	{
		throw null;
	}

	public UnmanagedMemoryStream? GetStream(string name)
	{
		throw null;
	}

	public UnmanagedMemoryStream? GetStream(string name, CultureInfo? culture)
	{
		throw null;
	}

	public virtual string? GetString(string name)
	{
		throw null;
	}

	public virtual string? GetString(string name, CultureInfo? culture)
	{
		throw null;
	}

	protected virtual ResourceSet? InternalGetResourceSet(CultureInfo culture, bool createIfNotExists, bool tryParents)
	{
		throw null;
	}

	public virtual void ReleaseAllResources()
	{
	}
}
