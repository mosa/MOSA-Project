using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class RegistryAuditRule : AuditRule
{
	public RegistryRights RegistryRights
	{
		get
		{
			throw null;
		}
	}

	public RegistryAuditRule(IdentityReference identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AuditFlags.None)
	{
	}

	public RegistryAuditRule(string identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AuditFlags.None)
	{
	}
}
