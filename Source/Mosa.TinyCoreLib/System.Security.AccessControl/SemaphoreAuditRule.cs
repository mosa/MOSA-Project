using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class SemaphoreAuditRule : AuditRule
{
	public SemaphoreRights SemaphoreRights
	{
		get
		{
			throw null;
		}
	}

	public SemaphoreAuditRule(IdentityReference identity, SemaphoreRights eventRights, AuditFlags flags)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AuditFlags.None)
	{
	}
}
