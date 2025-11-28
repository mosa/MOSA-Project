using System.Security.AccessControl;
using System.Security.Principal;

namespace System.IO.Pipes;

public sealed class PipeAuditRule : AuditRule
{
	public PipeAccessRights PipeAccessRights
	{
		get
		{
			throw null;
		}
	}

	public PipeAuditRule(IdentityReference identity, PipeAccessRights rights, AuditFlags flags)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AuditFlags.None)
	{
	}

	public PipeAuditRule(string identity, PipeAccessRights rights, AuditFlags flags)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AuditFlags.None)
	{
	}
}
