using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class RawSecurityDescriptor : GenericSecurityDescriptor
{
	public override ControlFlags ControlFlags
	{
		get
		{
			throw null;
		}
	}

	public RawAcl? DiscretionaryAcl
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

	public byte ResourceManagerControl
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public RawAcl? SystemAcl
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public RawSecurityDescriptor(byte[] binaryForm, int offset)
	{
	}

	public RawSecurityDescriptor(ControlFlags flags, SecurityIdentifier? owner, SecurityIdentifier? group, RawAcl? systemAcl, RawAcl? discretionaryAcl)
	{
	}

	public RawSecurityDescriptor(string sddlForm)
	{
	}

	public void SetFlags(ControlFlags flags)
	{
	}
}
