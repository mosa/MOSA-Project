using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class CommonSecurityDescriptor : GenericSecurityDescriptor
{
	public override ControlFlags ControlFlags
	{
		get
		{
			throw null;
		}
	}

	public DiscretionaryAcl? DiscretionaryAcl
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override SecurityIdentifier? Group
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public bool IsContainer
	{
		get
		{
			throw null;
		}
	}

	public bool IsDiscretionaryAclCanonical
	{
		get
		{
			throw null;
		}
	}

	public bool IsDS
	{
		get
		{
			throw null;
		}
	}

	public bool IsSystemAclCanonical
	{
		get
		{
			throw null;
		}
	}

	public override SecurityIdentifier? Owner
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SystemAcl? SystemAcl
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CommonSecurityDescriptor(bool isContainer, bool isDS, byte[] binaryForm, int offset)
	{
	}

	public CommonSecurityDescriptor(bool isContainer, bool isDS, ControlFlags flags, SecurityIdentifier? owner, SecurityIdentifier? group, SystemAcl? systemAcl, DiscretionaryAcl? discretionaryAcl)
	{
	}

	public CommonSecurityDescriptor(bool isContainer, bool isDS, RawSecurityDescriptor rawSecurityDescriptor)
	{
	}

	public CommonSecurityDescriptor(bool isContainer, bool isDS, string sddlForm)
	{
	}

	public void AddDiscretionaryAcl(byte revision, int trusted)
	{
	}

	public void AddSystemAcl(byte revision, int trusted)
	{
	}

	public void PurgeAccessControl(SecurityIdentifier sid)
	{
	}

	public void PurgeAudit(SecurityIdentifier sid)
	{
	}

	public void SetDiscretionaryAclProtection(bool isProtected, bool preserveInheritance)
	{
	}

	public void SetSystemAclProtection(bool isProtected, bool preserveInheritance)
	{
	}
}
