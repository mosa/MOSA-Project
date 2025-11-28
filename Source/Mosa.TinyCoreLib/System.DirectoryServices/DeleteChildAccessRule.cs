using System.Security.AccessControl;
using System.Security.Principal;

namespace System.DirectoryServices;

public sealed class DeleteChildAccessRule : ActiveDirectoryAccessRule
{
	public DeleteChildAccessRule(IdentityReference identity, AccessControlType type)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}

	public DeleteChildAccessRule(IdentityReference identity, AccessControlType type, ActiveDirectorySecurityInheritance inheritanceType)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}

	public DeleteChildAccessRule(IdentityReference identity, AccessControlType type, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}

	public DeleteChildAccessRule(IdentityReference identity, AccessControlType type, Guid childType)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}

	public DeleteChildAccessRule(IdentityReference identity, AccessControlType type, Guid childType, ActiveDirectorySecurityInheritance inheritanceType)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}

	public DeleteChildAccessRule(IdentityReference identity, AccessControlType type, Guid childType, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}
}
