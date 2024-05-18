using System.Security.AccessControl;
using System.Security.Principal;

namespace System.DirectoryServices;

public sealed class ListChildrenAccessRule : ActiveDirectoryAccessRule
{
	public ListChildrenAccessRule(IdentityReference identity, AccessControlType type)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}

	public ListChildrenAccessRule(IdentityReference identity, AccessControlType type, ActiveDirectorySecurityInheritance inheritanceType)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}

	public ListChildrenAccessRule(IdentityReference identity, AccessControlType type, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}
}
