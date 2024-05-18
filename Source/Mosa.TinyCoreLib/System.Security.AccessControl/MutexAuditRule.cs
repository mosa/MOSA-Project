using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class MutexAuditRule : AuditRule
{
	public MutexRights MutexRights
	{
		get
		{
			throw null;
		}
	}

	public MutexAuditRule(IdentityReference identity, MutexRights eventRights, AuditFlags flags)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AuditFlags.None)
	{
	}
}
