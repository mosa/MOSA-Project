namespace System.Security.AccessControl;

public sealed class RawAcl : GenericAcl
{
	public override int BinaryLength
	{
		get
		{
			throw null;
		}
	}

	public override int Count
	{
		get
		{
			throw null;
		}
	}

	public override GenericAce this[int index]
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override byte Revision
	{
		get
		{
			throw null;
		}
	}

	public RawAcl(byte revision, int capacity)
	{
	}

	public RawAcl(byte[] binaryForm, int offset)
	{
	}

	public override void GetBinaryForm(byte[] binaryForm, int offset)
	{
	}

	public void InsertAce(int index, GenericAce ace)
	{
	}

	public void RemoveAce(int index)
	{
	}
}
