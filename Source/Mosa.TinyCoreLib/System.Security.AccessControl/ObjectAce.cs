using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class ObjectAce : QualifiedAce
{
	public override int BinaryLength
	{
		get
		{
			throw null;
		}
	}

	public Guid InheritedObjectAceType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ObjectAceFlags ObjectAceFlags
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Guid ObjectAceType
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public ObjectAce(AceFlags aceFlags, AceQualifier qualifier, int accessMask, SecurityIdentifier sid, ObjectAceFlags flags, Guid type, Guid inheritedType, bool isCallback, byte[]? opaque)
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
