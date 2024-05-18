using System.Reflection;
using System.Security.Policy;

namespace System.Security;

public class HostSecurityManager
{
	public virtual PolicyLevel DomainPolicy
	{
		get
		{
			throw null;
		}
	}

	public virtual HostSecurityManagerOptions Flags
	{
		get
		{
			throw null;
		}
	}

	public virtual ApplicationTrust DetermineApplicationTrust(Evidence applicationEvidence, Evidence activatorEvidence, TrustManagerContext context)
	{
		throw null;
	}

	public virtual EvidenceBase GenerateAppDomainEvidence(Type evidenceType)
	{
		throw null;
	}

	public virtual EvidenceBase GenerateAssemblyEvidence(Type evidenceType, Assembly assembly)
	{
		throw null;
	}

	public virtual Type[] GetHostSuppliedAppDomainEvidenceTypes()
	{
		throw null;
	}

	public virtual Type[] GetHostSuppliedAssemblyEvidenceTypes(Assembly assembly)
	{
		throw null;
	}

	public virtual Evidence ProvideAppDomainEvidence(Evidence inputEvidence)
	{
		throw null;
	}

	public virtual Evidence ProvideAssemblyEvidence(Assembly loadedAssembly, Evidence inputEvidence)
	{
		throw null;
	}

	[Obsolete]
	public virtual PermissionSet ResolvePolicy(Evidence evidence)
	{
		throw null;
	}
}
