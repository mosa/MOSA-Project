using System.Security.AccessControl;
using System.Security.Principal;

namespace System.DirectoryServices;

public sealed class ExtendedRightAccessRule : ActiveDirectoryAccessRule
{
	public ExtendedRightAccessRule(IdentityReference identity, AccessControlType type)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}

	public ExtendedRightAccessRule(IdentityReference identity, AccessControlType type, ActiveDirectorySecurityInheritance inheritanceType)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}

	public ExtendedRightAccessRule(IdentityReference identity, AccessControlType type, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}

	public ExtendedRightAccessRule(IdentityReference identity, AccessControlType type, Guid extendedRightType)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}

	public ExtendedRightAccessRule(IdentityReference identity, AccessControlType type, Guid extendedRightType, ActiveDirectorySecurityInheritance inheritanceType)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}

	public ExtendedRightAccessRule(IdentityReference identity, AccessControlType type, Guid extendedRightType, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}
}
