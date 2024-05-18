using System.Security.AccessControl;
using System.Security.Principal;

namespace System.DirectoryServices;

public sealed class PropertyAccessRule : ActiveDirectoryAccessRule
{
	public PropertyAccessRule(IdentityReference identity, AccessControlType type, PropertyAccess access)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}

	public PropertyAccessRule(IdentityReference identity, AccessControlType type, PropertyAccess access, ActiveDirectorySecurityInheritance inheritanceType)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}

	public PropertyAccessRule(IdentityReference identity, AccessControlType type, PropertyAccess access, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}

	public PropertyAccessRule(IdentityReference identity, AccessControlType type, PropertyAccess access, Guid propertyType)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}

	public PropertyAccessRule(IdentityReference identity, AccessControlType type, PropertyAccess access, Guid propertyType, ActiveDirectorySecurityInheritance inheritanceType)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}

	public PropertyAccessRule(IdentityReference identity, AccessControlType type, PropertyAccess access, Guid propertyType, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}
}
