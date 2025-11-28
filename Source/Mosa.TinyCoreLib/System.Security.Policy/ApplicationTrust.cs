using System.Collections.Generic;

namespace System.Security.Policy;

public sealed class ApplicationTrust : EvidenceBase, ISecurityEncodable
{
	public ApplicationIdentity ApplicationIdentity
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public PolicyStatement DefaultGrantSet
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public object ExtraInfo
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public IList<StrongName> FullTrustAssemblies
	{
		get
		{
			throw null;
		}
	}

	public bool IsApplicationTrustedToRun
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool Persist
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ApplicationTrust()
	{
	}

	public ApplicationTrust(ApplicationIdentity identity)
	{
	}

	[Obsolete("Code Access Security is not supported or honored by the runtime.", DiagnosticId = "SYSLIB0003", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public ApplicationTrust(PermissionSet defaultGrantSet, IEnumerable<StrongName> fullTrustAssemblies)
	{
	}

	public void FromXml(SecurityElement element)
	{
	}

	public SecurityElement ToXml()
	{
		throw null;
	}
}
