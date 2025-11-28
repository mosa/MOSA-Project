using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class MutexAccessRule : AccessRule
{
	public MutexRights MutexRights
	{
		get
		{
			throw null;
		}
	}

	public MutexAccessRule(IdentityReference identity, MutexRights eventRights, AccessControlType type)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
	{
	}

	public MutexAccessRule(string identity, MutexRights eventRights, AccessControlType type)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
	{
	}
}
