using System.Collections;

namespace System.Security.Policy;

public sealed class PolicyLevel
{
	[Obsolete("Because all GAC assemblies always get full trust, the full trust list is no longer meaningful. You should install any assemblies that are used in security policy in the GAC to ensure they are trusted.")]
	public IList FullTrustAssemblies
	{
		get
		{
			throw null;
		}
	}

	public string Label
	{
		get
		{
			throw null;
		}
	}

	public IList NamedPermissionSets
	{
		get
		{
			throw null;
		}
	}

	public CodeGroup RootCodeGroup
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string StoreLocation
	{
		get
		{
			throw null;
		}
	}

	public PolicyLevelType Type
	{
		get
		{
			throw null;
		}
	}

	internal PolicyLevel()
	{
	}

	[Obsolete("Because all GAC assemblies always get full trust, the full trust list is no longer meaningful. You should install any assemblies that are used in security policy in the GAC to ensure they are trusted.")]
	public void AddFullTrustAssembly(StrongName sn)
	{
	}

	[Obsolete("Because all GAC assemblies always get full trust, the full trust list is no longer meaningful. You should install any assemblies that are used in security policy in the GAC to ensure they are trusted.")]
	public void AddFullTrustAssembly(StrongNameMembershipCondition snMC)
	{
	}

	[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public void AddNamedPermissionSet(NamedPermissionSet permSet)
	{
	}

	[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public NamedPermissionSet ChangeNamedPermissionSet(string name, PermissionSet pSet)
	{
		throw null;
	}

	[Obsolete("AppDomain policy levels are obsolete. See https://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
	public static PolicyLevel CreateAppDomainLevel()
	{
		throw null;
	}

	public void FromXml(SecurityElement e)
	{
	}

	[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public NamedPermissionSet GetNamedPermissionSet(string name)
	{
		throw null;
	}

	public void Recover()
	{
	}

	[Obsolete("Because all GAC assemblies always get full trust, the full trust list is no longer meaningful. You should install any assemblies that are used in security policy in the GAC to ensure they are trusted.")]
	public void RemoveFullTrustAssembly(StrongName sn)
	{
	}

	[Obsolete("Because all GAC assemblies always get full trust, the full trust list is no longer meaningful. You should install any assemblies that are used in security policy in the GAC to ensure they are trusted.")]
	public void RemoveFullTrustAssembly(StrongNameMembershipCondition snMC)
	{
	}

	[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public NamedPermissionSet RemoveNamedPermissionSet(NamedPermissionSet permSet)
	{
		throw null;
	}

	[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public NamedPermissionSet RemoveNamedPermissionSet(string name)
	{
		throw null;
	}

	public void Reset()
	{
	}

	public PolicyStatement Resolve(Evidence evidence)
	{
		throw null;
	}

	public CodeGroup ResolveMatchingCodeGroups(Evidence evidence)
	{
		throw null;
	}

	public SecurityElement ToXml()
	{
		throw null;
	}
}
