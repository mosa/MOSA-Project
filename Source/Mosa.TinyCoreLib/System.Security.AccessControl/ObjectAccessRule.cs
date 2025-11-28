using System.Security.Principal;

namespace System.Security.AccessControl;

public abstract class ObjectAccessRule : AccessRule
{
	public Guid InheritedObjectType
	{
		get
		{
			throw null;
		}
	}

	public ObjectAceFlags ObjectFlags
	{
		get
		{
			throw null;
		}
	}

	public Guid ObjectType
	{
		get
		{
			throw null;
		}
	}

	protected ObjectAccessRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, Guid objectType, Guid inheritedObjectType, AccessControlType type)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
	{
	}
}
