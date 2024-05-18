using System.Runtime.InteropServices;
using System.Security.Principal;

namespace System.Security.AccessControl;

public abstract class ObjectSecurity
{
	public abstract Type AccessRightType { get; }

	protected bool AccessRulesModified
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public abstract Type AccessRuleType { get; }

	public bool AreAccessRulesCanonical
	{
		get
		{
			throw null;
		}
	}

	public bool AreAccessRulesProtected
	{
		get
		{
			throw null;
		}
	}

	public bool AreAuditRulesCanonical
	{
		get
		{
			throw null;
		}
	}

	public bool AreAuditRulesProtected
	{
		get
		{
			throw null;
		}
	}

	protected bool AuditRulesModified
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public abstract Type AuditRuleType { get; }

	protected bool GroupModified
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected bool IsContainer
	{
		get
		{
			throw null;
		}
	}

	protected bool IsDS
	{
		get
		{
			throw null;
		}
	}

	protected bool OwnerModified
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected CommonSecurityDescriptor SecurityDescriptor
	{
		get
		{
			throw null;
		}
	}

	protected ObjectSecurity()
	{
	}

	protected ObjectSecurity(bool isContainer, bool isDS)
	{
	}

	protected ObjectSecurity(CommonSecurityDescriptor securityDescriptor)
	{
	}

	public abstract AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type);

	public abstract AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags);

	public IdentityReference? GetGroup(Type targetType)
	{
		throw null;
	}

	public IdentityReference? GetOwner(Type targetType)
	{
		throw null;
	}

	public byte[] GetSecurityDescriptorBinaryForm()
	{
		throw null;
	}

	public string GetSecurityDescriptorSddlForm(AccessControlSections includeSections)
	{
		throw null;
	}

	public static bool IsSddlConversionSupported()
	{
		throw null;
	}

	protected abstract bool ModifyAccess(AccessControlModification modification, AccessRule rule, out bool modified);

	public virtual bool ModifyAccessRule(AccessControlModification modification, AccessRule rule, out bool modified)
	{
		throw null;
	}

	protected abstract bool ModifyAudit(AccessControlModification modification, AuditRule rule, out bool modified);

	public virtual bool ModifyAuditRule(AccessControlModification modification, AuditRule rule, out bool modified)
	{
		throw null;
	}

	protected virtual void Persist(bool enableOwnershipPrivilege, string name, AccessControlSections includeSections)
	{
	}

	protected virtual void Persist(SafeHandle handle, AccessControlSections includeSections)
	{
	}

	protected virtual void Persist(string name, AccessControlSections includeSections)
	{
	}

	public virtual void PurgeAccessRules(IdentityReference identity)
	{
	}

	public virtual void PurgeAuditRules(IdentityReference identity)
	{
	}

	protected void ReadLock()
	{
	}

	protected void ReadUnlock()
	{
	}

	public void SetAccessRuleProtection(bool isProtected, bool preserveInheritance)
	{
	}

	public void SetAuditRuleProtection(bool isProtected, bool preserveInheritance)
	{
	}

	public void SetGroup(IdentityReference identity)
	{
	}

	public void SetOwner(IdentityReference identity)
	{
	}

	public void SetSecurityDescriptorBinaryForm(byte[] binaryForm)
	{
	}

	public void SetSecurityDescriptorBinaryForm(byte[] binaryForm, AccessControlSections includeSections)
	{
	}

	public void SetSecurityDescriptorSddlForm(string sddlForm)
	{
	}

	public void SetSecurityDescriptorSddlForm(string sddlForm, AccessControlSections includeSections)
	{
	}

	protected void WriteLock()
	{
	}

	protected void WriteUnlock()
	{
	}
}
public abstract class ObjectSecurity<T> : NativeObjectSecurity where T : struct
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

	protected ObjectSecurity(bool isContainer, ResourceType resourceType)
		: base(isContainer: false, ResourceType.Unknown)
	{
	}

	protected ObjectSecurity(bool isContainer, ResourceType resourceType, SafeHandle? safeHandle, AccessControlSections includeSections)
		: base(isContainer: false, ResourceType.Unknown)
	{
	}

	protected ObjectSecurity(bool isContainer, ResourceType resourceType, SafeHandle? safeHandle, AccessControlSections includeSections, ExceptionFromErrorCode? exceptionFromErrorCode, object? exceptionContext)
		: base(isContainer: false, ResourceType.Unknown)
	{
	}

	protected ObjectSecurity(bool isContainer, ResourceType resourceType, string? name, AccessControlSections includeSections)
		: base(isContainer: false, ResourceType.Unknown)
	{
	}

	protected ObjectSecurity(bool isContainer, ResourceType resourceType, string? name, AccessControlSections includeSections, ExceptionFromErrorCode? exceptionFromErrorCode, object? exceptionContext)
		: base(isContainer: false, ResourceType.Unknown)
	{
	}

	public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
	{
		throw null;
	}

	public virtual void AddAccessRule(AccessRule<T> rule)
	{
	}

	public virtual void AddAuditRule(AuditRule<T> rule)
	{
	}

	public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
	{
		throw null;
	}

	protected internal void Persist(SafeHandle handle)
	{
	}

	protected internal void Persist(string name)
	{
	}

	public virtual bool RemoveAccessRule(AccessRule<T> rule)
	{
		throw null;
	}

	public virtual void RemoveAccessRuleAll(AccessRule<T> rule)
	{
	}

	public virtual void RemoveAccessRuleSpecific(AccessRule<T> rule)
	{
	}

	public virtual bool RemoveAuditRule(AuditRule<T> rule)
	{
		throw null;
	}

	public virtual void RemoveAuditRuleAll(AuditRule<T> rule)
	{
	}

	public virtual void RemoveAuditRuleSpecific(AuditRule<T> rule)
	{
	}

	public virtual void ResetAccessRule(AccessRule<T> rule)
	{
	}

	public virtual void SetAccessRule(AccessRule<T> rule)
	{
	}

	public virtual void SetAuditRule(AuditRule<T> rule)
	{
	}
}
