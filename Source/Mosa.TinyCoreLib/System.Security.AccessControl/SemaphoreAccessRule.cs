using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class SemaphoreAccessRule : AccessRule
{
	public SemaphoreRights SemaphoreRights
	{
		get
		{
			throw null;
		}
	}

	public SemaphoreAccessRule(IdentityReference identity, SemaphoreRights eventRights, AccessControlType type)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
	{
	}

	public SemaphoreAccessRule(string identity, SemaphoreRights eventRights, AccessControlType type)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
	{
	}
}
