using System.Security.Principal;

namespace System.Security.AccessControl;

public abstract class GenericSecurityDescriptor
{
	public int BinaryLength
	{
		get
		{
			throw null;
		}
	}

	public abstract ControlFlags ControlFlags { get; }

	public abstract SecurityIdentifier? Group { get; set; }

	public abstract SecurityIdentifier? Owner { get; set; }

	public static byte Revision
	{
		get
		{
			throw null;
		}
	}

	internal GenericSecurityDescriptor()
	{
	}

	public void GetBinaryForm(byte[] binaryForm, int offset)
	{
	}

	public string GetSddlForm(AccessControlSections includeSections)
	{
		throw null;
	}

	public static bool IsSddlConversionSupported()
	{
		throw null;
	}
}
