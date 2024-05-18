namespace System.ComponentModel.Design.Serialization;

public abstract class MemberRelationshipService
{
	public MemberRelationship this[MemberRelationship source]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public MemberRelationship this[object sourceOwner, MemberDescriptor sourceMember]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected virtual MemberRelationship GetRelationship(MemberRelationship source)
	{
		throw null;
	}

	protected virtual void SetRelationship(MemberRelationship source, MemberRelationship relationship)
	{
	}

	public abstract bool SupportsRelationship(MemberRelationship source, MemberRelationship relationship);
}
