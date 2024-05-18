using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class EventWaitHandleAccessRule : AccessRule
{
	public EventWaitHandleRights EventWaitHandleRights
	{
		get
		{
			throw null;
		}
	}

	public EventWaitHandleAccessRule(IdentityReference identity, EventWaitHandleRights eventRights, AccessControlType type)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
	{
	}

	public EventWaitHandleAccessRule(string identity, EventWaitHandleRights eventRights, AccessControlType type)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
	{
	}
}
