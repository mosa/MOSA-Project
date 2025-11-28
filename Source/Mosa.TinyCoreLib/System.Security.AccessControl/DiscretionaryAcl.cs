using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class DiscretionaryAcl : CommonAcl
{
	public DiscretionaryAcl(bool isContainer, bool isDS, byte revision, int capacity)
	{
	}

	public DiscretionaryAcl(bool isContainer, bool isDS, int capacity)
	{
	}

	public DiscretionaryAcl(bool isContainer, bool isDS, RawAcl? rawAcl)
	{
	}

	public void AddAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
	{
	}

	public void AddAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
	{
	}

	public void AddAccess(AccessControlType accessType, SecurityIdentifier sid, ObjectAccessRule rule)
	{
	}

	public bool RemoveAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
	{
		throw null;
	}

	public bool RemoveAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
	{
		throw null;
	}

	public bool RemoveAccess(AccessControlType accessType, SecurityIdentifier sid, ObjectAccessRule rule)
	{
		throw null;
	}

	public void RemoveAccessSpecific(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
	{
	}

	public void RemoveAccessSpecific(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
	{
	}

	public void RemoveAccessSpecific(AccessControlType accessType, SecurityIdentifier sid, ObjectAccessRule rule)
	{
	}

	public void SetAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
	{
	}

	public void SetAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
	{
	}

	public void SetAccess(AccessControlType accessType, SecurityIdentifier sid, ObjectAccessRule rule)
	{
	}
}
