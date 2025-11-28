using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;

namespace System.IO.Pipes;

public class PipeSecurity : NativeObjectSecurity
{
	public override Type AccessRightType
	{
		get
		{
			throw null;
		}
	}

	public override Type AccessRuleType
	{
		get
		{
			throw null;
		}
	}

	public override Type AuditRuleType
	{
		get
		{
			throw null;
		}
	}

	public PipeSecurity()
		: base(isContainer: false, ResourceType.Unknown)
	{
	}

	public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
	{
		throw null;
	}

	public void AddAccessRule(PipeAccessRule rule)
	{
	}

	public void AddAuditRule(PipeAuditRule rule)
	{
	}

	public sealed override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
	{
		throw null;
	}

	protected internal void Persist(SafeHandle handle)
	{
	}

	protected internal void Persist(string name)
	{
	}

	public bool RemoveAccessRule(PipeAccessRule rule)
	{
		throw null;
	}

	public void RemoveAccessRuleSpecific(PipeAccessRule rule)
	{
	}

	public bool RemoveAuditRule(PipeAuditRule rule)
	{
		throw null;
	}

	public void RemoveAuditRuleAll(PipeAuditRule rule)
	{
	}

	public void RemoveAuditRuleSpecific(PipeAuditRule rule)
	{
	}

	public void ResetAccessRule(PipeAccessRule rule)
	{
	}

	public void SetAccessRule(PipeAccessRule rule)
	{
	}

	public void SetAuditRule(PipeAuditRule rule)
	{
	}
}
