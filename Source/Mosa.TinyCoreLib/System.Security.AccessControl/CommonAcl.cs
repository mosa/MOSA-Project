using System.Security.Principal;

namespace System.Security.AccessControl;

public abstract class CommonAcl : GenericAcl
{
	public sealed override int BinaryLength
	{
		get
		{
			throw null;
		}
	}

	public sealed override int Count
	{
		get
		{
			throw null;
		}
	}

	public bool IsCanonical
	{
		get
		{
			throw null;
		}
	}

	public bool IsContainer
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

	public sealed override GenericAce this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public sealed override byte Revision
	{
		get
		{
			throw null;
		}
	}

	internal CommonAcl()
	{
	}

	public sealed override void GetBinaryForm(byte[] binaryForm, int offset)
	{
	}

	public void Purge(SecurityIdentifier sid)
	{
	}

	public void RemoveInheritedAces()
	{
	}
}
