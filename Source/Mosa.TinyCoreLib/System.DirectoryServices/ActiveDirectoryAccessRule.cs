using System.Security.AccessControl;
using System.Security.Principal;

namespace System.DirectoryServices;

public class ActiveDirectoryAccessRule : ObjectAccessRule
{
	public ActiveDirectoryRights ActiveDirectoryRights
	{
		get
		{
			throw null;
		}
	}

	public ActiveDirectorySecurityInheritance InheritanceType
	{
		get
		{
			throw null;
		}
	}

	public ActiveDirectoryAccessRule(IdentityReference identity, ActiveDirectoryRights adRights, AccessControlType type)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, default(Guid), default(Guid), AccessControlType.Allow)
	{
	}

	public ActiveDirectoryAccessRule(IdentityReference identity, ActiveDirectoryRights adRights, AccessControlType type, ActiveDirectorySecurityInheritance inheritanceType)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, default(Guid), default(Guid), AccessControlType.Allow)
	{
	}

	public ActiveDirectoryAccessRule(IdentityReference identity, ActiveDirectoryRights adRights, AccessControlType type, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, default(Guid), default(Guid), AccessControlType.Allow)
	{
	}

	public ActiveDirectoryAccessRule(IdentityReference identity, ActiveDirectoryRights adRights, AccessControlType type, Guid objectType)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, default(Guid), default(Guid), AccessControlType.Allow)
	{
	}

	public ActiveDirectoryAccessRule(IdentityReference identity, ActiveDirectoryRights adRights, AccessControlType type, Guid objectType, ActiveDirectorySecurityInheritance inheritanceType)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, default(Guid), default(Guid), AccessControlType.Allow)
	{
	}

	public ActiveDirectoryAccessRule(IdentityReference identity, ActiveDirectoryRights adRights, AccessControlType type, Guid objectType, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, default(Guid), default(Guid), AccessControlType.Allow)
	{
	}
}
