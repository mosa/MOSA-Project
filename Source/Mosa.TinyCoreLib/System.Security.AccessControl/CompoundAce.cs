using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class CompoundAce : KnownAce
{
	public override int BinaryLength
	{
		get
		{
			throw null;
		}
	}

	public CompoundAceType CompoundAceType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public CompoundAce(AceFlags flags, int accessMask, CompoundAceType compoundAceType, SecurityIdentifier sid)
	{
	}

	public override void GetBinaryForm(byte[] binaryForm, int offset)
	{
	}
}
