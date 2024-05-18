using System.Security.AccessControl;
using System.Security.Principal;

namespace System.DirectoryServices;

public sealed class PropertySetAccessRule : ActiveDirectoryAccessRule
{
	public PropertySetAccessRule(IdentityReference identity, AccessControlType type, PropertyAccess access, Guid propertySetType)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}

	public PropertySetAccessRule(IdentityReference identity, AccessControlType type, PropertyAccess access, Guid propertySetType, ActiveDirectorySecurityInheritance inheritanceType)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}

	public PropertySetAccessRule(IdentityReference identity, AccessControlType type, PropertyAccess access, Guid propertySetType, ActiveDirectorySecurityInheritance inheritanceType, Guid inheritedObjectType)
		: base(null, (ActiveDirectoryRights)0, AccessControlType.Allow)
	{
	}
}
