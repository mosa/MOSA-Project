using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class RegistryAccessRule : AccessRule
{
	public RegistryRights RegistryRights
	{
		get
		{
			throw null;
		}
	}

	public RegistryAccessRule(IdentityReference identity, RegistryRights registryRights, AccessControlType type)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
	{
	}

	public RegistryAccessRule(IdentityReference identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
	{
	}

	public RegistryAccessRule(string identity, RegistryRights registryRights, AccessControlType type)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
	{
	}

	public RegistryAccessRule(string identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
	{
	}
}
