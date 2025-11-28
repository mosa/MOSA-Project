using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class CommonAce : QualifiedAce
{
	public override int BinaryLength
	{
		get
		{
			throw null;
		}
	}

	public CommonAce(AceFlags flags, AceQualifier qualifier, int accessMask, SecurityIdentifier sid, bool isCallback, byte[]? opaque)
	{
	}

	public override void GetBinaryForm(byte[] binaryForm, int offset)
	{
	}

	public static int MaxOpaqueLength(bool isCallback)
	{
		throw null;
	}
}
