using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class EventWaitHandleAuditRule : AuditRule
{
	public EventWaitHandleRights EventWaitHandleRights
	{
		get
		{
			throw null;
		}
	}

	public EventWaitHandleAuditRule(IdentityReference identity, EventWaitHandleRights eventRights, AuditFlags flags)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AuditFlags.None)
	{
	}
}
