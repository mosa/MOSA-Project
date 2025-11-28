using System.Security.AccessControl;
using System.Security.Principal;

namespace System.IO.Pipes;

public sealed class PipeAccessRule : AccessRule
{
	public PipeAccessRights PipeAccessRights
	{
		get
		{
			throw null;
		}
	}

	public PipeAccessRule(IdentityReference identity, PipeAccessRights rights, AccessControlType type)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
	{
	}

	public PipeAccessRule(string identity, PipeAccessRights rights, AccessControlType type)
		: base(null, 0, isInherited: false, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow)
	{
	}
}
